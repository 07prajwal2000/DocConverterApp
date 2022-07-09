using Syncfusion.Licensing;

namespace DocConverter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No Arguments are passed.");
                Console.ReadLine();
                return;
            }

            var file = new FileInfo(args[0]);
            string mode = "";
            if (args.Length >= 2)
            {
                mode = args[1];
            }
            SyncfusionLicenseProvider.RegisterLicense(Constants.LicenseKey);
            try
            {
                if (!file.Exists)
                {
                    Console.WriteLine("Error no file found at " + file.FullName);
                    return;
                }
                else if (file.Extension == ".doc" || file.Extension == ".docx")
                {
                    IConverter converter = new DocToPdfConvert();
                    await converter.Convert(file);
                }
                else if (file.Extension == ".pdf" && mode.Contains("-c"))
                {
                    IConverter converter = new PdfCompress();
                    await converter.Convert(file);
                }
                else if (file.Extension == ".pdf")
                {
                    IConverter converter = new PdfToDoc();
                    await converter.Convert(file);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                throw;
            }
            Console.WriteLine("Press any key to close.");
            Console.ReadLine();

        }
    }
}