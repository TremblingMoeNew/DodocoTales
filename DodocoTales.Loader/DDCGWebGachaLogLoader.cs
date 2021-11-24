using DodocoTales.Common.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DodocoTales.Loader.Models;
using Newtonsoft.Json;
using System.Threading;

namespace DodocoTales.Loader
{
    public partial class DDCGWebGachaLogLoader
    {
        readonly string locallow = Environment.GetEnvironmentVariable("USERPROFILE") + @"/AppData/LocalLow/miHoYo";

        public bool isCnApi = true;
        string apipattern { get { if (isCnApi) return apipattern_cn; else return apipattern_os; } }
        readonly string apipattern_cn = @"https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog?{0}&gacha_type={1}&page={2}&size={4}&end_id={3}";
        readonly string apipattern_os = @"https://hk4e-api-os.mihoyo.com/event/gacha_info/api/getGachaLog?{0}&gacha_type={1}&page={2}&size={4}&end_id={3}";

        HttpClient client;

        public DDCGWebGachaLogLoader()
        {
            client = new HttpClient();
        }

        public string getAuthKey()
        {
            string output_file,log;
            if (Directory.Exists(locallow + @"/原神"))
            {
                output_file = locallow + @"/原神/output_log.txt";
            }
            else
            {
                output_file = locallow + @"/Genshin Impact/output_log.txt";
            }
            try
            {
                var stream = File.Open(output_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                log = reader.ReadToEnd();
            }
            catch(Exception)
            {
                return null;
            }
            string pattern = @"OnGetWebViewPageFinish:.+\?(.+)#/log";
            var result = Regex.Matches(log, pattern);
            if (result.Count == 0) return null;
            else
            {
                Regex regex = new Regex(@"lang=.+&device_type");
                return regex.Replace(result[result.Count - 1].Groups[1].Value, "lang=en&device_type");
            }
        }

        public async Task<long>tryConnectAndGetUid(string authkey)
        {
            isCnApi = true;
            var uid = await getUidFromWeb(authkey);
            if (uid < 0)
            {
                isCnApi = false;
                uid = await getUidFromWeb(authkey);
            }
            if (uid == -1) uid = getUidFromLocal();
            return uid;
        }

        public async Task<long>getUidFromWeb(string authkey)
        {
            var bl = await GetGachaLogAsync(authkey, 1, DDCCPoolType.Beginner, 0);
            if (bl == null) return -2;
            bl.AddRange(await GetGachaLogAsync(authkey, 1, DDCCPoolType.Permanent, 0));
            if(bl.Count==0)
            {
                bl.AddRange(await GetGachaLogAsync(authkey, 1, DDCCPoolType.EventCharacter, 0));
                bl.AddRange(await GetGachaLogAsync(authkey, 1, DDCCPoolType.EventWeapon, 0));
            }
            if (bl.Count == 0)
                return -1;
            else
                return bl[0].uid;
        }
        
        public long getUidFromLocal()
        {
            string output_file, log;
            long res = -1;
            if (Directory.Exists(locallow + @"/原神"))
            {
                output_file = locallow + @"/原神/UidInfo.txt";
            }
            else
            {
                output_file = locallow + @"/Genshin Impact/UidInfo.txt";
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

        public async Task<List<DDCGGachaLogResponseItem>> GetGachaLogAsync(string authkey,int pageid, DDCCPoolType type, ulong lastid, int size = 6)
        {
            var api = String.Format(
                apipattern,
                authkey, (int)type, pageid, lastid, size
            );
            Thread.Sleep(200);

            var stringresponse = await client.GetStringAsync(api);
            var response = JsonConvert.DeserializeObject<DDCGGachaLogResponse>(stringresponse);
            if (response.retcode != 0)
            {
                if (response.retcode == -110)
                {
                    Console.WriteLine("-110 visit too frequently");
                    Thread.Sleep(1000);
                    return await GetGachaLogAsync(authkey, pageid, type, lastid, size);
                }
                return null;
            }
            return response.data.list;
            
        }
        public async Task GetGachaLogsByTypeAsync(string authkey, List<DDCGGachaLogResponseItem> res,DDCCPoolType type, ulong endid, int size=6)
        {
            ulong lastid = 0;
            bool run = true;
            for (int i = 1; run; i++)
            {
                var list = await GetGachaLogAsync(authkey,i, type, lastid, size);
                foreach (var item in list)
                {
                    lastid = item.id;
                    if (lastid == endid)
                    {
                        run = false;
                        break;
                    }
                    /// TODO: 时区修正
                    res.Add(item);
                }
                if (list.Count < size) run = false;
            }
            res.Reverse();
        }

        
        public async Task<DDCGGachaInitialLogs> GetGachaLogsAsync(string authkey,int size=6)
        {
            DDCGGachaInitialLogs logs = new DDCGGachaInitialLogs()
            {
                Beginner = new List<DDCGGachaLogResponseItem>(),
                Permanent = new List<DDCGGachaLogResponseItem>(),
                EventCharacter = new List<DDCGGachaLogResponseItem>(),
                EventWeapon = new List<DDCGGachaLogResponseItem>()
            };
            Console.WriteLine(logs);
            //List<Task> taskQuery = new List<Task>
            //{
            await GetGachaLogsByTypeAsync(authkey, logs.Beginner, DDCCPoolType.Beginner, DDCG.LogMerger.getLastLogId(DDCCPoolType.Beginner), size);
            await GetGachaLogsByTypeAsync(authkey, logs.Permanent, DDCCPoolType.Permanent, DDCG.LogMerger.getLastLogId(DDCCPoolType.Permanent), size);
            await GetGachaLogsByTypeAsync(authkey, logs.EventCharacter, DDCCPoolType.EventCharacter, DDCG.LogMerger.getLastLogId(DDCCPoolType.EventCharacter), size);
            await GetGachaLogsByTypeAsync(authkey, logs.EventWeapon, DDCCPoolType.EventWeapon, DDCG.LogMerger.getLastLogId(DDCCPoolType.EventWeapon), size);
            //};
            //await Task.WhenAll(taskQuery);

            
            return logs;
        }
    }
}
