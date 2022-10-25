using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.Accounts.OpeningBalanceModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.Accounts.OpeningBalanceModule;


namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OpeningBalanceController : Controller
    {
        private readonly IOpeningBalanceRepository openingBalanceRepository;

        private readonly UserManager<AppUser> userManager;
        public OpeningBalanceController(IOpeningBalanceRepository openingBalanceRepository, UserManager<AppUser> userManager)
        {
            this.openingBalanceRepository = openingBalanceRepository;

            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await openingBalanceRepository.GetAll();

                return View(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(OpeningBalanceDTO openingBalanceDTO)
        {
            try
            {

                bool IsPatientExist = (await openingBalanceRepository.CheckIfRecordExist(openingBalanceDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    openingBalanceDTO.CreatedBy = user.Id;

                    var result = await openingBalanceRepository.Create(openingBalanceDTO);

                    if (result != null)
                    {
                        return Json(new { success = true, responseText = "Category has been successfully created" });
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
        public async Task<IActionResult> Update(OpeningBalanceDTO openingBalanceDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);              

                openingBalanceDTO.UpdatedBy = user.Id;    

                var results = await openingBalanceRepository.Update(openingBalanceDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Category details has been successfully updated" });
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
                var results = await openingBalanceRepository.Delete(Id);

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
                var expenseType = await openingBalanceRepository.GetAll();

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
                var expenseType = await openingBalanceRepository.GetById(Id);

                if (expenseType != null)
                {
                    OpeningBalanceDTO file = new OpeningBalanceDTO()
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
