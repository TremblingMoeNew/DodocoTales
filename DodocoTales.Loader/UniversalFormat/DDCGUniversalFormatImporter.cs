using DodocoTales.Common.Enums;
using DodocoTales.Library;
using DodocoTales.Library.StoragedUser.Models;
using DodocoTales.Library.Utils;
using DodocoTales.Loader.UniversalFormat.Models;
using DodocoTales.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.UniversalFormat
{
    public class DDCGUniversalFormatImporter
    {
        public static readonly ulong UFMaxCustomId = 1600000000000000000;

        public DDCLUserGachaLog InitializeLog(DDCGUFUserGachaLog uf)
        {
            if (uf == null || uf.info == null)
            {
                DDCLog.Error(DCLN.Loader,"UF metadata not found.");
                return null;
            }
            long uid = 0;
            try
            {
                uid = Convert.ToInt64(uf.info.uid);
            }
            catch
            {
                DDCLog.Error(DCLN.Loader,"UF uid not found.");
                return null;
            }
            DDCL.UserDataLib.TryAddEmptyUser(uid);
            return DDCL.UserDataLib.GetUserLogByUid(uid);
        }
        public DDCLGachaLogItem ConvertToDDCItem(DDCGUFLogItem uf)
        {
            DDCLGachaLogItem res = null;
            try
            {
                var unitclass = DDCL.UnitLib.GetItemByName(uf.name);
                if (unitclass.type == DDCCUnitType.Unknown)
                {
                    unitclass = DDCL.UnitLib.GetItemByCode(uf.name);
                    if (unitclass.type == DDCCUnitType.Unknown) throw new Exception();
                } 
                res = new DDCLGachaLogItem
                {
                    rank = Convert.ToUInt64(uf.rank_type),
                    time = Convert.ToDateTime(uf.time),
                    gachatype = Convert.ToUInt64(uf.gacha_type),
                    id = Convert.ToUInt64(uf.id),
                    name = unitclass.name,
                    code = unitclass.code,
                    unitclass = unitclass.id,
                    unittype = unitclass.type,
                };
                if (res.id < DDCGUniversalFormatImporter.UFMaxCustomId)
                {
                    res.idlost = true;
                }
                switch (uf.uigf_gacha_type)
                {
                    case "100":
                        res.pooltype = DDCCPoolType.Beginner;
                        break;
                    case "200":
                        res.pooltype = DDCCPoolType.Permanent;
                        break;
                    case "301":
                        res.pooltype = DDCCPoolType.EventCharacter;
                        break;
                    case "302":
                        res.pooltype = DDCCPoolType.EventWeapon;
                        break;
                    default:
                        throw new Exception();
                }
            }
            catch
            {
                DDCLog.Warning(DCLN.Loader, string.Format("Failed to convert UF log item. {0}", JsonConvert.SerializeObject(uf)));
                return null;
            }
            return res;
        }

        public List<DDCLGachaLogItem> ConvertList(DDCGUFUserGachaLog uf)
        {
            DDCLInternalIdGenerator idgen = new DDCLInternalIdGenerator();
            var res = new SortedList<ulong,DDCLGachaLogItem>();
            if (uf == null || uf.list == null)
            {
                return res.Values.ToList();
            }
            foreach(var ufitem in uf.list)
            {
                var item = ConvertToDDCItem(ufitem);
                if (item != null) res.Add(idgen.GenerateNextInternalId(item.time), item);
            }
            return res.Values.ToList();
        }

        public void Import(DDCGUFUserGachaLog uf)
        {
            var userlog = InitializeLog(uf);
            if (userlog == null)
            {
                // TODO
                return;
            }
            var list = ConvertList(uf);
            var merger = new DDCGGachaLogMerger(userlog);
            merger.Merge(list);
        }



        public async Task<DDCGUFUserGachaLog> LoadUFJsonAsync(string path)
        {
            DDCLog.Info(DCLN.Lib, String.Format("Loading UF userlog file: {0}", path));
            try
            {
                var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var response = JsonConvert.DeserializeObject<DDCGUFUserGachaLog>(await reader.ReadToEndAsync());
                DDCLog.Info(DCLN.Loader, String.Format("UF userlog successfully loaded."));
                return response;
            }
            catch (Exception e)
            {
                DDCLog.Error(DCLN.Loader, String.Format("Failed to load UF userlog: {0}", path), e);
                return null;
            }
        }

    }
}
