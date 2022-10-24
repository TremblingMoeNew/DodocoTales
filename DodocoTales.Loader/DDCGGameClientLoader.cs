using DodocoTales.Common.Enums;
using DodocoTales.Library;
using DodocoTales.Library.Enums;
using DodocoTales.Library.GameClient.Models;
using DodocoTales.Logs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DodocoTales.Loader
{
    public class DDCGGameClientLoader
    {
        readonly string output_log_cn = Environment.GetEnvironmentVariable("USERPROFILE") + @"/AppData/LocalLow/miHoYo/原神/output_log.txt";
        readonly string output_log_os = Environment.GetEnvironmentVariable("USERPROFILE") + @"/AppData/LocalLow/miHoYo/Genshin Impact/output_log.txt";
        readonly string game_path_pattern = @"Warmup file (.+)StreamingAssets.+";
        public readonly string GenshinData_CN = "YuanShen_Data";
        public readonly string GenshinData_OS = "GenshinImpact_Data";
        public readonly string ClientName_CN = "原神";
        public readonly string ClientName_OS = "Genshin Impact";

        public void LoadGameClientFromGameLog()
        {
            var ls = new List<DDCLGameClientItem>();
            ls.AddRange(LoadGameClientFromGameLogBase(output_log_cn));
            ls.AddRange(LoadGameClientFromGameLogBase(output_log_os));
            var exists = DDCL.GameClientLib.GetClients().Select(x => x.Path);
            ls.RemoveAll(x => exists.Contains(x.Path));
            DDCL.GameClientLib.AddClients(ls);
        }

        private List<DDCLGameClientItem> LoadGameClientFromGameLogBase(string output_log)
        {
            var items = new List<DDCLGameClientItem>();
            try
            {
                using(var stream = File.Open(output_log, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    StreamReader reader = new StreamReader(stream);
                    var log = reader.ReadToEnd();
                    var result = Regex.Match(log, game_path_pattern);
                    var genshin_data_dir = result.Groups[1].Value;

                    var item = new DDCLGameClientItem
                    {
                        Path = genshin_data_dir,
                        IsDefault = false,
                        TimeZone = DDCCTimeZone.DefaultUTCP8
                    };
                    
                    var info = new DirectoryInfo(genshin_data_dir);
                    if (info.Name == GenshinData_CN)
                    {
                        DDCLog.Info(DCLN.Loader, String.Format("CN client detected: {0}", genshin_data_dir));
                        item.Name = ClientName_CN;
                        item.ClientType = DDCLGameClientType.CN;
                    }
                    else if (info.Name == GenshinData_OS)
                    {
                        DDCLog.Info(DCLN.Loader, String.Format("Global client detected: {0}", genshin_data_dir));
                        item.Name = ClientName_OS;
                        item.ClientType = DDCLGameClientType.Global;
                    }
                    else
                    {
                        item = null;
                    }
                    if (item != null) items.Add(item);
                }
            }
            catch
            {

            }
            return items;
        }


        public readonly string ClientExecutable_CN = "YuanShen.exe";
        public readonly string ClientExecutable_OS = "GenshinImpact.exe";

        public DDCLGameClientItem LoadGameClientItemFromExecutablePath(string execpath)
        {
            FileInfo exec = new FileInfo(execpath);
            if(exec.Name == ClientExecutable_CN)
            {
                var genshin_data_dir = exec.DirectoryName + "/" + GenshinData_CN + "/";
                genshin_data_dir = genshin_data_dir.Replace('\\', '/');
                if (Directory.Exists(genshin_data_dir))
                {
                    DDCLog.Info(DCLN.Loader, String.Format("CN client confirmed: {0}", genshin_data_dir));
                    return new DDCLGameClientItem
                    {
                        Name = ClientName_CN,
                        ClientType = DDCLGameClientType.CN,
                        Path = genshin_data_dir,
                        TimeZone = DDCCTimeZone.DefaultUTCP8,
                        IsDefault = false
                    };
                }
            }
            else if (exec.Name == ClientExecutable_OS)
            {
                var genshin_data_dir = exec.DirectoryName + "/" + GenshinData_OS + "/";
                genshin_data_dir = genshin_data_dir.Replace('\\', '/');
                if (Directory.Exists(genshin_data_dir))
                {
                    DDCLog.Info(DCLN.Loader, String.Format("Global client confirmed: {0}", genshin_data_dir));
                    return new DDCLGameClientItem
                    {
                        Name = ClientName_OS,
                        ClientType = DDCLGameClientType.Global,
                        Path = genshin_data_dir,
                        TimeZone = DDCCTimeZone.DefaultUTCP8,
                        IsDefault = false
                    };
                }
            }
            return null;
        }


        readonly string WebCachePath = @"/webCaches/Cache/Cache_Data/data_2";
        readonly string authkey_pattern = @"1/0/\S+\?(\S+&game_biz=hk4e_(cn|global))";

        public string GetAuthkeyFromWebCache(DDCLGameClientItem client)
        {
            string authkey = null;
            string path = client.Path + WebCachePath;
            if (!File.Exists(path))
            {
                return null;
            }
            string target = Path.GetTempFileName();
            File.Copy(path,target,true);
            try
            {
                using (var stream = File.Open(target, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    StreamReader reader = new StreamReader(stream);
                    var log = reader.ReadToEnd();
                    var result = Regex.Matches(log, authkey_pattern);
                    if(result.Count > 0)
                    {
                        Regex regex = new Regex(@"lang=.+&device_type");
                        authkey = regex.Replace(result[result.Count - 1].Groups[1].Value, "lang=en&device_type");
                    }
                }
            }
            catch(Exception ex)
            {
                authkey =  null;
                Console.WriteLine(ex.Message);
            }
            File.Delete(target);
            return authkey;
        }

        public void RemoveCacheFile(DDCLGameClientItem client)
        {
            string path = client.Path + WebCachePath;
            try
            {
                File.Delete(path);
                DDCLog.Info(DCLN.Loader, String.Format("Remove cache success: {0}", path));
            }
            catch
            {
                DDCLog.Warning(DCLN.Loader, String.Format("Remove cache failed: {0}", path));
            }
            
        }
    }
}
