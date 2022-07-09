using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocConverter
{
    internal interface IConverter
    {
        Task Convert(FileInfo file);
    }
}
