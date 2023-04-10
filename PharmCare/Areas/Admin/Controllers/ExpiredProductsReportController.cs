using AspNetCore.ReportingServices.ReportProcessing.OnDemandReportObjectModel;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using PharmCare.BLL.Repositories.StockModule;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using PharmCare.DAL.Models;
namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExpiredProductsReportController : Controller
    {
        private readonly IStockRepository stockRepository;

        private readonly UserManager<AppUser> userManager;

        public ExpiredProductsReportController(UserManager<AppUser> userManager,IStockRepository stockRepository)
        {
            this.stockRepository = stockRepository;

            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DownloadGeneralReport()
        {
            try
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);

                var userName= user.FirstName +" " + user.LastName;

                var sales = stockRepository.GetExpiredProducts().ToList();

                if (sales.Count == 0)
                {
                    TempData["ErrorGeneralReport"] = "There are no sales report";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var sumTotal = sales.ToList().Select(c => c.Total).Sum();

                var sumManufacturerPrice = sales.ToList().Select(c => c.ManufacturerPrice).Sum();

                var sumSellingPrice = sales.ToList().Select(c => c.SellingPrice).Sum();

                var sumQuantity = sales.ToList().Select(c => c.Quantity).Sum();

                var stream = new MemoryStream();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("ExpiredProducts");

                    var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");

                    namedStyle.Style.Font.UnderLine = true;

                    namedStyle.Style.Font.Color.SetColor(Color.Blue);

                    const int startRow = 5;

                    var row = startRow;

                    //Create Headers and format them

                    worksheet.Cells["A1,B1,C1,D1,E1,F1,G1,H1,I1"].Value = "MUMIAS WEST HEALTH CARE SALES REPORT  - " + DateTime.Now + "";

                    using (var r = worksheet.Cells["A1:I1"])
                    {
                        r.Merge = true;

                        r.Style.Font.Color.SetColor(Color.White);

                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;

                        r.Style.Font.Bold = true;

                        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    }

                    worksheet.Cells["A2"].Value = "Medicine";

                    worksheet.Cells["B2"].Value = "Category";

                    worksheet.Cells["C2"].Value = "ShelfName";

                    worksheet.Cells["D2"].Value = "Date Of Manufacture";

                    worksheet.Cells["E2"].Value = "ExpiryDate";

                    worksheet.Cells["F2"].Value = "Supplier";

                    worksheet.Cells["G2"].Value = "Quantity";  
                    
                    worksheet.Cells["H2"].Value = "Cost Price";   
                    
                    worksheet.Cells["I2"].Value = "Selling Price";          


                    worksheet.Cells["A2:I2"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    worksheet.Cells["A2:I2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));

                    worksheet.Cells["A2:I2"].Style.Font.Bold = true;


                    row = 3;

                    foreach (var salesData in sales)
                    {
                        worksheet.Cells[row, 1].Value = salesData.MedicineName;

                        worksheet.Cells[row, 2].Value = salesData.CategoryName;

                        worksheet.Cells[row, 3].Value = salesData.ShelfName;

                        worksheet.Cells[row, 4].Value = salesData.DateOfManufacture.ToShortDateString();

                        worksheet.Cells[row, 5].Value = salesData.ExpiryDate.ToShortDateString();

                        worksheet.Cells[row, 6].Value = salesData.NameOfSupplier.ToString();

                        worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 7].Value = salesData.Quantity;

                        worksheet.Cells[row, 8].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 8].Value = salesData.ManufacturerPrice;    
                        
                        
                        worksheet.Cells[row, 9].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[row, 9].Value = salesData.SellingPrice;


                        //worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                        //worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        //worksheet.Cells[row, 6].Value = salesData.Total;


                        row++;
                    }
                    // set some core property values

                    var getLastRow = sales.Count() + (3);

                    worksheet.Cells[getLastRow, 6].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 6].Value = "Total Amount";

                    worksheet.Cells[getLastRow, 7].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 7].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 7].Value = sumQuantity;

                    worksheet.Cells[getLastRow, 8].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 8].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 8].Value = sumManufacturerPrice;

                    worksheet.Cells[getLastRow, 9].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[getLastRow, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[getLastRow, 9].Style.Font.Bold = true;
                    worksheet.Cells[getLastRow, 9].Value = sumSellingPrice;


                    var preparedBy = sales.Count() + (4);

                    worksheet.Cells[preparedBy, 6].Style.Font.Bold = true;
                    worksheet.Cells[preparedBy, 6].Value = "Prepared by : " + userName.ToString();



                    xlPackage.Workbook.Properties.Title = "FP";

                    xlPackage.Workbook.Properties.Author = "FP";

                    xlPackage.Workbook.Properties.Subject = "FP";

                    // save the new spreadsheet
                    xlPackage.Save();
                    // Response.Clear();
                }
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExpiriesReport.xlsx");
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
