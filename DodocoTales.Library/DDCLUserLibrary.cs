﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DodocoTales.Common.Models;
using Newtonsoft.Json;

namespace DodocoTales.Library
{
    public partial class DDCLUserLibrary
    {
        public long CurrentUserUID { get; set; }
        public Dictionary<long, DDCCUserGachaLogs> U { get; set; }

        public DDCCUserGachaLogs createEmptyLocalGachaLog(long uid)
        {
            return new DDCCUserGachaLogs { uid = uid, V = new List<DDCCVersionLogs>() };
        }
        public async Task loadLocalGachaLogByUidAsync(long uid)
        {
            string logfile = String.Format("userdata/userlog_{0}.json", uid);
            try
            {
                var stream = File.Open(logfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var response = JsonConvert.DeserializeObject<DDCCUserGachaLogs>(await reader.ReadToEndAsync());
                /// TODO: 检查版本
                U.Add(response.uid, response);
            }
            catch (Exception)
            {
                /// TODO: 报告载入错误
            }
        }
        public async Task loadLocalGachaLogs()
        {
            DirectoryInfo dir = new DirectoryInfo("userdata");
            var files = dir.GetFiles("");
            string pattern = @"userlog_(\d+)\.json";
            List<Task> taskQuery = new List<Task>();
            foreach (var f in files)
            {
                var result = Regex.Match(f.Name, pattern);
                long uid=0;
                try
                {
                    uid = Convert.ToInt64(result.Groups[1]);
                }
                catch(Exception)
                {
                    /// TODO: 处理与报告载入错误
                }
                taskQuery.Add(loadLocalGachaLogByUidAsync(uid));
            }
            await Task.WhenAll(taskQuery);
            
            /// TODO: 返回载入结果？
        }

        public async Task saveCurrentUser()
        {
            string logfile = String.Format("userdata/userlog_{0}.json", CurrentUserUID);
            var curuser = getCurrentUser();
            if (curuser == null) return; 
            /// TODO：更新库版本
            try
            {
                var stream = File.Open(logfile, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter writer = new StreamWriter(stream);
                var serialized = JsonConvert.SerializeObject(curuser, Formatting.Indented);
                await writer.WriteLineAsync(serialized);
            }
            catch (Exception)
            {
                /// TODO: 报告保存错误？
            }
        }
        public DDCCUserGachaLogs getCurrentUser()
        {
            U.TryGetValue(CurrentUserUID, out DDCCUserGachaLogs user);
            return user;
        }
        public async Task swapUser(long uid)
        {
            await saveCurrentUser();
            CurrentUserUID = uid;
            if (!U.ContainsKey(CurrentUserUID))
            {
                createEmptyLocalGachaLog(CurrentUserUID);
            }
        }


    }
}
