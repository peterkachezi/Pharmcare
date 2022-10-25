using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.Accounts.BankModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.Accounts.BankModule;


namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BanksController : Controller
    {
        private readonly IBankRepository bankRepository;

        private readonly UserManager<AppUser> userManager;
        public BanksController(IBankRepository bankRepository, UserManager<AppUser> userManager)
        {
            this.bankRepository = bankRepository;

            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await bankRepository.GetAll();

                return View(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(BankDTO bankDTO)
        {
            try
            {

                bool IsPatientExist = (await bankRepository.CheckIfRecordExist(bankDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    bankDTO.CreatedBy = user.Id;

                    var result = await bankRepository.Create(bankDTO);

                    if (result != null)
                    {
                        return Json(new { success = true, responseText = "Bank has been successfully created" });
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Failed to create record" });
                    }
                }

                if (IsPatientExist == true)
                {
                    return Json(new { success = false, responseText = " A record with the same details already exists" });
                }

                else
                {
                    return Json(new { success = false, responseText = "Something went wrong" });

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public async Task<IActionResult> Update(BankDTO bankDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                bankDTO.UpdatedBy = user.Id;

                var results = await bankRepository.Update(bankDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Bank details has been successfully updated" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to update record!" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                var results = await bankRepository.Delete(Id);

                if (results == true)
                {
                    return Json(new { success = true, responseText = "Record  has been successfully deleted " });
                }
                else
                {
                    return Json(new { success = false, responseText = "Record has not been deleted ,it could be in use by other files" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public async Task<IActionResult> GetExpenseTypes()
        {
            try
            {
                var expenseType = await bankRepository.GetAll();

                return Json(new { data = expenseType });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var expenseType = await bankRepository.GetById(Id);

                if (expenseType != null)
                {
                    BankDTO file = new BankDTO()
                    {
                        Id = expenseType.Id,

                        CreateDate = expenseType.CreateDate,
                    };

                    return Json(new { data = file });
                }

                return Json(new { data = false });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
    }
}
