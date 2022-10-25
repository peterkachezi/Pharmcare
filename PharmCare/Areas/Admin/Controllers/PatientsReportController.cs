

using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using PharmCare.BLL.Repositories.PatientModule;

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

        public async Task<IActionResult> PrintPDF()
        {
            MemoryStream ms = new MemoryStream();

            PdfWriter writer = new PdfWriter(ms);

            PdfDocument pdfDoc = new PdfDocument(writer);

            Document document = new Document(pdfDoc, PageSize.A4, true);

            writer.SetCloseStream(false);

            Paragraph header = new Paragraph("Patient Report")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20);
            document.Add(header);

            Paragraph subheader = new Paragraph(DateTime.Now.ToShortDateString())
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFontSize(15);
            document.Add(subheader);

            //empty line
            document.Add(new Paragraph(""));

            //line separtor
            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);

            //empty line
            document.Add(new Paragraph(""));

            //add table contaning data
            document.Add(await GetPdfTable());

            //page Numbers

            int n = pdfDoc.GetNumberOfPages();
            for (int i = 1; i <= n; i++)
            {
                document.ShowTextAligned(new Paragraph(String
                  .Format("Page " + i + " of " + n)),
                  559, 806, i, TextAlignment.RIGHT,
                  VerticalAlignment.TOP, 0);
            }

            document.Close();

            byte[] byteInfo = ms.ToArray();
            ms.Write(byteInfo, 0, byteInfo.Length);
            ms.Position = 0;

            FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");

            fileStreamResult.FileDownloadName = "PatientsReport.pdf";

            return fileStreamResult;
        }

        private async Task<Table> GetPdfTable()
        {

            Table table = new Table(4, false);

            //Headings

            Cell cellFirstName = new Cell(1, 1)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("First Name"));

            Cell cellLastName = new Cell(1, 1)
             .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
             .SetTextAlignment(TextAlignment.CENTER)
             .Add(new Paragraph("Last Name"));

            Cell cellPhoneNumber = new Cell(1, 1)
             .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
             .SetTextAlignment(TextAlignment.CENTER)
             .Add(new Paragraph("Phone Number"));

            Cell cellGender = new Cell(1, 1)
             .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
             .SetTextAlignment(TextAlignment.CENTER)
             .Add(new Paragraph("Gender"));

            table.AddCell(cellFirstName);
            table.AddCell(cellLastName);
            table.AddCell(cellPhoneNumber);
            table.AddCell(cellGender);

            var patients = await patientRepository.GetAll();

            foreach (var item in patients)
            {
                Cell Cfn = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph(item.FirstName.ToString()));

                Cell Cln = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph(item.LastName.ToString()));

                Cell Cph = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph(item.PhoneNumber.ToString()));

                Cell Cgend = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph(item.Gender.ToString()));

                table.AddCell(Cfn);
                table.AddCell(Cln);
                table.AddCell(Cph);
                table.AddCell(Cgend);
            }

            return table;
        }
    }
}
