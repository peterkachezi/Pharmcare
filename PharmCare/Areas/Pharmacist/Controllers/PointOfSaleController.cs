using AspNetCore.Reporting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.MedecineModule;
using PharmCare.BLL.Repositories.SalesModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.SalesModule;
using System.Data;

namespace PharmCare.Areas.Pharmacist.Controllers
{
    [Area("Pharmacist")]
    public class PointOfSaleController : Controller
    {
        private readonly IMedicineRepository medicineRepository;

        private readonly ISalesRepository salesRepository;

        private readonly UserManager<AppUser> userManager;

        private readonly IWebHostEnvironment env;
        public PointOfSaleController(IWebHostEnvironment env, ISalesRepository salesRepository, UserManager<AppUser> userManager, IMedicineRepository medicineRepository)
        {
            this.userManager = userManager;

            this.medicineRepository = medicineRepository;

            this.salesRepository = salesRepository;

            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Medicines = await medicineRepository.GetAll();

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveTransaction(SalesDTO salesDTO)
        {
            try
            {

                if (salesDTO.AmountPaid.Equals(null))
                {
                    return Json(new { success = false, responseText = "Please enter the amount paid" });
                }

                if (salesDTO.AmountPaid < salesDTO.TotalAmount)
                {
                    return Json(new { success = false, responseText = "You have entered insufficient funds " });

                }

                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                salesDTO.CreatedBy = user.Id;

                var result = salesRepository.SaveSales(salesDTO);

                if (result != null)
                {


                    return Json(new { success = true, responseText = salesDTO.ReceiptNo });
                }

                else
                {
                    return Json(new { success = false, responseText = "Transaction was not successfull" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<ActionResult> GetReceipt(string ReceiptNo)
        {
            try
            {
                if (ReceiptNo != null)
                {
                    var dt = new DataTable();

                    dt = await GetTransaction(ReceiptNo);

                    var getSale = await salesRepository.GetSaleByReceiptNo(ReceiptNo);

                    string mimetype = "";

                    int extension = 1;

                    var path = $"{env.WebRootPath}\\Report\\PrintReceipt.rdlc";

                    Dictionary<string, string> parameters = new Dictionary<string, string>();

                    parameters.Add("TotalAmount", getSale.TotalAmount.ToString());

                    parameters.Add("AmountPaid", getSale.AmountPaid.ToString());

                    parameters.Add("Balance", getSale.Balance.ToString());

                    parameters.Add("CreatedByName", getSale.CreatedByName.ToString());

                    LocalReport localReport = new LocalReport(path);

                    localReport.AddDataSource("Receipt", dt);

                    var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

                    return File(result.MainStream, "application/pdf");
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
        public async Task<DataTable> GetTransaction(string ReceiptNo)
        {
            var list = await salesRepository.GetSalesDetailsByReceiptNo(ReceiptNo);

            if (list != null)
            {

                var dt = new DataTable();

                dt.Columns.Add("Id");

                dt.Columns.Add("Quantity");

                dt.Columns.Add("SellingPrice");

                dt.Columns.Add("Total");

                dt.Columns.Add("CreateDate");

                dt.Columns.Add("MedicineName");

                dt.Columns.Add("Balance");

                dt.Columns.Add("AmountPaid");

                dt.Columns.Add("ReceiptNo");


                DataRow row;

                foreach (var item in list)
                {
                    row = dt.NewRow();

                    row["Id"] = item.Id.ToString().ToUpper();

                    row["Quantity"] = item.Quantity.ToString();

                    row["SellingPrice"] = item.SellingPrice.ToString();

                    row["Total"] = item.Total.ToString();

                    row["MedicineName"] = item.MedicineName.ToString();

                    row["Balance"] = item.Balance.ToString();

                    row["AmountPaid"] = item.AmountPaid.ToString();

                    row["CreateDate"] = item.NewCreateDate.ToString();

                    row["ReceiptNo"] = item.ReceiptNo.ToString();

                    dt.Rows.Add(row);
                }

                return dt;
            }

            return null;

        }
    }
}
