using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.ApplicationUserModule;
using PharmCare.BLL.Repositories.MedecineModule;
using PharmCare.BLL.Repositories.PatientModule;
using PharmCare.BLL.Repositories.PrescriptionModule;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IPatientRepository patientRepository;

        private readonly IMedicineRepository medicineRepository;

        private readonly IPrescriptionRepository prescriptionRepository;

        private readonly IApplicationUserRepository applicationUserRepository;

        public DashboardController(IApplicationUserRepository applicationUserRepository,IPrescriptionRepository prescriptionRepository,IMedicineRepository medicineRepository,IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;

            this.medicineRepository = medicineRepository;

            this.prescriptionRepository = prescriptionRepository;

            this.applicationUserRepository = applicationUserRepository;
        }

        public async Task<IActionResult> Index()
        {

            try
            {
                ViewBag.Users = (await applicationUserRepository.GetAllUsers()).Count;

                ViewBag.Prescription = (await prescriptionRepository.GetAll()).Count;

                ViewBag.Patients = (await patientRepository.GetAll()).Count;

                ViewBag.Medicine = (await medicineRepository.GetAll()).Count;

                ViewBag.OutOfStock = (await medicineRepository.GetAllOutOfStockProducts()).Count();

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
    }
}
