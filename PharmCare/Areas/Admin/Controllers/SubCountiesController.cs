using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.CountyModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.CountyModule;

namespace PharmCare.Areas.Admin.Controllers
{
    public class SubCountiesController : Controller
    {
        private readonly ICountyRepository countyRepository;

        private readonly UserManager<AppUser> userManager;
        public SubCountiesController(ICountyRepository countyRepository, UserManager<AppUser> userManager)
        {
            this.countyRepository = countyRepository;

            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var counties = await countyRepository.GetAllSubCounties();

                return View(counties);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(SubCountyDTO subCountyDTO)
        {
            try
            {
                var name = subCountyDTO.Name.Substring(0, 1).ToUpper() + subCountyDTO.Name.Substring(1).ToLower().Trim();

                subCountyDTO.Name = name;

                bool IsPatientExist = (await countyRepository.CheckIfSubCountyExist(subCountyDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    subCountyDTO.CreatedBy = user.Id;

                    var result = await countyRepository.CreateSubCounty(subCountyDTO);

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
        public async Task<IActionResult> Update(SubCountyDTO subCountyDTO)
        {
            try
            {
                var name = subCountyDTO.Name.Substring(0, 1).ToUpper() + subCountyDTO.Name.Substring(1).ToLower().Trim();

                subCountyDTO.Name = name;

                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                subCountyDTO.UpdatedBy = user.Id;

                var results = await countyRepository.UpdateSubCounty(subCountyDTO);

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
                var results = await countyRepository.DeleteSubCounty(Id);

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
                var expenseType = await countyRepository.GetAllSubCountyById(Id);

                if (expenseType != null)
                {
                    SubCountyDTO file = new SubCountyDTO()
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
