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

                if (form.Fields.Keys.Contains(nameof(pdf.FirstName)) && pdf.FirstName != null)
                    form.SetField(nameof(pdf.FirstName), pdf.FirstName);

                if (form.Fields.Keys.Contains(nameof(pdf.LastName)) && pdf.LastName != null)
                    form.SetField(nameof(pdf.LastName), pdf.LastName);

                stamper.FormFlattening = true;
            }
        }
    }

    public class PdfSymbols
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Add your template fields here
        public string Whatever { get; set; }
    }
}
