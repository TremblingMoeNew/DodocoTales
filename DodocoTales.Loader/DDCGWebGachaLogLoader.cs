﻿using DodocoTales.Common;
using DodocoTales.Common.Enums;
using DodocoTales.Library;
using DodocoTales.Library.Enums;
using DodocoTales.Library.StoragedUser.Models;
using DodocoTales.Library.Utils;
using DodocoTales.Loader.Models;
using DodocoTales.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DodocoTales.Loader
{
    public partial class DDCGWebGachaLogLoader
    {
        readonly string locallow = Environment.GetEnvironmentVariable("USERPROFILE") + @"/AppData/LocalLow/miHoYo";

        public DDCLGameClientType ClientType;
        public string ApiPattern
        {
            get
            {
                if (ClientType == DDCLGameClientType.CN)
                    return apipattern_cn;
                else if (ClientType == DDCLGameClientType.Global)
                    return apipattern_os;
                else
                    return null;
            }
        }
        readonly string apipattern_cn = @"https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog?{0}&gacha_type={1}&page={2}&size={4}&end_id={3}";
        readonly string apipattern_os = @"https://hk4e-api-os.mihoyo.com/event/gacha_info/api/getGachaLog?{0}&gacha_type={1}&page={2}&size={4}&end_id={3}";

        HttpClient client;

        public DDCGWebGachaLogLoader()
        {
            client = new HttpClient();
        }

        private string GetAuthKey()
        {
            SortedList<long,string> output_logs = new SortedList<long,string>();
            string log;

            string output_file_cn = locallow + @"/原神/output_log.txt";
            if (File.Exists(output_file_cn))
            {
                DDCLog.Info(DCLN.Loader, "CN-client logs found.");
                output_logs.Add(File.GetLastWriteTime(output_file_cn).Ticks, output_file_cn);
            }

            string output_file_os = locallow + @"/Genshin Impact/output_log.txt";
            if (File.Exists(output_file_os))
            {
                DDCLog.Info(DCLN.Loader, "OS-client logs found.");
                output_logs.Add(File.GetLastWriteTime(output_file_os).Ticks, output_file_os);
            }

            foreach(var output_file in output_logs.Values.Reverse())
            {
                try
                {
                    var stream = File.Open(output_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader reader = new StreamReader(stream);
                    log = reader.ReadToEnd();
                }
                catch (Exception)
                {
                    continue;
                }
                string pattern = @"OnGetWebViewPageFinish:.+\?(.+)#/log";
                var result = Regex.Matches(log, pattern);
                if (result.Count != 0)
                {
                    Regex regex = new Regex(@"lang=.+&device_type");
                    DDCLog.Info(DCLN.Loader, "Authkey captured from client logs.");
                    return regex.Replace(result[result.Count - 1].Groups[1].Value, "lang=en&device_type");
                }
            }
            DDCLog.Info(DCLN.Loader, "Authkey not found.");
            return null;

        }

        public async Task<List<DDCGGachaLogResponseItem>> GetGachaLogAsync(string authkey, int pageid, DDCCPoolType type, ulong lastid, int size = 6, int retrycnt=0)
        {
            if (ApiPattern == null)
            {
                return null;
            }
            var api = String.Format(
                ApiPattern,
                authkey, (int)type, pageid, lastid, size
            );
            Thread.Sleep(200);
            string stringresponse;
            try
            {
                stringresponse = await client.GetStringAsync(api);
            }
            catch
            {
                DDCLog.Warning(DCLN.Loader, "MiHoYo-API connection timeout.");
                if (retrycnt == 3) return null;
                DDCS.Emit_ImportConnectionTimeout();
                Thread.Sleep(1000);
                DDCS.Emit_ImportConnectionRetry();
                return await GetGachaLogAsync(authkey, pageid, type, lastid, size);
            }
            var response = JsonConvert.DeserializeObject<DDCGGachaLogResponse>(stringresponse);
            if (response.retcode != 0)
            {
                if (response.retcode == -110)
                {
                    DDCLog.Warning(DCLN.Loader, "MiHoYo-API connection throttled.");
                    if (retrycnt > 10) return null;
                    DDCS.Emit_ImportConnectionThrottled();
                    if (retrycnt > 5)
                    {
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);
                    DDCS.Emit_ImportConnectionRetry();
                    return await GetGachaLogAsync(authkey, pageid, type, lastid, size);
                }
                DDCLog.Warning(DCLN.Loader, String.Format("Unknown retcode {0}: {1}", response.retcode, response.message));
                return null;
            }
            return response.data.list;
        }

        public async Task<List<DDCGGachaLogResponseItem>> GetGachaLogsByTypeAsync(string authkey, DDCCPoolType type, ulong endid, ulong beginid = 0, int size = 6)
        {
            var res = new List<DDCGGachaLogResponseItem>();
            DDCLog.Info(DCLN.Loader, String.Format("Fetching gachalogs in {0} pool of {1}-{2}", type, endid, beginid));
            ulong lastid = beginid;
            bool run = true;
            for (int i = 1; run; i++)
            {
                DDCS.Emit_ImportStatusFromWebRefreshed(type, i);
                var list = await GetGachaLogAsync(authkey, i, type, lastid, size);
                if (list == null)
                {
                    return res;
                }
                foreach (var item in list)
                {
                    lastid = item.id;
                    if (lastid == endid)
                    {
                        run = false;
                        break;
                    }
                    res.Add(item);
                }
                if (list.Count < size) run = false;
            }
            res.Reverse();
            return res;
        }


        public async Task<long> TryConnectAndGetUid(string authkey, DDCLGameClientType clientType)
        {
            ClientType = clientType;
            var uid = await GetUidFromWeb(authkey);
            if (uid < 0)
            {
                uid = await GetUidFromWeb(authkey);
            }
            if (uid == -1) uid = GetUidFromLocal();
            return uid;
        }

        public async Task<long> GetUidFromWeb(string authkey)
        {
            var bl = await GetGachaLogAsync(authkey, 1, DDCCPoolType.Beginner, 0);
            if (bl == null) return -2;
            bl.AddRange(await GetGachaLogAsync(authkey, 1, DDCCPoolType.Permanent, 0));
            if (bl.Count == 0)
            {
                bl.AddRange(await GetGachaLogAsync(authkey, 1, DDCCPoolType.EventCharacter, 0));
                bl.AddRange(await GetGachaLogAsync(authkey, 1, DDCCPoolType.EventWeapon, 0));
            }
            if (bl.Count == 0)
                return -1;
            else
                return bl[0].uid;
        }

        public long GetUidFromLocal()
        {
            string output_file, log;
            long res = -1;
            if (ClientType == DDCLGameClientType.CN && Directory.Exists(locallow + @"/原神"))
            {
                output_file = locallow + @"/原神/UidInfo.txt";
            }
            else if (ClientType == DDCLGameClientType.Global && Directory.Exists(locallow + @"/Genshin Impact"))
            {
                output_file = locallow + @"/Genshin Impact/UidInfo.txt";
            }
            else
            {
                return -1;
            }
            try
            {
                var stream = File.Open(output_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                log = reader.ReadToEnd();
                res = Convert.ToInt64(log);
            }
            catch (Exception)
            {
                return -1;
            }
            return res;
        }

        public DDCLGachaLogItem ConvertToDDCLLogItem(DDCGGachaLogResponseItem DDCGItem, DDCCPoolType pooltype)
        {
            var unitclass = DDCL.UnitLib.GetItemByCode(DDCGItem.name);
            return new DDCLGachaLogItem()
            {
                id = DDCGItem.id,
                code = DDCGItem.name,
                rank = DDCGItem.rank_type,
                time = DDCGItem.time,
                unittype = DDCGItem.item_type,
                unitclass = unitclass.id,
                name = unitclass.type == DDCCUnitType.Unknown ? DDCGItem.name : unitclass.name,
                gachatype = DDCGItem.gacha_type,
                pooltype = pooltype

            };
        }


        public async Task GetGachaLogsAsNormalMode(string authkey, DDCLGameClientType clientType)
        {
            ClientType = clientType;
            var merger = new DDCGGachaLogMerger(DDCL.CurrentUser.OriginalLogs);
            var res = new SortedList<ulong, DDCLGachaLogItem>();
            DDCLInternalIdGenerator idgen = new DDCLInternalIdGenerator();
            foreach (var item in await GetGachaLogsByTypeAsync(authkey, DDCCPoolType.Beginner, merger.GetLastItemMihoyoIdByType(DDCCPoolType.Beginner)))
            {
                res.Add(idgen.GenerateNextInternalId(item.time),ConvertToDDCLLogItem(item, DDCCPoolType.Beginner));
            }
            foreach (var item in await GetGachaLogsByTypeAsync(authkey, DDCCPoolType.Permanent, merger.GetLastItemMihoyoIdByType(DDCCPoolType.Permanent)))
            {
                res.Add(idgen.GenerateNextInternalId(item.time), ConvertToDDCLLogItem(item, DDCCPoolType.Permanent));
            }
            foreach (var item in await GetGachaLogsByTypeAsync(authkey, DDCCPoolType.EventCharacter, merger.GetLastItemMihoyoIdByType(DDCCPoolType.EventCharacter)))
            {
                res.Add(idgen.GenerateNextInternalId(item.time), ConvertToDDCLLogItem(item, DDCCPoolType.EventCharacter));
            }
            foreach (var item in await GetGachaLogsByTypeAsync(authkey, DDCCPoolType.EventWeapon, merger.GetLastItemMihoyoIdByType(DDCCPoolType.EventWeapon)))
            {
                res.Add(idgen.GenerateNextInternalId(item.time), ConvertToDDCLLogItem(item, DDCCPoolType.EventWeapon));
            }
            merger.Merge(res.Values.ToList());
        }
    }
}
