using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocConverterInstaller
{
    internal class CloseInstallation : IInstallationStep
    {
        public void ChooseNextStep(ref InstallationSteps nextStep, ref object? data)
        {
            nextStep = InstallationSteps.Close;
        }

        public async Task StartTask(object? data)
        {
            Console.ResetColor();
        }
    }
}
