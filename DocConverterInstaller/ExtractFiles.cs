using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DocConverterInstaller
{
    internal class ExtractFiles : IInstallationStep
    {
        readonly Stream appZipFile;
        string destinationPath;
        public ExtractFiles()
        {
            appZipFile = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("DocConverterInstaller.Resources.docConverter.zip");
        }

        public async Task StartTask(object? data) => await Task.Run(() =>
        {
            var driveInfo = (DriveInfo)data;
            Console.WriteLine("Selected Drive: " + driveInfo.Name);
            Console.WriteLine("Installation has Started. Please wait while the installation complete\n");
            var destPath = driveInfo.Name + "PrajwalSoft\\Documents Converter";
            var zipArchive = new ZipArchive(appZipFile);
            zipArchive.ExtractToDirectory(destPath);
            destinationPath = destPath;
        });

        public void ChooseNextStep(ref InstallationSteps nextStep, ref object? data)
        {
            nextStep = InstallationSteps.AddToWindowsRegistry;
            data = destinationPath;
        }

    }
}
