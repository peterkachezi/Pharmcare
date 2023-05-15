using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.MedicalConditionModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.MedicalConditionModule;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MedicalConditionsController : Controller
    {
        private readonly IMedicalConditionRepository medicalConditionRepository;

        private readonly UserManager<AppUser> userManager;
        public MedicalConditionsController(IMedicalConditionRepository medicalConditionRepository, UserManager<AppUser> userManager)
        {
            this.medicalConditionRepository = medicalConditionRepository;

            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var medicalConditions = await medicalConditionRepository.GetAll();

                return View(medicalConditions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(MedicalConditionDTO medicalConditionDTO)
        {
            try
            {
                var name = medicalConditionDTO.Name.Substring(0, 1).ToUpper() + medicalConditionDTO.Name.Substring(1).ToLower().Trim();

                bool IsPatientExist = (await medicalConditionRepository.CheckIfRecordExist(medicalConditionDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    medicalConditionDTO.CreatedBy = user.Id;

                    medicalConditionDTO.Name = name;

                    var result = await medicalConditionRepository.Create(medicalConditionDTO);

                    if (result != null)
                    {
                        return Json(new { success = true, responseText = "Medical condition has been successfully created" });
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
        public async Task<IActionResult> Update(MedicalConditionDTO medicalConditionDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                var name = medicalConditionDTO.Name.Substring(0, 1).ToUpper() + medicalConditionDTO.Name.Substring(1).ToLower().Trim();

                medicalConditionDTO.UpdatedBy = user.Id;

                medicalConditionDTO.Name = name;

                var results = await medicalConditionRepository.Update(medicalConditionDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Medical condition details has been successfully updated" });
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
                var results = await medicalConditionRepository.Delete(Id);

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
                var medicalCondition = await medicalConditionRepository.GetById(Id);

                if (medicalCondition != null)
                {

                    return Json(new { data = medicalCondition });
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
