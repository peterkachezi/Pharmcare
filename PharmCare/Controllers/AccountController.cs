using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PharmCare.DAL.Models;
using PharmCare.DTO.ApplicationUsersModule;
using PharmCare.Extensions;
using PharmCare.Services.EmailModule;
using System.Net;
using System.Net.Mail;
using PasswordOptions = PharmCare.Extensions.PasswordOptions;

namespace PharmCare.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;

        private readonly RoleManager<IdentityRole> roleManager;

        private readonly UserManager<AppUser> userManager;

        private readonly IConfiguration _config;

        private readonly IMailService mailService;

        private IWebHostEnvironment _env;
        public AccountController(
            
            IMailService mailService,
            
            SignInManager<AppUser> signInManager,

            RoleManager<IdentityRole> roleManager,

            UserManager<AppUser> userManager, IConfiguration config, IWebHostEnvironment env)
        {

            this.signInManager = signInManager;

            this.roleManager = roleManager;

            this.userManager = userManager;

            this.mailService = mailService;

            _config = config;

            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(loginDTO.Email);

                    if (user == null)
                    {
                        TempData["Error"] = "Invalid user account / Account does not exist";

                        return RedirectToAction("Login", "Account");
                    }

                    if (user.isActive == false)
                    {
                        TempData["Error"] = "Your account has been disabled,kindly contact system administrator";

                        return RedirectToAction("Login", "Account");
                    }

                    var result = await signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, loginDTO.RemeberMe, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        var getUserRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();

                        if (getUserRole == null)
                        {
                            TempData["Error"] = "The user has not been mapped to roles";
                            return RedirectToAction("Login", "Account");
                        }

                        if (getUserRole == "Admin")
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        }

                        if (getUserRole == "Doctor")
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Doctor" });
                        }

                  
                        if (getUserRole == "Pharmacist")
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Pharmacist" });
                        }

                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }

                    }

                    if (result.IsLockedOut)
                    {
                        TempData["Error"] = "This account has been locked out,please try again later";

                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        TempData["Error"] = "Wrong user name or password";

                        return RedirectToAction("Login", "Account");

                    }

                }

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {

            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);

                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password);

                    if (result.Succeeded)
                    {
                        await signInManager.RefreshSignInAsync(user);

                        return View("Login", "Account");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(resetPasswordDTO);

                }
                return View("ResetPasswordConfirmation");
            }
            return View(resetPasswordDTO);
        }

        public async Task<IActionResult> SendPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);

                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    string password = PasswordStore.GenerateRandomPassword(new PasswordOptions
                    {
                        RequiredLength = 8,

                        RequireNonLetterOrDigit = true,

                        RequireDigit = true,

                        RequireLowercase = true,

                        RequireUppercase = true,

                        RequireNonAlphanumeric = true,

                        RequiredUniqueChars = 1
                    });

                    resetPasswordDTO.Token = token;

                    resetPasswordDTO.Password = password;

                    resetPasswordDTO.FullName = user.FirstName + " " + user.LastName;

                    //resetPasswordDTO.PhoneNumber = user.PhoneNumber;

                    var result = await userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password);


                    var sendmail = mailService.PasswordResetEmailNotification(resetPasswordDTO);

                    TempData["Success"] = "If you have an account with us , we have sent a new  password  to " + resetPasswordDTO.Email + "";

                    return RedirectToAction("ForgotPassword", "Account");



                }

                TempData["Error"] = "You have entered an invalid account. " + resetPasswordDTO.Email + " does not exist";

                return RedirectToAction("ForgotPassword", "Account");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.FindByEmailAsync(forgotPasswordDTO.Email);

                    if (user != null)
                    {
                        var token = await userManager.GeneratePasswordResetTokenAsync(user);

                        var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = forgotPasswordDTO.Email, token }, Request.Scheme);

                        forgotPasswordDTO.ResetLink = passwordResetLink;

                        forgotPasswordDTO.FullName = user.FirstName + " " + user.LastName;

                        var sendEmail = SendEmailNotification(forgotPasswordDTO);

                        TempData["Success"] = "we have sent a password reset link to " + forgotPasswordDTO.Email + "";

                        return RedirectToAction("ForgotPassword", "Account");
                    }

                    TempData["Error"] = "Invalid acount";

                    return RedirectToAction("ForgotPassword", "Account");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    return null;
                }
            }

            return View(forgotPasswordDTO);
        }

        public IActionResult sendmail()
        {

            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("mutulijane@yahoo.com");

                //receiver email adress
                mailMessage.To.Add("peterkachezi@gmail.com");

                //subject of the email
                mailMessage.Subject = "This is a subject";

                //attach the file
                //mailMessage.Attachments.Add(new Attachment(@"C:\\attachedfile.jpg"));
                mailMessage.Body = "Body of the email";
                mailMessage.IsBodyHtml = true;

                //SMTP client
                SmtpClient smtpClient = new SmtpClient("smtp.mail.yahoo.com");
                //port number for Yahoo
                smtpClient.Port = 587;
                //credentials to login in to yahoo account
                smtpClient.Credentials = new NetworkCredential("mutulijane@yahoo.com", "26404166J");
                //enabled SSL
                smtpClient.EnableSsl = true;
                //Send an email
                smtpClient.Send(mailMessage);

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;

            }
        }

        public ActionResult SendEmailNotification(ForgotPasswordDTO forgotPasswordDTO)
        {
            //var claimDetails = _claimService.GetClaimById(claimDTO.Id);

            var SMTPEmailToNetwork = _config.GetValue<string>("MailSettings:SMTPEmailToNetwork");

            var SMTPMailServer = _config.GetValue<string>("MailSettings:SMTPMailServer");

            var SMTPPort = _config.GetValue<string>("MailSettings:SMTPPort");

            var SMTPUserName = _config.GetValue<string>("MailSettings:SMTPUserName");

            var Password = _config.GetValue<string>("MailSettings:Password");

            var SMTPUseSSL = _config.GetValue<string>("MailSettings:SMTPUseSSL");

            try
            {
                MailAddressCollection mailAddressesTo = new MailAddressCollection();

                mailAddressesTo.Add(new MailAddress(forgotPasswordDTO.Email));

                MailAddress mailAddressFrom = new MailAddress(SMTPUserName);

                MailMessage mailMessage = new MailMessage();

                mailMessage.From = mailAddressFrom;

                foreach (var to in mailAddressesTo)
                    mailMessage.To.Add(to);


                mailMessage.Subject = "Healthier Kenya password reset instructions:-";

                var templatePath = _env.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "PasswordResetLink.html";

                var builder = new BodyBuilder();

                using (StreamReader SourceReader = System.IO.File.OpenText(templatePath))
                {

                    builder.HtmlBody = SourceReader.ReadToEnd();

                }

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                mailMessage.Body = string.Format(builder.HtmlBody,

                     forgotPasswordDTO.FullName,

                     forgotPasswordDTO.ResetLink


                    );

                mailMessage.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = SMTPMailServer;
                    client.Port = int.Parse(SMTPPort);
                    if (SMTPUseSSL != string.Empty)
                    {
                        client.EnableSsl = bool.Parse(SMTPUseSSL);
                    }

                    client.UseDefaultCredentials = false;
                    bool bNetwork = bool.Parse(SMTPEmailToNetwork);
                    if (bNetwork)
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    }
                    else
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    }

                    client.Credentials = new NetworkCredential(SMTPUserName, Password);
                    client.ServicePoint.MaxIdleTime = 2;
                    client.ServicePoint.ConnectionLimit = 1;
                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View();
        }



        public IActionResult UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            try
            {

                if (updatePasswordDTO.CurrentPassword == null)
                {
                    return Json(new { success = false, responseText = "Current password is a required field" });

                }

                if (updatePasswordDTO.NewPassword == null)
                {
                    return Json(new { success = false, responseText = "New password is a required field" });

                }

                if (updatePasswordDTO.ConfirmPassword == null)
                {
                    return Json(new { success = false, responseText = "Confirm password is a required field" });

                }

                if (updatePasswordDTO.ConfirmPassword != updatePasswordDTO.NewPassword)
                {
                    return Json(new { success = false, responseText = "Password and confirm password do not match" });

                }


                if (ModelState.IsValid)
                {
                    var user = await userManager.GetUserAsync(User);

                    if (user == null)
                    {
                        return RedirectToAction("/Account/Logout");
                    }

                    var result = await userManager.ChangePasswordAsync(user, updatePasswordDTO.CurrentPassword, updatePasswordDTO.NewPassword);

                    if (!result.Succeeded)
                    {

                        var validation = (result.Errors.FirstOrDefault().Description);

                        return Json(new { success = false, responseText = validation });

                    }

                    await signInManager.RefreshSignInAsync(user);

                    return Json(new { success = true, responseText = "Password has been changed successfully" });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

            return View(updatePasswordDTO);
        }


    }
}
