using DodocoTales.Common;
using DodocoTales.Common.Enums;
using DodocoTales.Common.Signals;
using DodocoTales.Library;
using DodocoTales.Library.StoragedUser.Models;
using DodocoTales.Library.StoragedUser.Models.V0;
using DodocoTales.Library.Utils;
using DodocoTales.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader
{
    public class DDCGV0LogUpdater
    {
        public readonly string UserDataDirPath = "userdata";
        public readonly string UserDataFileSearchPattern = "userlog_*.json";
        public readonly string UserDataFileRegexPattern = @"userlog_(\d+)\.json";
        public readonly string UserDataFileOpenPattern = "userdata/userlog_{0}.json";
        public DDCGV0LogUpdater()
        {
            
        }
        public void Initialize()
        {
            DDCS.V0StyleLogFileDetacted += new DDCSUidParamDelegate(Convert);
        }
        public async Task<DDCLUserGachaLogV0> LoadLocalGachaLogByUidAsync(long uid)
        {
            string logfile = String.Format(UserDataFileOpenPattern, uid);
            DDCLog.Info(DCLN.Loader, String.Format("Loading userlog file: {0}", logfile));
            try
            {
                var stream = File.Open(logfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var response = JsonConvert.DeserializeObject<DDCLUserGachaLogV0>(await reader.ReadToEndAsync());
                if (response.uid != uid)
                {
                    DDCLog.Warning(DCLN.Lib, String.Format("{0}, UID:{1}", logfile, response.uid));
                }
                DDCLog.Info(DCLN.Loader, String.Format("V0-style userlog successfully loaded. UID:{0}", response.uid));
                return response;
            }
            catch (Exception e)
            {
                DDCLog.Error(DCLN.Loader, String.Format("Failed to load V0-style userlog. UID:{0}", uid), e);
                return null;
            }
        }
        public DDCLGachaLogItem ConvertToV1Item(DDCLStoragedGachaLogItem v0, DDCCPoolType pool)
        {
            return new DDCLGachaLogItem
            {
                id = v0.id,
                idlost = v0.idLost,
                name = v0.name,
                code = v0.code,
                pooltype = pool,
                gachatype = v0.gachatype == DDCCPoolType.Null ? (ulong)pool : (ulong)v0.gachatype,
                rank = v0.rank,
                time = v0.time,
                unitclass = v0.unitclass,
                unittype = v0.unittype
            };
        }
        public List<DDCLGachaLogItem>ConvertToList(DDCLUserGachaLogV0 v0)
        {
            var res = new SortedList<ulong, DDCLGachaLogItem>();
            DDCLInternalIdGenerator idgen = new DDCLInternalIdGenerator();
            foreach(var version in v0.V)
            {
                foreach(var banner in version.B)
                {
                    foreach(var round in banner.R)
                    {
                        foreach(var item in round.L)
                        {
                            res.Add(idgen.GenerateNextInternalId(item.time), ConvertToV1Item(item, banner.poolType));
                        }
                    }
                }
            }
            return res.Values.ToList();
        }
        public async void Convert(long uid)
        {
            var v0 = await LoadLocalGachaLogByUidAsync(uid);
            uid = v0.uid;
            DDCL.UserDataLib.TryAddEmptyUser(uid);
            var v1 = DDCL.UserDataLib.GetUserLogByUid(uid);
            v1.zone = v0.zone;
            // BannerVer
            var list = ConvertToList(v0);
            var merger = new DDCGGachaLogMerger(v1);
            merger.Merge(list);
        }
    }
}
