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
    public partial class DDCLItemLibrary
    {
        public List<DDCLItemInfo> library;
        public Dictionary<int, DDCLItemInfo> idMap;
        public Dictionary<string, DDCLItemInfo> codeMap;
        readonly DDCLItemInfo notfound = new DDCLItemInfo { id = 0, code = "Unknown", name = "Unknown", type = DDCCItemType.Unknown };
        public readonly string libPath = @"library/ItemLibrary.json";

        public DDCLItemLibrary()
        {
            library = new List<DDCLItemInfo>();
            idMap = new Dictionary<int, DDCLItemInfo>();
            codeMap = new Dictionary<string, DDCLItemInfo>();
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
                library = JsonConvert.DeserializeObject<List<DDCLItemInfo>>(buffer);
                createMap();
            }
            catch(Exception)
            {
                idMap.Clear();
                codeMap.Clear();
                library = null;
                return false;
            }
            return true;
        }
        void createMap()
        {
            foreach (var item in library)
            {
                idMap.Add(item.id, item);
                codeMap.Add(item.code, item);
            }
        }
        public DDCLItemInfo getItemById(int id)
        {
            if (idMap.TryGetValue(id, out DDCLItemInfo info)) return info; else return notfound;
        }
        public DDCLItemInfo getItemByCode(string code)
        {
            if (codeMap.TryGetValue(code, out DDCLItemInfo info)) return info; else return notfound;
        }
    }
}
