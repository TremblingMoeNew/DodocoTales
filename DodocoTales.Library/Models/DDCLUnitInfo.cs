using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Common.Enums;
namespace DodocoTales.Library.Models
{
    public partial class DDCLUnitInfo
    {
        public int id { get; set; }
        public DDCCUnitType type { get; set; }
        public int rank { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string portrait { get; set; }
    }
}
