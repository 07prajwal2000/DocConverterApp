using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocConverterInstaller
{
    internal enum InstallationSteps
    {
        Welcome,
        ExtractFiles,
        AddToWindowsRegistry,
        Success,
        Close
    }
}
