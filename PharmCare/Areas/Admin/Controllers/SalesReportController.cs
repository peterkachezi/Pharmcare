using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PharmCare.BLL.Repositories.SalesModule;
using PharmCare.DAL.Models;
using PharmCare.DTO.ReportModule;
using System.Drawing;
using static QuestPDF.Helpers.Colors;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalesReportController : Controller
    {
        private readonly ISalesRepository salesRepository;
        public SalesReportController(ISalesRepository salesRepository)
        {
            this.salesRepository = salesRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DownloadReport(ReportDTO reportDTO)//advanced report
        {
            try
            {
                if (reportDTO.DateFrom == DateTime.MinValue & reportDTO.DateTo == DateTime.MinValue)
                {
                    return await DownloadGeneralReport();
                }
                if (reportDTO.DateFrom != null & reportDTO.DateTo != null)
                {
                    return await DownloadReportByDateRange(reportDTO);
                }

                else
                {
                    return RedirectToAction("Index", new { area = "Admin" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<IActionResult> DownloadGeneralReport()
        {
            try
            {

                var sales = (await salesRepository.GetAllSalesDetails()).ToList();

                if (sales.Count == 0)
                {
                    TempData["ErrorGeneralReport"] = "There are no sales report";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var sumTotal = sales.ToList().Select(c => c.Total).Sum();

                var sumSellingPrice = sales.ToList().Select(c => c.SellingPrice).Sum();

                var sumQuantity = sales.ToList().Select(c => c.Quantity).Sum();

                var stream = new MemoryStream();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Sales");

                    var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");

                    namedStyle.Style.Font.UnderLine = true;

                    namedStyle.Style.Font.Color.SetColor(Color.Blue);

                    const int startRow = 5;

                    var row = startRow;

                    //Create Headers and format them

                    worksheet.Cells["A1,B1,C1,D1,E1,F1"].Value = "MALELA PHARMACY SALES REPORT  - " + DateTime.Now + "";

                    using (var r = worksheet.Cells["A1:f1"])
                    {
                        r.Merge = true;

                        r.Style.Font.Color.SetColor(Color.White);

                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;

                        r.Style.Font.Bold = true;

                        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));

                    }

                    worksheet.Cells["A2"].Value = "Transaction No";

                    worksheet.Cells["B2"].Value = "Medicine Name";

                    worksheet.Cells["C2"].Value = "Transaction Date";

                    worksheet.Cells["D2"].Value = "Quantity Sold";

                    worksheet.Cells["E2"].Value = "Selling Price";

                    worksheet.Cells["F2"].Value = "Total";


                    worksheet.Cells["A2:F2"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    worksheet.Cells["A2:F2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));

                    worksheet.Cells["A2:F2"].Style.Font.Bold = true;


                    row = 3;

                    foreach (var salesData in sales)
                    {
                        worksheet.Cells[row, 1].Value = salesData.ReceiptNo;

                        worksheet.Cells[row, 2].Value = salesData.MedicineFullName;

                        worksheet.Cells[row, 3].Value = salesData.CreateDate.ToShortDateString();


                        worksheet.Cells[row, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 4].Value = salesData.Quantity;

                        worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 5].Value = salesData.SellingPrice;


                        worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 6].Value = salesData.Total;


                        row++;
                    }
                    // set some core property values

                    var getLastRow = sales.Count() + (3);

                    worksheet.Cells[getLastRow, 3].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 3].Value = "Total Amount";

                    worksheet.Cells[getLastRow, 4].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 4].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 4].Value = sumQuantity;

                    worksheet.Cells[getLastRow, 5].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 5].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 5].Value = sumSellingPrice;

                    worksheet.Cells[getLastRow, 6].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 6].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 6].Value = sumTotal;


                    xlPackage.Workbook.Properties.Title = "Pharm";

                    xlPackage.Workbook.Properties.Author = "Pharm";

                    xlPackage.Workbook.Properties.Subject = "Pharm";

                    // save the new spreadsheet
                    xlPackage.Save();
                    // Response.Clear();
                }
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesReport.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> DownloadReportByDateRange(ReportDTO reportDTO)
        {

            try
            {
                if (reportDTO.DateFrom == null)
                {
                    TempData["ErrorDateReport"] = "Please select date from ";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                if (reportDTO.DateTo == null)
                {
                    TempData["ErrorDate"] = "Please select date to ";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var endDate = reportDTO.DateTo.AddHours(23).AddMinutes(59).AddSeconds(59);

                var sales = (await salesRepository.GetAllSalesDetails()).Where(x => x.CreateDate >= reportDTO.DateFrom && x.CreateDate <= endDate).ToList();

                if (sales.Count == 0)
                {
                    TempData["ErrorDate"] = "There are no sales report on the selected date";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var sumTotal = sales.ToList().Select(c => c.Total).Sum();

                var sumSellingPrice = sales.ToList().Select(c => c.SellingPrice).Sum();

                var sumQuantity = sales.ToList().Select(c => c.Quantity).Sum();

                var stream = new MemoryStream();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Sales");

                    var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");

                    namedStyle.Style.Font.UnderLine = true;

                    namedStyle.Style.Font.Color.SetColor(Color.Blue);

                    const int startRow = 5;

                    var row = startRow;

                    //Create Headers and format them
                    worksheet.Cells["A1,B1,C1,D1,E1,F1"].Value = "MALELA PHARMACY SALES REPORT  - " + DateTime.Now + "";

                    using (var r = worksheet.Cells["A1:f1"])
                    {
                        r.Merge = true;

                        r.Style.Font.Color.SetColor(Color.White);

                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;

                        r.Style.Font.Bold = true;

                        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));


                    }

                    worksheet.Cells["A2"].Value = "Transaction No";

                    worksheet.Cells["B2"].Value = "Medicine Name";

                    worksheet.Cells["C2"].Value = "Transaction Date";

                    worksheet.Cells["D2"].Value = "Quantity Sold";

                    worksheet.Cells["E2"].Value = "Selling Price";

                    worksheet.Cells["F2"].Value = "Total";


                    worksheet.Cells["A2:F2"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    worksheet.Cells["A2:F2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));

                    worksheet.Cells["A2:F2"].Style.Font.Bold = true;


                    row = 3;

                    foreach (var salesData in sales)
                    {
                        worksheet.Cells[row, 1].Value = salesData.ReceiptNo;

                        worksheet.Cells[row, 2].Value = salesData.MedicineFullName;

                        worksheet.Cells[row, 3].Value = salesData.CreateDate.ToShortDateString();

                        worksheet.Cells[row, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 4].Value = salesData.Quantity;

                        worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 5].Value = salesData.SellingPrice;

                        worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 6].Value = salesData.Total;

                        row++;
                    }
                    // set some core property values

                    var getLastRow = sales.Count() + (3);

                    worksheet.Cells[getLastRow, 3].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 3].Value = "Total Amount";

                    worksheet.Cells[getLastRow, 4].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 4].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 4].Value = sumQuantity;

                    worksheet.Cells[getLastRow, 5].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 5].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 5].Value = sumSellingPrice;

                    worksheet.Cells[getLastRow, 6].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 6].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 6].Value = sumTotal;


                    xlPackage.Workbook.Properties.Title = "Pharm";

                    xlPackage.Workbook.Properties.Author = "Pharm";

                    xlPackage.Workbook.Properties.Subject = "Pharm";

                    // save the new spreadsheet
                    xlPackage.Save();
                    // Response.Clear();
                }

                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesReport.xlsx");
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
