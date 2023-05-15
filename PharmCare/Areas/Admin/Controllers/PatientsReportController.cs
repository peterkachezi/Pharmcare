using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PharmCare.BLL.Repositories.PatientModule;
using PharmCare.DTO.ReportModule;
using System.Drawing;

namespace PharmCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PatientsReportController : Controller
    {
        private readonly IPatientRepository patientRepository;

        public PatientsReportController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }
        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> DownloadReportByDateRange(DateTime DateFrom, DateTime DateTo)
        {
            try
            {

                if (DateFrom == default)
                {
                    TempData["ErrorDateReport"] = "Please select date from";

                    return RedirectToAction("Index", new { area = "Claims" });
                }

                if (DateTo == default)
                {
                    TempData["ErrorDateReport"] = "Please select date to";

                    return RedirectToAction("Index", new { area = "Claims" });
                }

                var data = await patientRepository.GetAll();

                var endDate = DateTo.AddHours(23).AddMinutes(59).AddSeconds(59);

                var patients = data.Where(x => x.CreateDate >= DateFrom && x.CreateDate <= endDate).ToList();

                if (patients.Count == 0)
                {
                    TempData["ErrorDateReport"] = "There are no  report";

                    return RedirectToAction("Index", new { area = "Admin" });
                }

                var stream = new MemoryStream();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Patients");

                    var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");

                    namedStyle.Style.Font.UnderLine = true;

                    namedStyle.Style.Font.Color.SetColor(Color.Blue);

                    const int startRow = 5;

                    var row = startRow;

                    //Create Headers and format them
                    worksheet.Cells["A1,B1,C1,D1,E1,F1,G1"].Value = "MALELA PHARMACY - PATIENTS REPORT";

                    using (var r = worksheet.Cells["A1:G1"])
                    {
                        r.Merge = true;

                        r.Style.Font.Color.SetColor(Color.White);

                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;

                        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    }

                    worksheet.Cells["A2"].Value = "PatientNumber";

                    worksheet.Cells["B2"].Value = "FullName";

                    worksheet.Cells["C2"].Value = "Gender";

                    worksheet.Cells["D2"].Value = "PhoneNumber";

                    worksheet.Cells["E2"].Value = "DateOfBirth";

                    worksheet.Cells["F2"].Value = "Age";

                    worksheet.Cells["G2"].Value = "EntryDate";

                    worksheet.Cells["A2:G2"].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    worksheet.Cells["A2:G2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));

                    worksheet.Cells["A2:G2"].Style.Font.Bold = true;

                    row = 3;

                    foreach (var user in patients)
                    {

                        worksheet.Cells[row, 1].Value = user.PatientNumber;

                        worksheet.Cells[row, 2].Value = user.FullName;

                        worksheet.Cells[row, 3].Value = user.Gender;

                        worksheet.Cells[row, 4].Value = user.PhoneNumber;

                        worksheet.Cells[row, 5].Value = user.DateOfBirth.ToShortDateString();

                        worksheet.Cells[row, 6].Value = user.Age;

                        worksheet.Cells[row, 7].Value = user.CreateDate.ToShortDateString();

                        row++;
                    }

                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "MAKL";

                    xlPackage.Workbook.Properties.Author = "MAKL";

                    xlPackage.Workbook.Properties.Subject = "MAKL";
                    // save the new spreadsheet
                    xlPackage.Save();
                    // Response.Clear();
                }
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Patients Report From " + DateFrom.ToShortDateString() + " To " + DateTo.ToShortDateString() + " .xlsx");
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
