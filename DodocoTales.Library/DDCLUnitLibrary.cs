using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Common.Enums;
using DodocoTales.Library.Models;
using Newtonsoft.Json;

namespace DodocoTales.Library
{
    public partial class DDCLUnitLibrary
    {
        public List<DDCLUnitInfo> L;
        public Dictionary<int, DDCLUnitInfo> idMap;
        public Dictionary<string, DDCLUnitInfo> codeMap;
        readonly DDCLUnitInfo notfound = new DDCLUnitInfo { id = 0, code = "Unknown", name = "Unknown", type = DDCCUnitType.Unknown };
        public readonly string libPath = @"library/UnitLibrary.json";

        public DDCLUnitLibrary()
        {
            L = new List<DDCLUnitInfo>();
            idMap = new Dictionary<int, DDCLUnitInfo>();
            codeMap = new Dictionary<string, DDCLUnitInfo>();
        }

        
        public bool loadLibrary()
        {
            idMap.Clear();
            codeMap.Clear();
            try
            {
                var stream = File.Open(libPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var buffer = reader.ReadToEnd();
                L = JsonConvert.DeserializeObject<List<DDCLUnitInfo>>(buffer);
                createMap();
            }
            catch(Exception)
            {
                idMap.Clear();
                codeMap.Clear();
                L = null;
                return false;
            }
            return true;
        }
        void createMap()
        {
            foreach (var item in L)
            {
                idMap.Add(item.id, item);
                codeMap.Add(item.code, item);
                if (item.code_alias != null) codeMap.Add(item.code_alias, item);
            }
        }
        public DDCLUnitInfo getItemById(int id)
        {
            if (idMap.TryGetValue(id, out DDCLUnitInfo info)) return info; else return notfound;
        }
        public DDCLUnitInfo getItemByCode(string code)
        {
            if (codeMap.TryGetValue(code, out DDCLUnitInfo info)) return info; else return notfound;
        }
    }
}
