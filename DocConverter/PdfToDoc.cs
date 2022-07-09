using Syncfusion.DocIO.DLS;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DocConverter
{
    internal class PdfToDoc : IConverter
    {
        readonly string data;
        private readonly string owner;

        public PdfToDoc()
        {
            var stream1 = Assembly.GetExecutingAssembly().GetManifestResourceStream("DocConverter.Resources." + Constants.PdfToDoc_ASCII);
            var stream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("DocConverter.Resources.Owner.txt");
            using var sr1 = new StreamReader(stream1);
            using var sr2 = new StreamReader(stream2);
            data = sr1.ReadToEnd();
            owner = sr2.ReadToEnd();
        }

        private void ShowDetails(FileInfo file)
        {
            Constants.ShowBaseDetails(file, data);

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Save Location : " + Path.Combine(file.DirectoryName!, file.Name.Split('.')[0] + ".docx"));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Starting Conversion...\n");
        }

        public async Task Convert(FileInfo file) => await Task.Run(() =>
        {
            ShowDetails(file);
            var inputFile = file.FullName;
            var outputFile = Path.Combine(file.DirectoryName!, file.Name.Split('.')[0] + "_converted" + ".docx");
            Aspose.Pdf.Document pdfDoc = new Aspose.Pdf.Document(inputFile);
            pdfDoc.DisableFontLicenseVerifications = true;
            pdfDoc.Save(outputFile, Aspose.Pdf.SaveFormat.DocX);

            using var fs = new FileStream(outputFile, FileMode.Open);
            using WordDocument doc = new WordDocument(fs, Syncfusion.DocIO.FormatType.Automatic);
            var selection = doc.FindAll(@"Evaluation Only. Created with Aspose.PDF. Copyright 2002-2022 Aspose Pty Ltd.", false, true);
            foreach (var sel in selection)
            {
                sel.GetAsOneRange().Text = "";
            }
            StartProgress();
            doc.Save(fs, Syncfusion.DocIO.FormatType.Docx);
            ShowEnd(outputFile, file);
        });

        private void StartProgress()
        {
            Console.WriteLine("Conversion Started");
        }

        private void ShowEnd(string output, FileInfo file)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Successfully Converted to PDF.");
            Console.WriteLine("File Path : " + output);
            Console.WriteLine("_____________________________________________________________________________________________\n");
            Console.ResetColor();
        }
    }
}
