using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;

namespace DocConverterInstaller
{
	internal class Program
	{
		public static async Task Main(string[] args)
        {
            //ZipAndCopy();
            //Console.ReadLine();
            //return;
            var installedPath = CheckAppInstalled();
            if (installedPath is not null)
            {
                var beforeInstallation = new BeforeInstallation(installedPath);
                var continueInstallation = beforeInstallation.Start();
                if (!continueInstallation)
                {
                    return;
                }
            }
            await Start();
            // ___________

            //RegistryKeyTest();
        }

		private static async Task Start()
        {
            IInstallationStep step;
            var currentStep = InstallationSteps.Welcome;
            var data = new object();

            while (currentStep != InstallationSteps.Close)
            {
                switch (currentStep)
                {
                    case InstallationSteps.Welcome:
                        step = Factory<Welcome>();
                        break;
                    case InstallationSteps.ExtractFiles:
                        step = Factory<ExtractFiles>();
                        break;
                    case InstallationSteps.AddToWindowsRegistry:
                        step = Factory<AddToWindowsRegistry>();
                        break;
                    case InstallationSteps.Success:
                        step = Factory<SuccessStep>();
                        break;
                    case InstallationSteps.Close:
                        step = Factory<CloseInstallation>();
                        break;
                    default:
                        step = Factory<Welcome>();
                        break;
                }
                await step.StartTask(data);
                step.ChooseNextStep(ref currentStep, ref data);
            }
            Console.WriteLine("Press any key to close.");
            Console.Read();
        }

        private static T Factory<T>() where T : new() => new T();

        private static void ZipAndCopy()
        {
            var path = @"D:\Programming-Projects\PersonalProjects\DocConverterApp\DocConverter\bin\Release\net6.0\publish\win-x86";
            var dest = @"D:\Programming-Projects\PersonalProjects\DocConverterApp\DocConverterInstaller\Resources\docConverter.zip";
            File.Delete(dest);
            ZipFile.CreateFromDirectory(path, dest, CompressionLevel.Optimal, false);
            Console.WriteLine("Completed");
        }

        private static void RegistryKeyTest()
        {
            using var wordKey = Registry.ClassesRoot.CreateSubKey(@"*\shell\Word(docx) to PDF");
            
        }

        private static string? CheckAppInstalled()
        {
            using var key = Registry.ClassesRoot.OpenSubKey(@"*\DocConverterApp");
            var exePath = (string)key?.GetValue("Location");
            return exePath;
        }
    }
}