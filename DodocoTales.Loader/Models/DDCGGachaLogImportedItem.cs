using DodocoTales.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.Models
{
    public class DDCGGachaLogImportedItem : DDCCGachaLogItem
    {
        [JsonIgnore]
        public bool ItemUnitClassNotFound { get; set; }
        [JsonIgnore]
        public bool IsNewItem { get; set; }
    }
}
