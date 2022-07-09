using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocConverterInstaller
{
    internal interface IInstallationStep
    {
        Task StartTask(object? data);
        void ChooseNextStep(ref InstallationSteps nextStep, ref object? data);
    }
}
