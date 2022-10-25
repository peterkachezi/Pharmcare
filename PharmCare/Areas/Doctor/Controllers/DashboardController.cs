using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.MedecineModule;
using PharmCare.BLL.Repositories.PatientModule;

namespace PharmCare.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    public class DashboardController : Controller
    {
        private readonly IPatientRepository patientRepository;

        private readonly IMedicineRepository medicineRepository;

        public DashboardController(IMedicineRepository medicineRepository,IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;

            this.medicineRepository = medicineRepository;
        }

        public async Task<IActionResult> Index()
        {

            try
            {
                ViewBag.Patients = (await patientRepository.GetAll()).Count;

                ViewBag.Medicine = (await medicineRepository.GetAll()).Count;

                ViewBag.OutOfStock = (await medicineRepository.GetAll()).Where(x=>x.Quantity==0).Count();

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
