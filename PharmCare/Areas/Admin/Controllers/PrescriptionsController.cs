using AspNetCore.Reporting;
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
using System.Data;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PrescriptionsController : Controller
    {
        private readonly IMedicineRepository medicineRepository;

        private readonly IPrescriptionRepository prescriptionRepository;

        private readonly UserManager<AppUser> userManager;

        private readonly IPatientRepository patientRepository;

        private readonly IWebHostEnvironment env;
        public PrescriptionsController(IWebHostEnvironment env, IMedicineRepository medicineRepository, IPatientRepository patientRepository, IPrescriptionRepository prescriptionRepository, UserManager<AppUser> userManager)
        {
            this.prescriptionRepository = prescriptionRepository;

            this.userManager = userManager;

            this.patientRepository = patientRepository;

            this.medicineRepository = medicineRepository;

            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var data = (await prescriptionRepository.GetAll()).OrderByDescending(x => x.CreateDate).ToList();

                return View(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }


        public async Task<IActionResult> AddPrescription()
        {
            try
            {
                ViewBag.Medicines = await medicineRepository.GetAll();

                var data = await patientRepository.GetAll();

                return View(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> Details(Guid? Id, Guid PatientId)
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

                    ViewBag.PatientId = PatientId;

                    ViewBag.PrescriptionId = Id;


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

                if (results != null)
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
                var medicine = await medicineRepository.GetStockDetailsById(Id);

                if (medicine != null)
                {

                    return Json(new { data = medicine });
                }

                return Json(new { data = false });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Json(new { success = false, responseText = "Something went wrong" });
            }
        }
        public async Task<IActionResult> DowloadPrescripton(Guid? Id, Guid? PatientId)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                var createdBy = user.FirstName + " " + user.LastName;

                var dt = new DataTable();

                dt = GetPrescription(Id.Value);

                var getPatient = await patientRepository.GetById(PatientId.Value);

                string mimetype = "";

                int extension = 1;

                var path = $"{env.WebRootPath}\\Report\\Prescription.rdlc";

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("FullName", getPatient.FullName.ToString());

                parameters.Add("PhoneNumber", getPatient.PhoneNumber.ToString());

                parameters.Add("Gender", getPatient.Gender.ToString());

                parameters.Add("Age", getPatient.Age.ToString());

                parameters.Add("CreatedByName", createdBy.ToString());

                LocalReport localReport = new LocalReport(path);

                localReport.AddDataSource("PrintPrescription", dt);

                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

                // return File(result.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet, "Receipt.pdf");

                return File(result.MainStream, "application/pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public DataTable GetPrescription(Guid Id)
        {
            try
            {
                var list = prescriptionRepository.GetAllPrescriptionDetailsById(Id);

                if (list != null)
                {

                    var dt = new DataTable();

                    dt.Columns.Add("Id");

                    dt.Columns.Add("MedicineName");

                    dt.Columns.Add("Frequency");

                    dt.Columns.Add("WhenToTake");

                    dt.Columns.Add("NoOfDays");

                    dt.Columns.Add("CreateDate");

                    dt.Columns.Add("BillNo");

                    dt.Columns.Add("PaymentStatusDescription");

                    DataRow row;

                    foreach (var item in list)
                    {
                        row = dt.NewRow();

                        row["Id"] = item.Id.ToString().ToUpper();

                        row["MedicineName"] = item.MedicineName.ToString();

                        row["Frequency"] = item.Frequency.ToString();

                        row["WhenToTake"] = item.WhenToTake.ToString();

                        row["NoOfDays"] = item.NoOfDays.ToString();

                        row["CreateDate"] = item.CreateDate.ToShortDateString();

                        row["BillNo"] = item.BillNo.ToString();

                        row["PaymentStatusDescription"] = item.PaymentStatusDescription.ToString();

                        dt.Rows.Add(row);
                    }

                    return dt;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                var results = await prescriptionRepository.Delete(Id);

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

        public async Task<ActionResult> DeleteDetails(Guid Id)
        {
            try
            {
                var results = await prescriptionRepository.Delete(Id);

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

    }
}
