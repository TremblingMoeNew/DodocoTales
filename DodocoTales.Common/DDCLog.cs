using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Logs
{
    public static partial class DDCLog
    {
        public static string ClientVersion;
        public static string LibrarydVersion;
        public static string CommonVersion;
        public static string LoaderVersion;

        public static string ClientVersionFull;
        public static string LibrarydVersionFull;
        public static string CommonVersionFull;
        public static string LoaderVersionFull;

        public static readonly string LogPath = "Logs";
        public static readonly string LogFilePattern = "DodocoTales_Log_{0:yyyy-MM-dd HH-mm-ss}.txt";
        public static readonly string LogFilePath;
        public static readonly string LogPattern = "[{1:G}][{0}/{2}]{3}";
        static DDCLog()
        {
            LogFilePath = LogPath + '/' + String.Format(LogFilePattern, DateTime.Now);
            DirectoryInfo dir = new DirectoryInfo(LogPath);
            if (!dir.Exists) dir.Create();
        }

        public static void InitHint()
        {
            var Entry = System.Reflection.Assembly.GetEntryAssembly();
            string Content =
                "DodocoTales.Log initialized." + Environment.NewLine
                + String.Format("DodocoTales Version {0}", Entry.GetName().Version) + Environment.NewLine;
            
            foreach(var dllname in Entry.GetReferencedAssemblies().ToList().FindAll(x => x.Name.Contains("DodocoTales")))
            {
                Content += String.Format("  {0} Version {1}", dllname.Name, dllname.Version) + Environment.NewLine;
            }
            Log(DDCLogType.Info, DCLN.Log, Content);
        }

        public static void Log(DDCLogType Type, DCLN Namespace, string Content)
        {
            var text = String.Format(LogPattern, Type, DateTime.Now, Namespace, Content);
            using (FileStream fs = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLineAsync(text);
                }
            }

        }
        public static void Info(DCLN Namespace,string info)
        {
            Log(DDCLogType.Info, Namespace, info);
        }
        public static void Warning(DCLN Namespace,string info)
        {
            Log(DDCLogType.Warning, Namespace, info);
        }
        public static void Warning(DCLN Namespace, string info, Exception ex)
        {
            Log(DDCLogType.Warning, Namespace, info + Environment.NewLine + ex.StackTrace);
        }
        public static void Error(DCLN Namespace, string info)
        {
            Log(DDCLogType.Error, Namespace, info);
        }
        public static void Error(DCLN Namespace, string info, Exception ex)
        {
            Log(DDCLogType.Error, Namespace, info + Environment.NewLine + ex.StackTrace);
        }
    }
}
