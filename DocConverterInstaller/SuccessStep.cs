using System;
using System.Threading.Tasks;

namespace DocConverterInstaller
{
    internal class SuccessStep : IInstallationStep
    {
        public void ChooseNextStep(ref InstallationSteps nextStep, ref object? data)
        {
            nextStep = InstallationSteps.Close;
        }

        public async Task StartTask(object? data)
        {
            var destPath = (string)data;
            Console.WriteLine("Successfully installed");
            Console.WriteLine("Installation Location : " + destPath);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("To Uninstall the Application. Reopen the Installer, there you can find the Uninstall Option");
            Console.ResetColor();
        }
    }
}