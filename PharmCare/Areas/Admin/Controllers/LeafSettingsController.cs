using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.CategoryModule;
using PharmCare.BLL.Repositories.LeafSettingModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.CategoryModule;
using PharmCare.DTO.LeafSettingModule;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LeafSettingsController : Controller
    {
        private readonly ILeafSettingRepository leafSettingRepository;

        private readonly UserManager<AppUser> userManager;
        public LeafSettingsController(ILeafSettingRepository leafSettingRepository, UserManager<AppUser> userManager)
        {
            this.leafSettingRepository = leafSettingRepository;

            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var leafSettings = await leafSettingRepository.GetAll();

                return View(leafSettings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(LeafSettingDTO leafSettingDTO)
        {
            try
            {

                bool IsPatientExist = (await leafSettingRepository.CheckIfRecordExist(leafSettingDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    leafSettingDTO.CreatedBy = user.Id;          

                    var result = await leafSettingRepository.Create(leafSettingDTO);

                    if (result != null)
                    {
                        return Json(new { success = true, responseText = "Record has been successfully created" });
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
        public async Task<IActionResult> Update(LeafSettingDTO leafSettingDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                leafSettingDTO.UpdatedBy = user.Id;

                var results = await leafSettingRepository.Update(leafSettingDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Record details has been successfully updated" });
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
                var results = await leafSettingRepository.Delete(Id);

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
                var expenseType = await leafSettingRepository.GetAll();

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
                var expenseType = await leafSettingRepository.GetById(Id);

                if (expenseType != null)
                {
                    LeafSettingDTO file = new LeafSettingDTO()
                    {
                        Id = expenseType.Id,

                        LeafType = expenseType.LeafType,

                        TotalNumberPerBox = expenseType.TotalNumberPerBox,

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
