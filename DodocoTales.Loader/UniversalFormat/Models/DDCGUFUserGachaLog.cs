using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.UniversalFormat.Models
{
    public class DDCGUFUserGachaLog
    {
        public DDCGUFMetaInfo info { get; set; }
        public List<DDCGUFLogItem> list { get; set; }
    }
}
