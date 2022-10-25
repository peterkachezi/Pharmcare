
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.MedecineModule;
using PharmCare.BLL.Repositories.MedicalConditionModule;
using PharmCare.BLL.Repositories.PatientModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.PatientModule;

namespace PharmCare.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    public class PatientsController : Controller
    {

        private readonly IPatientRepository patientRepository;

        private readonly UserManager<AppUser> userManager;

        private readonly IMedicineRepository medicineRepository;

        private readonly IMedicalConditionRepository medicalConditionRepository;
        public PatientsController(IMedicalConditionRepository medicalConditionRepository,IMedicineRepository medicineRepository,IPatientRepository patientRepository, UserManager<AppUser> userManager)
        {
            this.patientRepository = patientRepository;

            this.userManager = userManager;

            this.medicineRepository = medicineRepository;

            this.medicalConditionRepository = medicalConditionRepository;   

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.MedicalConditions = await medicalConditionRepository.GetAll();

                ViewBag.Medicines = await medicineRepository.GetAll();

                var patients =await patientRepository.GetAll();  

                return View(patients);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Create(PatientDTO patientDTO)
        {
            try
            {
                var firstName = patientDTO.FirstName.Substring(0, 1).ToUpper() + patientDTO.FirstName.Substring(1).ToLower().Trim();

                var lastName = patientDTO.LastName.Substring(0, 1).ToUpper() + patientDTO.LastName.Substring(1).ToLower().Trim();

                bool IsPatientExist = (await patientRepository.CheckIfPatientExist(patientDTO));

                if (IsPatientExist == false)
                {
                    var user = await userManager.FindByEmailAsync(User.Identity.Name);

                    patientDTO.CreatedBy = user.Id;

                    patientDTO.FirstName = firstName;

                    patientDTO.LastName = lastName;

                    var result = await patientRepository.Create(patientDTO);

                    if (result != null)
                    {
                        return Json(new { success = true, responseText = "Patient has been successfully created" });
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
        public async Task<IActionResult> Update(PatientDTO patientDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);              

                var firstName = patientDTO.FirstName.Substring(0, 1).ToUpper() + patientDTO.FirstName.Substring(1).ToLower().Trim();

                var lastName = patientDTO.LastName.Substring(0, 1).ToUpper() + patientDTO.LastName.Substring(1).ToLower().Trim();

                patientDTO.UpdatedBy = user.Id;

                patientDTO.FirstName = firstName;

                patientDTO.LastName = lastName;

                var results = await patientRepository.Update(patientDTO);

                if (results != null)
                {
                    return Json(new { success = true, responseText = "Patient details has been successfully updated" });
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
                var results = await patientRepository.Delete(Id);

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
                var expenseType = await patientRepository.GetAll();

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
                var expenseType = await patientRepository.GetById(Id);

                if (expenseType != null)
                {
                    PatientDTO file = new PatientDTO()
                    {
                        Id = expenseType.Id,

                        FirstName = expenseType.FirstName,

                        LastName = expenseType.LastName,
     
                        PhoneNumber = expenseType.PhoneNumber,

                        DateOfBirth = expenseType.DateOfBirth,             

                        PatientNumber = expenseType.PatientNumber,

                        CreateDate = expenseType.CreateDate,

                        Height = expenseType.Height,

                        Weight = expenseType.Weight,

                        Gender = expenseType.Gender,

                        Residence = expenseType.Residence,
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
        public async Task<IActionResult> GetByPhoneNumber(string PhoneNumber)
        {
            var getPatient=(await patientRepository.GetAll()).Where(x=>x.PhoneNumber==PhoneNumber);

            return Json(getPatient);
        }

    }
}
