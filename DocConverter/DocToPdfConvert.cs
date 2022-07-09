using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.DocIO.DLS;
using Syncfusion.Licensing;
using Syncfusion.Pdf;

namespace DocConverter
{
    internal class DocToPdfConvert : IConverter
    {
        readonly string data;
        public DocToPdfConvert()
        {
            using var stream1 = Assembly.GetExecutingAssembly().GetManifestResourceStream("DocConverter.Resources." + Constants.DocToPdf_ASCII);
            using var sr = new StreamReader(stream1);
            data = sr.ReadToEnd();
        }
        private void ShowDetails(FileInfo file)
        {
            Constants.ShowBaseDetails(file, data);
            
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Save Location : " + Path.Combine(file.DirectoryName, file.Name.Split('.')[0] + ".pdf"));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Starting Conversion...\n");
        }

        public async Task Convert(FileInfo file) => await Task.Run(async () =>
        {
            ShowDetails(file);

            await Task.Delay(1000);

            var inputFile = file.FullName;
            var outputFile = Path.Combine(file.DirectoryName, file.Name.Split('.')[0] + "_converted" + ".pdf");

            using var fileStream = File.OpenRead(inputFile);
            using WordDocument doc = new WordDocument(fileStream, Syncfusion.DocIO.FormatType.Automatic);
            using Syncfusion.DocIORenderer.DocIORenderer renderer = new Syncfusion.DocIORenderer.DocIORenderer();
            using var pdfDoc = renderer.ConvertToPDF(doc);
            using var outputFs = File.Open(outputFile, File.Exists(outputFile) ? FileMode.Truncate : FileMode.OpenOrCreate);
            
            StartProgress();
            pdfDoc.SaveProgress += OnSaving;
            pdfDoc.Save(outputFs);
            ShowEnd(outputFile, file);
            pdfDoc.SaveProgress -= OnSaving;
        });

        private void StartProgress()
        {
            Console.WriteLine("Conversion Started");
            Console.Write("Loading [ ..");
        }

        private void ShowEnd(string output, FileInfo file)
        {
            Console.WriteLine(" ]");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Successfully Converted to PDF.");
            Console.WriteLine("File Path : " + output);
            Console.WriteLine("_____________________________________________________________________________________________\n");
            Console.ResetColor();
        }

        private void OnSaving(object sender, ProgressEventArgs arguments)
        {
            Console.Write("..");
        }
    }
}
