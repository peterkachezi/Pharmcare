using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using PharmCare.BLL.Repositories.MedecineModule;
using PharmCare.BLL.Repositories.ProductTypeModule;
using PharmCare.BLL.Repositories.SupplierModule;
using PharmCare.DTO.ReportModule;
using PharmCare.BLL.Repositories.PatientModule;
using System.Drawing;
using PharmCare.DAL.Models;

namespace PharmCare.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SuppliersReportController : Controller
	{

        private readonly ISupplierRepository supplierRepository;

        private readonly IProductTypeRepository productTypeRepository;
		public SuppliersReportController(IProductTypeRepository productTypeRepository,ISupplierRepository supplierRepository)
		{

			this.supplierRepository = supplierRepository;

			this.productTypeRepository = productTypeRepository;
		}
        public async Task<IActionResult> Index()
		{

            ViewBag.ProductTypes = await productTypeRepository.GetAll();

            return View();
		}
        public async Task<IActionResult> DownloadReportByDateRange(DateTime DateFrom, DateTime DateTo)
        {
            try
            {

                if (DateFrom == default)
                {
                    TempData["ErrorDateReport"] = "Please select date from";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                if (DateTo == default)
                {
                    TempData["ErrorDateReport"] = "Please select date to";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var data = await supplierRepository.GetAll();

                var endDate = DateTo.AddHours(23).AddMinutes(59).AddSeconds(59);

                var suppliers = data.Where(x => x.CreateDate >= DateFrom && x.CreateDate <= endDate).ToList();

                if (suppliers.Count == 0)
                {
                    TempData["ErrorDateReport"] = "There are no  report";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var stream = new MemoryStream();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Suppliers");

                    var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");

                    namedStyle.Style.Font.UnderLine = true;

                    namedStyle.Style.Font.Color.SetColor(Color.Blue);

                    const int startRow = 5;

                    var row = startRow;

                    //Create Headers and format them
                    worksheet.Cells["A1,B1,C1,D1,E1,F1"].Value = "MALELA PHARMACY - SUPPLIERS REPORT";

                    using (var r = worksheet.Cells["A1:F1"])
                    {
                        r.Merge = true;

                        r.Style.Font.Color.SetColor(Color.White);

                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;

                        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    }

                    worksheet.Cells["A2"].Value = "SupplierNo";

                    worksheet.Cells["B2"].Value = "Name";

                    worksheet.Cells["C2"].Value = "PhoneNumber";

                    worksheet.Cells["D2"].Value = "Email";

                    worksheet.Cells["E2"].Value = "ProductTypeName";

                    worksheet.Cells["F2"].Value = "EntryDate";

                    worksheet.Cells["A2:F2"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    worksheet.Cells["A2:F2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));

                    worksheet.Cells["A2:F2"].Style.Font.Bold = true;

                    row = 3;

                    foreach (var user in suppliers)
                    {

                        worksheet.Cells[row, 1].Value = user.SupplierNo;

                        worksheet.Cells[row, 2].Value = user.Name;

                        worksheet.Cells[row, 3].Value = user.PhoneNumber;

                        worksheet.Cells[row, 4].Value = user.Email;

                        worksheet.Cells[row, 5].Value = user.ProductTypeName;

                        worksheet.Cells[row, 6].Value = user.CreateDate.ToShortDateString();     

                        row++;
                    }

                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "MAL";

                    xlPackage.Workbook.Properties.Author = "MAL";

                    xlPackage.Workbook.Properties.Subject = "MAL";
                    // save the new spreadsheet
                    xlPackage.Save();
                    // Response.Clear();
                }
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Supplier Report From " + DateFrom.ToShortDateString() + " To " + DateTo.ToShortDateString() + " .xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong ,please contact system admin for assistance";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        public async Task<IActionResult> DownloadReportByProductType(Guid ProductTypeId)
        {
            try
            {        
                var suppliers = (await supplierRepository.GetAll()).Where(x=>x.ProductTypeId == ProductTypeId).ToList();
         
                if (suppliers.Count == 0)
                {
                    TempData["ErrorProductType"] = "There are no  report";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var stream = new MemoryStream();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Suppliers");

                    var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");

                    namedStyle.Style.Font.UnderLine = true;

                    namedStyle.Style.Font.Color.SetColor(Color.Blue);

                    const int startRow = 5;

                    var row = startRow;

                    //Create Headers and format them
                    worksheet.Cells["A1,B1,C1,D1,E1,F1"].Value = "MALELA PHARMACY - SUPPLIERS REPORT";

                    using (var r = worksheet.Cells["A1:F1"])
                    {
                        r.Merge = true;

                        r.Style.Font.Color.SetColor(Color.White);

                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;

                        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    }

                    worksheet.Cells["A2"].Value = "SupplierNo";

                    worksheet.Cells["B2"].Value = "Name";

                    worksheet.Cells["C2"].Value = "PhoneNumber";

                    worksheet.Cells["D2"].Value = "Email";

                    worksheet.Cells["E2"].Value = "ProductTypeName";

                    worksheet.Cells["F2"].Value = "EntryDate";

                    worksheet.Cells["A2:F2"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    worksheet.Cells["A2:F2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));

                    worksheet.Cells["A2:F2"].Style.Font.Bold = true;

                    row = 3;

                    foreach (var user in suppliers)
                    {

                        worksheet.Cells[row, 1].Value = user.SupplierNo;

                        worksheet.Cells[row, 2].Value = user.Name;

                        worksheet.Cells[row, 3].Value = user.PhoneNumber;

                        worksheet.Cells[row, 4].Value = user.Email;

                        worksheet.Cells[row, 5].Value = user.ProductTypeName;

                        worksheet.Cells[row, 6].Value = user.CreateDate.ToShortDateString();

                        row++;
                    }

                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "MAL";

                    xlPackage.Workbook.Properties.Author = "MAL";

                    xlPackage.Workbook.Properties.Subject = "MAL";
                    // save the new spreadsheet
                    xlPackage.Save();
                    // Response.Clear();
                }
                stream.Position = 0;

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "YourReportName.xlsx");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong ,please contact system admin for assistance";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
    }
}
