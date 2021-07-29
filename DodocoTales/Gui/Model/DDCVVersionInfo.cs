using DodocoTales.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Common.Models;
namespace DodocoTales.Gui.Model
{
    public class DDCVVersionInfo
    {
        public DDCLVersionInfo Info { get; set; }
        public DDCCVersionLogs Logs { get; set; }
        public DDCVInheritedRounds Inherited { get; set; }
    }
}
