using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.CountyModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.CountyModule;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountiesController : Controller
    {
        private readonly ICountyRepository countyRepository;

        private readonly UserManager<AppUser> userManager;
        public CountiesController(ICountyRepository countyRepository, UserManager<AppUser> userManager)
        {
            this.countyRepository = countyRepository;

            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var counties = await countyRepository.GetAllCounties();

                return View(counties);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(CountyDTO countyDTO)
        {
            try
            {

                bool IsPatientExist = (await countyRepository.CheckIfCountyExist(countyDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    countyDTO.CreatedBy = user.Id;

                    var result = await countyRepository.Create(countyDTO);

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
        public async Task<IActionResult> Update(CountyDTO countyDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                countyDTO.UpdatedBy = user.Id;

                var results = await countyRepository.UpdateCounty(countyDTO);

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
                var results = await countyRepository.DeleteCounty(Id);

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
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var expenseType = await countyRepository.GetAllCountyById(Id);

                if (expenseType != null)
                {
                    CountyDTO file = new CountyDTO()
                    {
                        Id = expenseType.Id,

                        Name = expenseType.Name,

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
