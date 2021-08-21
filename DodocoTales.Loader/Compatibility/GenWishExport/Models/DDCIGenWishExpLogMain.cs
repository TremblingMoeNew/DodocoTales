using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.Compatibility.GenWishExport.Models
{
    public class DDCIGenWishExpLogMain
    {
        public List<JArray> result { get; set; }
        public string uid { get; set; }
        public string lang { get; set; }
    }
}
