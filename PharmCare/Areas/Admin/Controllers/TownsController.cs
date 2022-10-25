using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using PharmCare.BLL.Repositories.CountryModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.CountryModule;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TownsController : Controller
    {
        private readonly ICountryRepository countryRepository;

        private readonly UserManager<AppUser> userManager;

        public TownsController(ICountryRepository countryRepository, UserManager<AppUser> userManager)
        {
            this.countryRepository = countryRepository;

            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var patients = await countryRepository.GetAll();

                return View(patients);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        public async Task<IActionResult> Create(CountryDTO countryDTO)
        {
            try
            {
                var name = countryDTO.Name.Substring(0, 1).ToUpper() + countryDTO.Name.Substring(1).ToLower().Trim();

                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                countryDTO.CreatedBy = user.Id;

                countryDTO.Name = name;

                bool IsPatientExist = (await countryRepository.CheckIfRecordExist(countryDTO));

                if (IsPatientExist == false)
                {                

                    var result = await countryRepository.Create(countryDTO);

                    if (result != null)
                    {
                        return Json(new { success = true, responseText = "Town has been successfully created" });
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

        public async Task<IActionResult> Update(CountryDTO countryDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                countryDTO.UpdatedBy = user.Id;

                var name = countryDTO.Name.Substring(0, 1).ToUpper() + countryDTO.Name.Substring(1).ToLower().Trim();

                var IsTypeExist = (await countryRepository.GetAll()).Where(x => x.Name == name).Count();

                if (IsTypeExist > 0)
                {
                    return Json(new { success = true, responseText = "Town has been successfully updated" });
                }

                var results = await countryRepository.Update(countryDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Town has been successfully updated" });
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
                var results = await countryRepository.Delete(Id);

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

        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var expenseType = await countryRepository.GetAll();

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
                var expenseType = await countryRepository.GetById(Id);

                if (expenseType != null)
                {
                    CountryDTO file = new CountryDTO()
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
