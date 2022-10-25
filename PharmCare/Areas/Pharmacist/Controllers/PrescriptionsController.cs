using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.CategoryModule;
using PharmCare.BLL.Repositories.MedecineModule;
using PharmCare.BLL.Repositories.PatientModule;
using PharmCare.BLL.Repositories.PrescriptionModule;
using PharmCare.BLL.Repositories.SalesModule;
using PharmCare.BLL.Repositories.StockModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.MedicineModule;
using PharmCare.DTO.PrescriptionModule;
using PharmCare.DTO.SalesModule;
using PharmCare.DTO.StockModule;

namespace PharmCare.Areas.Pharmacist.Controllers
{
    [Area("Pharmacist")]
    public class PrescriptionsController : Controller
    {
        private readonly IMedicineRepository medicineRepository;

        private readonly IPrescriptionRepository prescriptionRepository;

        private readonly UserManager<AppUser> userManager;

        private readonly IPatientRepository patientRepository;
        public PrescriptionsController(IMedicineRepository medicineRepository,IPatientRepository patientRepository,IPrescriptionRepository prescriptionRepository, UserManager<AppUser> userManager)
        {
            this.prescriptionRepository = prescriptionRepository;

            this.userManager = userManager;

            this.patientRepository = patientRepository; 

            this.medicineRepository = medicineRepository;        
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await prescriptionRepository.GetAll();

                return View(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Details(Guid? Id,Guid PatientId)
        {
            try
            {

                if (Id != null)
                {

                    PrescriptionProfileDTO prescriptionProfileDTO = new PrescriptionProfileDTO()
                    {

                    };

                    prescriptionProfileDTO.prescriptionDetails = prescriptionRepository.GetAllPrescriptionDetailsById(Id.Value);

                    prescriptionProfileDTO.patientDTO = await patientRepository.GetById(PatientId);

                    return View(prescriptionProfileDTO);
                }

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public IPrescriptionRepository GetPrescriptionRepository()
        {
            return prescriptionRepository;
        }
        [HttpPost]
        public async Task<ActionResult> Create(PrescriptionDTO prescriptionDTO)
        {
            try
            {

                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                prescriptionDTO.CreatedBy = user.Id;

                prescriptionDTO.Id = Guid.NewGuid();

                var result = await prescriptionRepository.Create(prescriptionDTO);

                if (result != null)
                {
                    return Json(new { success = true, responseText = "Prescription was successfully submitted" });
                }

                else
                {
                    return Json(new { success = false, responseText = "Prescription was not successfull" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<ActionResult> IssueMedicine(SalesDetailsDTO salesDetailsDTO)
        {
            try
            {

                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                salesDetailsDTO.CreatedBy = user.Id;

                var results = await prescriptionRepository.IssueMedicine(salesDetailsDTO);

                if (results !=null)
                {
                    return Json(new { success = true, responseText = "Success ! " });
                }
                else
                {
                    return Json(new { success = false, responseText = "Unable to complete the process ,please try again" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public async Task<ActionResult> UnDoIssueMedicine(Guid Id)
        {
            try
            {
                var results = await prescriptionRepository.UnDoIssueMedicine(Id);

                if (results == true)
                {
                    return Json(new { success = true, responseText = "Success ! " });
                }
                else
                {
                    return Json(new { success = false, responseText = "Unable to complete the process ,please try again" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public async Task<IActionResult> GetByPrescriptionDetailId(Guid Id)
        {
            try
            {
                var getPrescription = await prescriptionRepository.GetById(Id);

                var medicine = await medicineRepository.GetById(getPrescription.MedicineId);

                var prescriptionId = Id;

                if (medicine != null)
                {
                    MedicineDTO file = new MedicineDTO()
                    {
                        Id = medicine.Id,

                        Name = medicine.Name,

                        ShelfId = medicine.ShelfId,

                        ShelfName = medicine.ShelfName,

                        Description = medicine.Description,

                        CategoryId = medicine.CategoryId,

                        ManufacturerPrice = medicine.ManufacturerPrice,

                        SellingPrice = medicine.SellingPrice,

                        Status = medicine.Status,

                        CreateDate = medicine.CreateDate,

                        CreatedBy = medicine.CreatedBy,

                        UnitId = medicine.UnitId,

                        UnitName = medicine.UnitName,

                        Quantity = medicine.Quantity,

                        PrescriptionId = prescriptionId,

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
