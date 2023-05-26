using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.ApplicationUserModule;
using PharmCare.BLL.Repositories.MedecineModule;
using PharmCare.BLL.Repositories.PatientModule;
using PharmCare.BLL.Repositories.PrescriptionModule;
using PharmCare.BLL.Repositories.StockModule;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IPatientRepository patientRepository;

        private readonly IMedicineRepository medicineRepository;

        private readonly IPrescriptionRepository prescriptionRepository;

        private readonly IApplicationUserRepository applicationUserRepository;

        private readonly IStockRepository stockRepository;

        public DashboardController(IStockRepository stockRepository, IApplicationUserRepository applicationUserRepository, IPrescriptionRepository prescriptionRepository, IMedicineRepository medicineRepository, IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;

            this.medicineRepository = medicineRepository;

            this.prescriptionRepository = prescriptionRepository;

            this.applicationUserRepository = applicationUserRepository;

            this.stockRepository = stockRepository;
        }

        public async Task<IActionResult> Index()
        {

            try
            {
                var endDate = DateTime.Now.AddHours(23).AddMinutes(59).AddSeconds(59);

               var users = await applicationUserRepository.GetAllUsers();

                var prescription = await prescriptionRepository.GetAll();

                var patients = await patientRepository.GetAll();

                var medicine = await medicineRepository.GetAll();

                ViewBag.Prescription = prescription.Count;

                ViewBag.Medicine = medicine.Count;

                ViewBag.Patients = patients.Count();

                ViewBag.Users = users.Count;

                ViewBag.TodaysPrescription = prescription.Where(x => x.CreateDate >= DateTime.Today).ToList().Count();

                ViewBag.TodaysPatients = patients.Where(x => x.CreateDate >= DateTime.Today).ToList().Count();

                ViewBag.TodaysMedicine = medicine.Where(x => x.CreateDate >= DateTime.Today).ToList().Count();

                ViewBag.TodaysUsers = users.Where(x => x.CreateDate >= DateTime.Today).ToList().Count();

         

                ViewBag.OutOfStock = (await medicineRepository.GetAllOutOfStockProducts()).Count();

                ViewBag.ExpiredProducts = stockRepository.GetExpiredProducts().Where(x => x.Status != 2).Count();

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
