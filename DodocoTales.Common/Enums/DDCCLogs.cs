using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Logs
{
    public enum DDCLogType
    {
        Info,
        Warning,
        Error,
    }
    public enum DCLN
    {
        Log,        // DodocoTales.Logs        (DodocoTales.Common)
        Common,     // DodocoTales.Common      (DodocoTales.Common)
        Lib,        // DodocoTales.Library     (DodocoTales.Library)
        Loader,
        Gui,
        Debug,

    }
}
