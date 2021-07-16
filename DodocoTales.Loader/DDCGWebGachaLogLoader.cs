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

namespace DodocoTales.Loader
{
    public partial class DDCGWebGachaLogLoader
    {
        readonly string locallow = Environment.GetEnvironmentVariable("USERPROFILE") + @"/AppData/LocalLow/miHoYo";

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
            string pattern = @"OnGetWebViewPageFinish:https://webstatic.mihoyo.com/hk4e/event/e20190909gacha/index.html\?(.+)#/log";
            var result = Regex.Matches(log, pattern);
            if (result.Count == 0) return null;
            else
            {
                Regex regex = new Regex(@"lang=.+&device_type");
                return regex.Replace(result[result.Count - 1].Groups[1].Value, "lang=en&device_type");
            }
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

        public async Task<List<DDCGGachaLogResponseItem>> GetGachaLogAsync(int pageid, DDCCPoolType type, ulong lastid, int size = 6)
        {
            var attr = getAuthKey();
            var api = String.Format(
                @"https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog?{0}&gacha_type={1}&page={2}&size={4}&end_id={3}",
                attr, (int)type, pageid, lastid, size
            );
            try
            {
                var response = JsonConvert.DeserializeObject<DDCGGachaLogResponse>(await client.GetStringAsync(api));
                if (response.retcode != 0) return null;
                return response.data.list;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task GetGachaLogsByTypeAsync(List<DDCGGachaLogResponseItem> res,DDCCPoolType type, ulong endid, int size=6)
        {
            ulong lastid = 0;
            bool run = true;
            for (int i = 1; run; i++)
            {
                var list = await GetGachaLogAsync(i, type, lastid, size);
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
        }
        public async Task<DDCGGachaInitialLogs> GetGachaLogsAsync()
        {
            // TODO: DDCGLocalGachaLogLoader: 载入
            ulong endid = 0;
            int size = 0;
            // Temp: endid,size

            DDCGGachaInitialLogs logs = new DDCGGachaInitialLogs()
            {
                Beginner = new List<DDCGGachaLogResponseItem>(),
                Permanent = new List<DDCGGachaLogResponseItem>(),
                EventCharacter = new List<DDCGGachaLogResponseItem>(),
                EventWeapon = new List<DDCGGachaLogResponseItem>()
            };
            List<Task> taskQuery = new List<Task>
            {
                GetGachaLogsByTypeAsync(logs.Beginner,DDCCPoolType.Beginner,endid,size),
                GetGachaLogsByTypeAsync(logs.Permanent,DDCCPoolType.Permanent,endid,size),
                GetGachaLogsByTypeAsync(logs.EventCharacter,DDCCPoolType.EventCharacter,endid,size),
                GetGachaLogsByTypeAsync(logs.EventWeapon,DDCCPoolType.EventWeapon,endid,size)
            };
            await Task.WhenAll(taskQuery);
            return logs;
        }
    }
}
