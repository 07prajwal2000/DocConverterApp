using Microsoft.Win32;
using System.Threading.Tasks;
#nullable enable
namespace DocConverterInstaller
{
    internal class AddToWindowsRegistry : IInstallationStep
    {
        private string destinationPath;

        public async Task StartTask(object? data) => await Task.Run(() =>
        {
            destinationPath = (string)data!;

            AddWordDocxToPdfRegistryKey();
            AddCompressRegistryKey();
            AddPdfToDocRegistryKey();
        });

        public void ChooseNextStep(ref InstallationSteps nextStep, ref object? data)
        {
            nextStep = InstallationSteps.Success;
            AddAppPathToRegistry();
            data = destinationPath;
        }

        private void AddAppPathToRegistry()
        {
            using var wordKey = Registry.ClassesRoot.CreateSubKey(@"*\DocConverterApp");
            wordKey.SetValue("Location", destinationPath);
        }

        private void AddWordDocxToPdfRegistryKey()
        {
            using var wordKey = Registry.ClassesRoot.CreateSubKey(@"*\shell\Word(docx) to PDF");
            wordKey.SetValue("", "Word to PDF", RegistryValueKind.String);
            wordKey.SetValue("AppliesTo", ".docx", RegistryValueKind.String);
            wordKey.SetValue("Icon", destinationPath + @"\Resources\icon.ico", RegistryValueKind.String);
            using var cmdKey = wordKey.CreateSubKey("Command");
            cmdKey.SetValue("", "\"" + destinationPath + "\\DocConverter.exe\"" + "\"%V\"", RegistryValueKind.String);
            wordKey.Close();
            cmdKey.Close();
        }

        private void AddCompressRegistryKey()
        {
            using var wordKey = Registry.ClassesRoot.CreateSubKey(@"*\shell\PdfCompress");
            wordKey.SetValue("", "Compress PDF", RegistryValueKind.String);
            wordKey.SetValue("AppliesTo", ".pdf", RegistryValueKind.String);
            wordKey.SetValue("Icon", destinationPath + @"\Resources\icon.ico", RegistryValueKind.String);

            using var cmdKey = wordKey.CreateSubKey("Command");
            cmdKey.SetValue("","\"" + destinationPath + "\\DocConverter.exe\"" + " \"%V\" \"-c\"", RegistryValueKind.String);
            wordKey.Close();
            cmdKey.Close();
        }

        private void AddPdfToDocRegistryKey()
        {
            using var wordKey = Registry.ClassesRoot.CreateSubKey(@"*\shell\PdfToWord");
            wordKey.SetValue("", "Pdf To Word", RegistryValueKind.String);
            wordKey.SetValue("AppliesTo", ".pdf", RegistryValueKind.String);
            wordKey.SetValue("Icon", destinationPath + @"\Resources\icon.ico", RegistryValueKind.String);

            using var cmdKey = wordKey.CreateSubKey("Command");
            cmdKey.SetValue("", "\"" + destinationPath + "\\DocConverter.exe\"" + " \"%V\"", RegistryValueKind.String);
            wordKey.Close();
            cmdKey.Close();
        }
    }
}