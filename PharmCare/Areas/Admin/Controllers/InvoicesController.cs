using AspNetCore.Reporting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.PatientModule;
using PharmCare.BLL.Repositories.PrescriptionModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.InvoiceModule;
using PharmCare.DTO.PrescriptionModule;
using System.Data;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InvoicesController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        private readonly IPrescriptionRepository prescriptionRepository;

        private readonly IPatientRepository patientRepository;

        private readonly IWebHostEnvironment env;
        public InvoicesController(IWebHostEnvironment env,

            UserManager<AppUser> userManager,

            IPatientRepository patientRepository,

            IPrescriptionRepository prescriptionRepository)
        {
            this.prescriptionRepository = prescriptionRepository;

            this.patientRepository = patientRepository;

            this.userManager = userManager;

            this.env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GenerateInvoice(Guid? Id, Guid PatientId)
        {
            try
            {
                if (Id != null)
                {

                    PrescriptionProfileDTO prescriptionProfileDTO = new PrescriptionProfileDTO()
                    {

                    };
                    var prescriptionDetails = prescriptionRepository.GetAllPrescriptionDetailsById(Id.Value);

                    decimal sum = prescriptionDetails.Select(t => t.Total).Sum();

                    ViewBag.TotalAmount = sum;

                    prescriptionProfileDTO.prescriptionDetails = prescriptionDetails;

                    prescriptionProfileDTO.patientDTO = await patientRepository.GetById(PatientId);

                    prescriptionProfileDTO.prescription = await prescriptionRepository.GetPrescriptionById(Id.Value);

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
        private DataTable GetPrescriptionDetails(Guid Id)
        {
            try
            {
                var list = prescriptionRepository.GetAllPrescriptionDetailsById(Id);

                if (list != null)
                {
                    var dt = new DataTable();

                    dt.Columns.Add("Id");

                    dt.Columns.Add("MedicineName");

                    dt.Columns.Add("Quantity");

                    dt.Columns.Add("SellingPrice");

                    dt.Columns.Add("Total");

                    dt.Columns.Add("BillNo");

                    DataRow row;

                    foreach (var item in list)
                    {
                        row = dt.NewRow();

                        row["Id"] = item.Id.ToString().ToUpper();

                        row["MedicineName"] = item.MedicineName.ToString();

                        row["Quantity"] = item.Quantity.ToString();

                        row["SellingPrice"] = item.SellingPrice.ToString();

                        row["Total"] = item.Total.ToString();

                        row["BillNo"] = item.BillNo.ToString();

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
        public async Task<IActionResult> DowloadInvoice(Guid? Id, Guid? PatientId)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                var createdBy = user.FirstName + " " + user.LastName;

                var dt = new DataTable();

                dt = GetPrescriptionDetails(Id.Value);

                var getPatient = await patientRepository.GetById(PatientId.Value);

                var prescription = await prescriptionRepository.GetPrescriptionById(Id.Value);

                var list = prescriptionRepository.GetAllPrescriptionDetailsById(Id.Value);

                decimal sum = list.Select(t => t.Total).Sum();

                string mimetype = "";

                int extension = 1;

                var path = $"{env.WebRootPath}\\Report\\PrescriptionInvoice.rdlc";

                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "FullName", getPatient.FullName.ToString() },

                    { "PhoneNumber", getPatient.PhoneNumber.ToString() },

                    { "CreatedByName", createdBy.ToString() },

                    { "TotalAmount",sum.ToString() },

                    { "CreateDate", prescription.CreateDate.ToShortDateString() },

                    { "BillNo", prescription.BillNo.ToString() },

                    { "AmountPaid",("0.00").ToString() },

                    { "SerialNo", prescription.Id.ToString().ToUpper() }
                };

                LocalReport localReport = new LocalReport(path);

                localReport.AddDataSource("GeneratePrescriptionInvoice", dt);

                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

                return File(result.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet, "Invoice.pdf");

                //return File(result.MainStream, "application/pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<JsonResult> CreatePayment(InvoicePaymentDTO invoicePaymentDTO)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                invoicePaymentDTO.CreatedBy = user.Id;

                var result = await prescriptionRepository.CreatePayment(invoicePaymentDTO);

                if (result != null)
                {
                    return Json(new { success = true, responseText = "Payment was successful" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Failed to create payment" });
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

