using iTextSharp.text.pdf;
using Microsoft.AspNet.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.Utils
{
    public class PdfUtils
    {
        // Example: 
        // PdfUtils.GenerateCerfificate(env, "test.pdf", "cerBOficat2.pdf", new PdfSymbols { FirstName = "BO" });

        public static void GenerateCerfificate(IHostingEnvironment env, string templateName, string outputName, PdfSymbols pdfSymbols)
        {
            var root = new Uri(env.WebRootPath);
            var rootParent = root.AbsoluteUri.Remove(root.AbsoluteUri.Length - root.Segments.Last().Length);
            var templatePath = rootParent + $@"PDF/Templates/{templateName}";
            var outputPath = rootParent + $@"PDF/OutPut/{outputName}";

            var fileLength = "file:///".Length;

            PdfUtils.GeneratePDF(templatePath.Substring(fileLength, templatePath.Count() - fileLength)
                , outputPath.Substring(fileLength, outputPath.Count() - fileLength)
                , pdfSymbols);
        }

        private static void GeneratePDF(string pathToPDFTemplate, string pathToNewPDF, PdfSymbols pdf)
        {
            using (var existingFileStream = new FileStream(pathToPDFTemplate, FileMode.Open))
            using (var newFileStream = new FileStream(pathToNewPDF, FileMode.Create))
            using (var pdfReader = new PdfReader(existingFileStream))
            using (var stamper = new PdfStamper(pdfReader, newFileStream))
            {
                var form = stamper.AcroFields;
                form.GenerateAppearances = true;

                if (form.Fields.Keys.Contains(nameof(pdf.CertificatName)) && pdf.CertificatName != null)
                    form.SetField(nameof(pdf.CertificatName), pdf.CertificatName);

                if (form.Fields.Keys.Contains(nameof(pdf.Date)) && pdf.Date != null)
                    form.SetField(nameof(pdf.Date), pdf.Date);

                if (form.Fields.Keys.Contains(nameof(pdf.Details)) && pdf.Details != null)
                    form.SetField(nameof(pdf.Details), pdf.Details);

                if (form.Fields.Keys.Contains(nameof(pdf.StudentName)) && pdf.StudentName != null)
                    form.SetField(nameof(pdf.StudentName), pdf.StudentName);

                if (form.Fields.Keys.Contains(nameof(pdf.Author)) && pdf.Author != null)
                    form.SetField(nameof(pdf.Author), pdf.Author);

                if (form.Fields.Keys.Contains(nameof(pdf.Company)) && pdf.Company != null)
                    form.SetField(nameof(pdf.Company), pdf.Company);

                stamper.FormFlattening = true;
            }
        }
    }

    public class PdfSymbols
    {
        public PdfSymbols()
        {
            Date = DateTime.Now.Date.ToString("dd/MM/yyyy");
        }
        public string CertificatName { get; set; }
        public string Date { get; set; }
        public string Details { get; set; }
        public string StudentName { get; set; }
        public string Author { get; set; }
        public string Company { get; set; }
    }
}
