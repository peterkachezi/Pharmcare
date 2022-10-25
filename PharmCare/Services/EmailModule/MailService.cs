
using MimeKit;
using PharmCare.DTO.ApplicationUsersModule;
using System.Net;
using System.Net.Mail;


namespace PharmCare.Services.EmailModule
{
    public class MailService : IMailService
    {
        private readonly IConfiguration config;

        private readonly IWebHostEnvironment env;
        public MailService(IConfiguration config, IWebHostEnvironment env)
        {
            this.config = config;

            this.env = env;                      

        }
        public bool AccountEmailNotification(ApplicationUserDTO applicationUserDTO)
        {
            try
            {
                var emailSender = config.GetValue<string>("MailSettings:SMTPUserName");

                var emailSenderPassword = config.GetValue<string>("MailSettings:Password");

                var emailSenderHost = config.GetValue<string>("MailSettings:SMTPMailServer");

                int emailSenderPort = Convert.ToInt32(config.GetValue<string>("MailSettings:SMTPPort"));

                bool emailIsSSL = Convert.ToBoolean(config.GetValue<string>("MailSettings:SMTPUseSSL"));

                //Fetching Email Body Text from EmailTemplate File. 

                string FilePath = env.WebRootPath
                              + Path.DirectorySeparatorChar.ToString()
                              + "Templates"
                              + Path.DirectorySeparatorChar.ToString()
                              + "EmailTemplate"
                              + Path.DirectorySeparatorChar.ToString()
                              + "UserAccountNotification.html";

                StreamReader str = new StreamReader(FilePath);

                string MailText = str.ReadToEnd();

                str.Close();

                //Repalce [newusername] = signup user name   

                MailText = MailText.Replace("[FullName]", applicationUserDTO.FullName.ToString());

                MailText = MailText.Replace("[Email]", applicationUserDTO.Email.ToString());

                MailText = MailText.Replace("[Password]", applicationUserDTO.Password.ToString());

                MailText = MailText.Replace("[RoleName]", applicationUserDTO.RoleName.ToString());

                string subject = "Account Login Details:";

                //Base class for sending email  
                MailMessage _mailmsg = new MailMessage();

                //Make TRUE because our body text is html  
                _mailmsg.IsBodyHtml = true;

                _mailmsg.From = new MailAddress(emailSender);

                _mailmsg.To.Add(applicationUserDTO.Email.ToString());

                _mailmsg.Subject = subject;

                _mailmsg.Body = MailText;

                //Now set your SMTP   
                SmtpClient _smtp = new SmtpClient();
                {
                    _smtp.Host = emailSenderHost;

                    _smtp.Port = emailSenderPort;

                    _smtp.EnableSsl = emailIsSSL;

                    NetworkCredential _network = new NetworkCredential(emailSender, emailSenderPassword);

                    _smtp.Credentials = _network;

                    _smtp.Send(_mailmsg);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public bool AppointmentApprovalNotification(ApplicationUserDTO applicationUserDTO)
        {

            try
            {
                var emailSender = config.GetValue<string>("MailSettings:SMTPUserName");

                var emailSenderPassword = config.GetValue<string>("MailSettings:Password");

                var emailSenderHost = config.GetValue<string>("MailSettings:SMTPMailServer");

                int emailSenderPort = Convert.ToInt32(config.GetValue<string>("MailSettings:SMTPPort"));

                bool emailIsSSL = Convert.ToBoolean(config.GetValue<string>("MailSettings:SMTPUseSSL"));

                //Fetching Email Body Text from EmailTemplate File. 

                string FilePath = env.WebRootPath
                              + Path.DirectorySeparatorChar.ToString()
                              + "Templates"
                              + Path.DirectorySeparatorChar.ToString()
                              + "EmailTemplate"
                              + Path.DirectorySeparatorChar.ToString()
                              + "UserAccountNotification.html";

                StreamReader str = new StreamReader(FilePath);

                string MailText = str.ReadToEnd();

                str.Close();

                //Repalce [newusername] = signup user name   

                MailText = MailText.Replace("[FullName]", applicationUserDTO.FullName.ToString());

                MailText = MailText.Replace("[Email]", applicationUserDTO.Email.ToString());

                MailText = MailText.Replace("[Password]", applicationUserDTO.Password.ToString());

                MailText = MailText.Replace("[RoleName]", applicationUserDTO.RoleName.ToString());

                string subject = "Account Login Details:";

                //Base class for sending email  
                MailMessage _mailmsg = new MailMessage();

                //Make TRUE because our body text is html  
                _mailmsg.IsBodyHtml = true;

                _mailmsg.From = new MailAddress(emailSender);

                _mailmsg.To.Add(applicationUserDTO.Email.ToString());

                _mailmsg.Subject = subject;

                _mailmsg.Body = MailText;

                //Now set your SMTP   
                SmtpClient _smtp = new SmtpClient();
                {
                    _smtp.Host = emailSenderHost;

                    _smtp.Port = emailSenderPort;

                    _smtp.EnableSsl = emailIsSSL;

                    NetworkCredential _network = new NetworkCredential(emailSender, emailSenderPassword);

                    _smtp.Credentials = _network;

                    _smtp.Send(_mailmsg);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public bool PasswordResetEmailNotification(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var SMTPEmailToNetwork = config.GetValue<string>("MailSettings:SMTPEmailToNetwork");

                var SMTPMailServer = config.GetValue<string>("MailSettings:SMTPMailServer");

                var SMTPPort = config.GetValue<string>("MailSettings:SMTPPort");

                var SMTPUseSSL = config.GetValue<string>("MailSettings:SMTPUseSSL");

                var SMTPUserName = config.GetValue<string>("MailSettings:SMTPUserName");

                var Password = config.GetValue<string>("MailSettings:Password");

                MailAddressCollection mailAddressesTo = new MailAddressCollection();

                mailAddressesTo.Add(new MailAddress(resetPasswordDTO.Email));

                MailAddress mailAddressFrom = new MailAddress(SMTPUserName);

                MailMessage mailMessage = new MailMessage();

                mailMessage.From = mailAddressFrom;

                foreach (var to in mailAddressesTo)
                    mailMessage.To.Add(to);

                mailMessage.Subject = "Healthier Kenya: ";

                var templatePath = env.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "Templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "EmailTemplate"
                           + Path.DirectorySeparatorChar.ToString()
                           + "PaswordResetNotification.html";

                var builder = new BodyBuilder();

                using (StreamReader SourceReader = File.OpenText(templatePath))
                {

                    builder.HtmlBody = SourceReader.ReadToEnd();

                }

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                mailMessage.Body = string.Format(builder.HtmlBody,

                     resetPasswordDTO.FullName,

                     resetPasswordDTO.Email,

                     resetPasswordDTO.Password

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

                return true;

            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

    }
}
