using DodocoTales.Library.StoragedUser.Models;
using DodocoTales.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DodocoTales.Library.StoragedUser
{
    public class DDCLUserDataLibrary
    {
        public readonly string UserDataDirPath = "userdata";
        public readonly string UserDataFileSearchPattern = "userlog_*.json";
        public readonly string UserDataFileRegexPattern = @"userlog_(\d+)\.json";
        public readonly string UserDataFileOpenPattern = "userdata/userlog_{0}.json";

        public Dictionary<ulong, DDCLUserGachaLog> U { get; set; }

        public DDCLUserDataLibrary()
        {
            U = new Dictionary<ulong, DDCLUserGachaLog>();
        }
        public DDCLUserGachaLog createEmptyLocalGachaLog(ulong uid)
        {
            return new DDCLUserGachaLog { uid = uid, V = new List<DDCLStoragedVersionLogs>() };
        }
        public void addEmptyUser(ulong uid)
        {
            if (U.ContainsKey(uid)) return;
            var log = createEmptyLocalGachaLog(uid);
            U.Add(uid, log);
            // TODO: Signal
            DDCLog.Info(DCLN.Lib, String.Format("New user added. UID:{0}", uid));
        }
        public async Task loadLocalGachaLogByUidAsync(ulong uid)
        {
            string logfile = String.Format(UserDataFileOpenPattern, uid);
            DDCLog.Info(DCLN.Lib, String.Format("Loading Userdata file: {0}", logfile));
            try
            {
                var stream = File.Open(logfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var response = JsonConvert.DeserializeObject<DDCLUserGachaLog>(await reader.ReadToEndAsync());
                if (U.ContainsKey(response.uid))
                {
                    // TODO: Signal
                    DDCLog.Warning(DCLN.Lib, String.Format("Userdata conflict. UID:{0}", response.uid));
                    return;
                }
                if (uid != response.uid)
                {
                    DDCLog.Warning(DCLN.Lib, String.Format("{0}, UID:{1}", logfile, response.uid));
                }
                U.Add(response.uid, response);
                DDCLog.Info(DCLN.Lib, String.Format("Userdata successfully loaded. UID:{0}", response.uid));

            }
            catch (Exception e)
            {
                DDCLog.Error(DCLN.Lib, String.Format("Failed to load userdata. UID:{0}", uid), e);
                // TODO: Signal
            }
        }
        public async Task loadLocalGachaLogsAsync()
        {
            DDCLog.Info(DCLN.Lib, "Loading userdata...");
            DirectoryInfo dir = new DirectoryInfo(UserDataDirPath);
            if (!dir.Exists) dir.Create();
            var files = dir.GetFiles(UserDataFileSearchPattern);
            List<Task> taskQuery = new List<Task>();
            foreach (var f in files)
            {
                var result = Regex.Match(f.Name, UserDataFileRegexPattern);
                ulong uid = 0;
                try
                {
                    uid = Convert.ToUInt64(result.Groups[1].Value);
                }
                catch (Exception e)
                {
                }
                taskQuery.Add(loadLocalGachaLogByUidAsync(uid));
            }
            await Task.WhenAll(taskQuery);
            // TODO: Signal
            DDCLog.Info(DCLN.Lib, "Userdata successfully loaded.");
        }
        public async Task saveUserAsync(DDCLUserGachaLog userlog)
        {
            if (userlog == null) return;
            string logfile = String.Format("userdata/userlog_{0}.json", userlog.uid);
            // TODO: LibVersion Update
            try
            {
                var stream = File.Open(logfile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter writer = new StreamWriter(stream);
                var serialized = JsonConvert.SerializeObject(userlog, Formatting.Indented);
                await writer.WriteAsync(serialized);
                await writer.FlushAsync();
                stream.Close();
                DDCLog.Info(DCLN.Lib, String.Format("Userdata successfully saved. UID:{0}", userlog.uid));
            }
            catch (Exception e)
            {
                // TODO: Signal
                DDCLog.Error(DCLN.Lib, String.Format("Failed to save userdata. UID:{0}", userlog.uid), e);
            }

        }

    }
}
