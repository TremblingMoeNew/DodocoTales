using DodocoTales.Common;
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

        public Dictionary<long, DDCLUserGachaLog> U { get; set; }

        public DDCLUserDataLibrary()
        {
            U = new Dictionary<long, DDCLUserGachaLog>();
        }
        public DDCLUserGachaLog CreateEmptyLocalGachaLog(long uid)
        {
            return new DDCLUserGachaLog { uid = uid,Logs=new List<DDCLGachaLogItem>(),EpitomizedPath=new List<DDCLEpitomizedPathItem>() };
        }
        public void TryAddEmptyUser(long uid)
        {
            if (U.ContainsKey(uid)) return;
            var log = CreateEmptyLocalGachaLog(uid);
            U.Add(uid, log);
            DDCLog.Info(DCLN.Lib, String.Format("New user added. UID:{0}", uid));
        }
        public async Task LoadLocalGachaLogByUidAsync(long uid)
        {
            string logfile = String.Format(UserDataFileOpenPattern, uid);
            DDCLog.Info(DCLN.Lib, String.Format("Loading userlog file: {0}", logfile));
            try
            {
                var stream = File.Open(logfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var response = JsonConvert.DeserializeObject<DDCLUserGachaLog>(await reader.ReadToEndAsync());
                if (U.ContainsKey(response.uid))
                {
                    DDCS.Emit_UserLibUidDeplicated(response.uid);
                    DDCLog.Warning(DCLN.Lib, String.Format("Userlog deplicated. UID:{0}", response.uid));
                    return;
                }
                if (uid != response.uid)
                {
                    DDCLog.Warning(DCLN.Lib, String.Format("{0}, UID:{1}", logfile, response.uid));
                }
                if (response.Logs == null)
                {
                    DDCLog.Info(DCLN.Lib, String.Format("V0-Style Userlog found, UID:{0}",response.uid));
                    DDCS.Emit_V0StyleLogFileDetacted(response.uid);
                    return;
                }
                U.Add(response.uid, response);
                DDCLog.Info(DCLN.Lib, String.Format("Userlog successfully loaded. UID:{0}", response.uid));

            }
            catch (Exception e)
            {
                DDCLog.Error(DCLN.Lib, String.Format("Failed to load userlog. UID:{0}", uid), e);
            }
        }
        public async Task LoadLocalGachaLogsAsync()
        {
            DDCLog.Info(DCLN.Lib, "Loading userdata...");
            DirectoryInfo dir = new DirectoryInfo(UserDataDirPath);
            if (!dir.Exists) dir.Create();
            var files = dir.GetFiles(UserDataFileSearchPattern);
            List<Task> taskQuery = new List<Task>();
            foreach (var f in files)
            {
                var result = Regex.Match(f.Name, UserDataFileRegexPattern);
                long uid = 0;
                try
                {
                    uid = Convert.ToInt64(result.Groups[1].Value);
                }
                catch (Exception e)
                {
                }
                taskQuery.Add(LoadLocalGachaLogByUidAsync(uid));
            }
            await Task.WhenAll(taskQuery);
            DDCS.Emit_UserLibReloadCompleted();
            DDCLog.Info(DCLN.Lib, "Userdata successfully loaded.");
        }
        public async Task SaveUserAsync(DDCLUserGachaLog userlog)
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
                DDCS.Emit_UserlogSaveCompleted();
                DDCLog.Info(DCLN.Lib, String.Format("Userlog successfully saved. UID:{0}", userlog.uid));
            }
            catch (Exception e)
            {
                DDCS.Emit_UserlogSaveFailed();
                DDCLog.Error(DCLN.Lib, String.Format("Failed to save userlog. UID:{0}", userlog.uid), e);
            }

        }

        public DDCLUserGachaLog GetUserLogByUid(long uid)
        {
            TryAddEmptyUser(uid);
            return U[uid];
        }
    }
}
