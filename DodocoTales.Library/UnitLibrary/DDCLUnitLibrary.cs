using DodocoTales.Common;
using DodocoTales.Common.Enums;
using DodocoTales.Library.UnitLibrary.Models;
using DodocoTales.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.UnitLibrary
{
    public class DDCLUnitLibrary
    {
        public List<DDCLUnitInfo> L;
        public Dictionary<ulong, DDCLUnitInfo> idMap;
        public Dictionary<string, DDCLUnitInfo> codeMap;
        public Dictionary<string, DDCLUnitInfo> nameMap;
        public readonly DDCLUnitInfo Unknown = new DDCLUnitInfo { id = 0, code = "Unknown", name = "Unknown", type = DDCCUnitType.Unknown };
        public readonly string libPath = @"library/UnitLibrary.json";

        public DDCLUnitLibrary()
        {
            L = new List<DDCLUnitInfo>();
            idMap = new Dictionary<ulong, DDCLUnitInfo>();
            codeMap = new Dictionary<string, DDCLUnitInfo>();
            nameMap = new Dictionary<string, DDCLUnitInfo>();
        }


        public async Task<bool> LoadLibraryAsync()
        {
            idMap.Clear();
            codeMap.Clear();
            nameMap.Clear();
            try
            {
                var stream = File.Open(libPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var buffer = await reader.ReadToEndAsync();
                L = JsonConvert.DeserializeObject<List<DDCLUnitInfo>>(buffer);
                CreateMap();
            }
            catch (Exception)
            {
                idMap.Clear();
                codeMap.Clear();
                nameMap.Clear();
                L = null;
                DDCLog.Error(DCLN.Lib, "Failed to load userlib");
                DDCS.Emit_UnitLibReloadFailed();
                return false;
            }
            DDCLog.Info(DCLN.Lib, "Userlib successfully loaded.");
            DDCS.Emit_UnitLibReloadCompleted();
            return true;
        }
        void CreateMap()
        {
            foreach (var item in L)
            {
                idMap.Add(item.id, item);
                codeMap.Add(item.code, item);
                if (item.code_alias != null) codeMap.Add(item.code_alias, item);
                nameMap.Add(item.name, item);
            }
        }
        public DDCLUnitInfo GetItemById(ulong id)
        {
            if (idMap.TryGetValue(id, out DDCLUnitInfo info)) return info; else return Unknown;
        }
        public DDCLUnitInfo GetItemByCode(string code)
        {
            if (codeMap.TryGetValue(code, out DDCLUnitInfo info)) return info; else return Unknown;
        }
        public DDCLUnitInfo GetItemByName(string name)
        {
            if (nameMap.TryGetValue(name, out DDCLUnitInfo info)) return info; else return Unknown;
        }
    }
}
