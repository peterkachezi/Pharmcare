using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using PharmCare.BLL.Repositories.MedecineModule;
using System.Drawing;
using PharmCare.BLL.Repositories.CategoryModule;
using PharmCare.DTO.ReportModule;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StockReportController : Controller
    {
        private readonly IMedicineRepository medicineRepository;

        private readonly ICategoryRepository categoryRepository;
        public StockReportController(ICategoryRepository categoryRepository, IMedicineRepository medicineRepository)
        {
            this.medicineRepository = medicineRepository;

            this.categoryRepository = categoryRepository;

        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await categoryRepository.GetAll();

            return View();
        }
        public async Task<IActionResult> DownloadReport(ReportDTO reportDTO)//advanced report
        {
            try
            {
                if (reportDTO.Status == 3)
                {
                    TempData["ErrorGeneralReport"] = "Please select report type";

                    return RedirectToAction("Index", new { area = "Admin" });
                }
                if (reportDTO.Status == 1)
                {
                    return await DownloadGeneralReport();
                }
                if (reportDTO.Status == 0)
                {
                    return await DownloadOutOfstock(reportDTO);
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
        private async Task<IActionResult> DownloadOutOfstock(ReportDTO reportDTO)
        {
            try
            {
                var stock = (await medicineRepository.GetAll()).Where(x => x.Quantity == 0);

                if (stock.Count() == 0)
                {
                    TempData["ErrorGeneralReport"] = "Report not available , try again later";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var sumCostPrice = stock.ToList().Select(c => c.ManufacturerPrice).Sum();

                var sumSellingPrice = stock.ToList().Select(c => c.SellingPrice).Sum();

                var sumQuantity = stock.ToList().Select(c => c.Quantity).Sum();

                var stream = new MemoryStream();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Users");

                    var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");

                    namedStyle.Style.Font.UnderLine = true;

                    namedStyle.Style.Font.Color.SetColor(Color.Blue);

                    const int startRow = 5;

                    var row = startRow;

                    //Create Headers and format them
                    worksheet.Cells["A1,B1,C1,D1,E1,F1,G1,H1"].Value = "Malela Pharmacy  Stock Report  - " + DateTime.Now + "";

                    using (var r = worksheet.Cells["A1:H1"])
                    {
                        r.Merge = true;

                        r.Style.Font.Color.SetColor(Color.White);

                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;

                        r.Style.Font.Bold = true;

                        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    }
                    worksheet.Cells["A2"].Value = "Medicine Name";

                    worksheet.Cells["B2"].Value = "Category";

                    worksheet.Cells["C2"].Value = "Treatment For";

                    worksheet.Cells["D2"].Value = "Shelf";

                    worksheet.Cells["E2"].Value = "CreateDate";

                    worksheet.Cells["F2"].Value = "Cost Price";

                    worksheet.Cells["G2"].Value = "SellingPrice";

                    worksheet.Cells["H2"].Value = "Quantity";

                    worksheet.Cells["A2:H2"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    worksheet.Cells["A2:H2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));

                    worksheet.Cells["A2:H2"].Style.Font.Bold = true;

                    row = 3;

                    foreach (var user in stock)
                    {
                        worksheet.Cells[row, 1].Value = user.MedicineFullName;

                        worksheet.Cells[row, 2].Value = user.CategoryName;

                        worksheet.Cells[row, 3].Value = user.MedicalConditionName;

                        worksheet.Cells[row, 4].Value = user.ShelfName;

                        worksheet.Cells[row, 5].Value = user.CreateDate.ToShortDateString();

                        worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 6].Value = user.ManufacturerPrice;

                        worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 7].Value = user.SellingPrice;

                        worksheet.Cells[row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 8].Value = user.Quantity;

                        row++;
                    }
                    // set some core property values

                    var getLastRow = stock.Count() + (3);

                    worksheet.Cells[getLastRow, 5].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 5].Value = "Total Amount";


                    worksheet.Cells[getLastRow, 6].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 6].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 6].Value = sumCostPrice;

                    worksheet.Cells[getLastRow, 7].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 7].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 7].Value = sumSellingPrice;

                    worksheet.Cells[getLastRow, 8].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 8].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 8].Value = sumQuantity;

                    xlPackage.Workbook.Properties.Title = "FP";

                    xlPackage.Workbook.Properties.Author = "FP";

                    xlPackage.Workbook.Properties.Subject = "FP";

                    // save the new spreadsheet
                    xlPackage.Save();
                    // Response.Clear();
                }
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockReport.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> DownloadGeneralReport()
        {
            try
            {
                var stock = (await medicineRepository.GetAll()).ToList();

                if (stock.Count() == 0)
                {
                    TempData["ErrorGeneralReport"] = "Report not available , try again later";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var sumCostPrice = stock.ToList().Select(c => c.ManufacturerPrice).Sum();

                var sumSellingPrice = stock.ToList().Select(c => c.SellingPrice).Sum();

                var sumQuantity = stock.ToList().Select(c => c.Quantity).Sum();

                var stream = new MemoryStream();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Users");

                    var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");

                    namedStyle.Style.Font.UnderLine = true;

                    namedStyle.Style.Font.Color.SetColor(Color.Blue);

                    const int startRow = 5;

                    var row = startRow;

                    //Create Headers and format them
                    worksheet.Cells["A1,B1,C1,D1,E1,F1,G1,H1"].Value = "Malela Pharmacy  Stock Report  - " + DateTime.Now + "";

                    using (var r = worksheet.Cells["A1:H1"])
                    {
                        r.Merge = true;

                        r.Style.Font.Color.SetColor(Color.White);

                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;

                        r.Style.Font.Bold = true;

                        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    }
                    worksheet.Cells["A2"].Value = "Medicine Name";

                    worksheet.Cells["B2"].Value = "Category";

                    worksheet.Cells["C2"].Value = "Treatment For";

                    worksheet.Cells["D2"].Value = "Shelf";

                    worksheet.Cells["E2"].Value = "CreateDate";

                    worksheet.Cells["F2"].Value = "Cost Price";

                    worksheet.Cells["G2"].Value = "SellingPrice";

                    worksheet.Cells["H2"].Value = "Quantity";



                    worksheet.Cells["A2:H2"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    worksheet.Cells["A2:H2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));

                    worksheet.Cells["A2:H2"].Style.Font.Bold = true;

                    row = 3;

                    foreach (var user in stock)
                    {
                        worksheet.Cells[row, 1].Value = user.MedicineFullName;

                        worksheet.Cells[row, 2].Value = user.CategoryName;

                        worksheet.Cells[row, 3].Value = user.MedicalConditionName;

                        worksheet.Cells[row, 4].Value = user.ShelfName;

                        worksheet.Cells[row, 5].Value = user.CreateDate.ToShortDateString();

                        worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 6].Value = user.ManufacturerPrice;

                        worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 7].Value = user.SellingPrice;


                        worksheet.Cells[row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 8].Value = user.Quantity;




                        row++;
                    }
                    // set some core property values

                    var getLastRow = stock.Count() + (3);

                    worksheet.Cells[getLastRow, 5].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 5].Value = "Total Amount";


                    worksheet.Cells[getLastRow, 6].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 6].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 6].Value = sumCostPrice;

                    worksheet.Cells[getLastRow, 7].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 7].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 7].Value = sumSellingPrice;

                    worksheet.Cells[getLastRow, 8].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 8].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 8].Value = sumQuantity;

                    xlPackage.Workbook.Properties.Title = "FP";

                    xlPackage.Workbook.Properties.Author = "FP";

                    xlPackage.Workbook.Properties.Subject = "FP";

                    // save the new spreadsheet
                    xlPackage.Save();
                    // Response.Clear();
                }
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockReport.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                TempData["Error"] = "Something went wrong";

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        public async Task<IActionResult> DownloadReportByCategory(ReportDTO reportDTO)
        {
            try
            {
                var stock = (await medicineRepository.GetAll()).Where(x => x.CategoryId == reportDTO.CategoryId);

                if (stock.Count() == 0)
                {
                    TempData["ErrorCategoryReport"] = "Report not available , try again later";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var sumCostPrice = stock.ToList().Select(c => c.ManufacturerPrice).Sum();

                var sumSellingPrice = stock.ToList().Select(c => c.SellingPrice).Sum();

                var sumQuantity = stock.ToList().Select(c => c.Quantity).Sum();

                var stream = new MemoryStream();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Users");

                    var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");

                    namedStyle.Style.Font.UnderLine = true;

                    namedStyle.Style.Font.Color.SetColor(Color.Blue);

                    const int startRow = 5;

                    var row = startRow;

                    //Create Headers and format them
                    worksheet.Cells["A1,B1,C1,D1,E1,F1,G1,H1"].Value = "Malela Pharmacy  Stock Report  - " + DateTime.Now + "";

                    using (var r = worksheet.Cells["A1:H1"])
                    {
                        r.Merge = true;

                        r.Style.Font.Color.SetColor(Color.White);

                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;

                        r.Style.Font.Bold = true;

                        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    }
                    worksheet.Cells["A2"].Value = "Medicine Name";

                    worksheet.Cells["B2"].Value = "Category";

                    worksheet.Cells["C2"].Value = "Treatment For";

                    worksheet.Cells["D2"].Value = "Shelf";

                    worksheet.Cells["E2"].Value = "CreateDate";

                    worksheet.Cells["F2"].Value = "Cost Price";

                    worksheet.Cells["G2"].Value = "SellingPrice";

                    worksheet.Cells["H2"].Value = "Quantity";



                    worksheet.Cells["A2:H2"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    worksheet.Cells["A2:H2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));

                    worksheet.Cells["A2:H2"].Style.Font.Bold = true;

                    row = 3;

                    foreach (var user in stock)
                    {
                        worksheet.Cells[row, 1].Value = user.MedicineFullName;

                        worksheet.Cells[row, 2].Value = user.CategoryName;

                        worksheet.Cells[row, 3].Value = user.MedicalConditionName;

                        worksheet.Cells[row, 4].Value = user.ShelfName;

                        worksheet.Cells[row, 5].Value = user.StockDate.ToShortDateString();

                        worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 6].Value = user.ManufacturerPrice;

                        worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 7].Value = user.SellingPrice;


                        worksheet.Cells[row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 8].Value = user.Quantity;




                        row++;
                    }
                    // set some core property values

                    var getLastRow = stock.Count() + (3);

                    worksheet.Cells[getLastRow, 5].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 5].Value = "Total Amount";


                    worksheet.Cells[getLastRow, 6].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 6].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 6].Value = sumCostPrice;

                    worksheet.Cells[getLastRow, 7].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 7].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 7].Value = sumSellingPrice;

                    worksheet.Cells[getLastRow, 8].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 8].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 8].Value = sumQuantity;

                    xlPackage.Workbook.Properties.Title = "FP";

                    xlPackage.Workbook.Properties.Author = "FP";

                    xlPackage.Workbook.Properties.Subject = "FP";

                    // save the new spreadsheet
                    xlPackage.Save();
                    // Response.Clear();
                }
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockReport.xlsx");
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
                if (reportDTO.DateFrom == default)
                {
                    TempData["ErrorDateReport"] = "Please select date from ";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                if (reportDTO.DateTo == default)
                {
                    TempData["ErrorDateReport"] = "Please select date to ";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var endDate = reportDTO.DateTo.AddHours(23).AddMinutes(59).AddSeconds(59);

                var stock = (await medicineRepository.GetAll()).Where(x => x.StockDate >= reportDTO.DateFrom && x.StockDate <= endDate).ToList();

                if (stock.Count() == 0)
                {
                    TempData["ErrorDateReport"] = "Report not available , try again later";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var sumCostPrice = stock.ToList().Select(c => c.ManufacturerPrice).Sum();

                var sumSellingPrice = stock.ToList().Select(c => c.SellingPrice).Sum();

                var sumQuantity = stock.ToList().Select(c => c.Quantity).Sum();

                var stream = new MemoryStream();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Users");

                    var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");

                    namedStyle.Style.Font.UnderLine = true;

                    namedStyle.Style.Font.Color.SetColor(Color.Blue);

                    const int startRow = 5;

                    var row = startRow;

                    //Create Headers and format them
                    worksheet.Cells["A1,B1,C1,D1,E1,F1,G1,H1"].Value = "Malela Pharmacy  Stock Report   - " + DateTime.Now + "";

                    using (var r = worksheet.Cells["A1:H1"])
                    {
                        r.Merge = true;

                        r.Style.Font.Color.SetColor(Color.White);

                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;

                        r.Style.Font.Bold = true;

                        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    }
                    worksheet.Cells["A2"].Value = "Medicine Name";

                    worksheet.Cells["B2"].Value = "Category";

                    worksheet.Cells["C2"].Value = "Treatment For";

                    worksheet.Cells["D2"].Value = "Shelf";

                    worksheet.Cells["E2"].Value = "CreateDate";

                    worksheet.Cells["F2"].Value = "Cost Price";

                    worksheet.Cells["G2"].Value = "SellingPrice";

                    worksheet.Cells["H2"].Value = "Quantity";


                    worksheet.Cells["A2:H2"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    worksheet.Cells["A2:H2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));

                    worksheet.Cells["A2:H2"].Style.Font.Bold = true;

                    row = 3;

                    foreach (var user in stock)
                    {
                        worksheet.Cells[row, 1].Value = user.MedicineFullName;

                        worksheet.Cells[row, 2].Value = user.CategoryName;

                        worksheet.Cells[row, 3].Value = user.MedicalConditionName;

                        worksheet.Cells[row, 4].Value = user.ShelfName;

                        worksheet.Cells[row, 5].Value = user.CreateDate.ToShortDateString();

                        worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 6].Value = user.ManufacturerPrice;

                        worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 7].Value = user.SellingPrice;


                        worksheet.Cells[row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 8].Value = user.Quantity;

                        row++;
                    }
                    // set some core property values

                    var getLastRow = stock.Count() + (3);

                    worksheet.Cells[getLastRow, 5].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 5].Value = "Total Amount";


                    worksheet.Cells[getLastRow, 6].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 6].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 6].Value = sumCostPrice;

                    worksheet.Cells[getLastRow, 7].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 7].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 7].Value = sumSellingPrice;

                    worksheet.Cells[getLastRow, 8].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 8].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 8].Value = sumQuantity;

                    xlPackage.Workbook.Properties.Title = "FP";

                    xlPackage.Workbook.Properties.Author = "FP";

                    xlPackage.Workbook.Properties.Subject = "FP";

                    // save the new spreadsheet
                    xlPackage.Save();
                    // Response.Clear();
                }
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockReport.xlsx");
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
