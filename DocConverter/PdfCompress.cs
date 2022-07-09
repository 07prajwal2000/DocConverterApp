using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System.Reflection;

namespace DocConverter
{
    internal class PdfCompress : IConverter
    {
        readonly string data;
        public PdfCompress()
        {
            using var stream1 = Assembly.GetExecutingAssembly().GetManifestResourceStream("DocConverter.Resources." + Constants.PdfCompress_ASCII);
            using var sr = new StreamReader(stream1);
            data = sr.ReadToEnd();
        }

        private void ShowDetails(FileInfo file)
        {
            Constants.ShowBaseDetails(file, data);

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Save Location : " + Path.Combine(file.DirectoryName!, file.Name.Split('.')[0] + ".docx"));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Starting Compression...\n");
        }

        public async Task Convert(FileInfo file) => await Task.Run(() => 
        {
            ShowDetails(file);

            var outputFile = Path.Combine(file.DirectoryName!, file.Name.Split('.')[0] + "_compressed" + ".pdf");
            FileStream inputDocument = new FileStream(file.FullName, FileMode.Open);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(inputDocument);
            PdfCompressionOptions options = new PdfCompressionOptions();
            loadedDocument.Compression = PdfCompressionLevel.Best;
            options.CompressImages = true;
            options.OptimizeFont = true;
            options.OptimizePageContents = true;
            options.ImageQuality = 30;
            loadedDocument.Compress(options);
            using var outputFs = new FileStream(outputFile, FileMode.OpenOrCreate);
            StartProgress();
            loadedDocument.Save(outputFs);
            ShowEnd(outputFile);
        });

        private void StartProgress()
        {
            Console.WriteLine("Conversion Started");
        }

        private void ShowEnd(string output)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Successfully Converted to PDF.");
            Console.WriteLine("File Path : " + output);
            Console.WriteLine("_____________________________________________________________________________________________\n");
            Console.ResetColor();
        }
    }
}
