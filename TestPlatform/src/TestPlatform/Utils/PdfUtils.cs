using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.Utils
{
    public class PdfUtils
    {
        public static void GeneratePDF(string pathToPDFTemplate, string pathToNewPDF, PdfSymbols pdf)
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
