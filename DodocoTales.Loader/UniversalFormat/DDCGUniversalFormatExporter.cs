using DodocoTales.Common.Enums;
using DodocoTales.Library;
using DodocoTales.Library.StoragedUser.Models;
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
    /// <summary>
    /// 将记录导出至通用交换格式
    /// </summary>
    public class DDCGUniversalFormatExporter
    {
        public DDCGUFLogItem ConvertToUFItem(DDCLGachaLogItem item)
        {
            var unitclass = DDCL.UnitLib.GetItemById(item.unitclass);
            if (unitclass.type == DDCCUnitType.Unknown) return null;
            var res = new DDCGUFLogItem
            {
                gacha_type = item.gachatype.ToString(),
                item_type = item.unittype.ToString(),
                rank_type = item.rank.ToString(),
                uigf_gacha_type = ((int)item.pooltype).ToString(),
                time = string.Format("{0:yyyy-MM-dd HH:mm:ss}", item.time),
                uid="0",
                name = item.name
            };
            if (!item.idlost) res.id = item.id.ToString();
            return res;
        }

        public List<DDCGUFLogItem> ConvertToUFLogItemList(List<DDCLGachaLogItem> list)
        {
            List<DDCGUFLogItem> res = new List<DDCGUFLogItem>();
            foreach(var item in list)
            {
                var t = ConvertToUFItem(item);
                if (t != null) res.Add(t);
            }
            return res;
        }
        public DDCGUFMetaInfo CreateMetaInfo(long uid)
        {
            return new DDCGUFMetaInfo
            {
                uid = uid.ToString(),
                export_time = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now),
                export_app = "DodocoTales",
                export_app_version = "0",// TODO: APP Version
                uigf_version = "v2.1",
                lang = "zh-cn"
                
            };
        }

        public DDCGUFUserGachaLog CreateUFLog(long uid)
        {
            return new DDCGUFUserGachaLog
            {
                info = CreateMetaInfo(uid),
                list = ConvertToUFLogItemList(DDCL.UserDataLib.GetUserLogByUid(uid).Logs)
            };
        }
        public async void ExportAsJson(long uid, string dir)
        {
            string path = string.Format("{0}/DDCExport_{2}_{1:yyyyMMdd_HHmmss}.json", dir, DateTime.Now, uid);
            try
            {
                var stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter writer = new StreamWriter(stream);
                var serialized = JsonConvert.SerializeObject(CreateUFLog(uid), Formatting.Indented);
                await writer.WriteAsync(serialized);
                await writer.FlushAsync();
                stream.Close();
                DDCLog.Info(DCLN.Loader, String.Format("Userlog exported as universal format. UID:{0}", uid));
            }
            catch (Exception e)
            {
                DDCLog.Error(DCLN.Loader, String.Format("Failed to exported userlog. UID:{0}", uid), e);
            }
        }

    }
}
