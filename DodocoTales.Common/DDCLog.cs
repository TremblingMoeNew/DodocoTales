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
            string Content =
                "DodocoTales.Log initialized." + Environment.NewLine
                + String.Format("DodocoTales Version {0} ({1})", "1.0.0", "1.0.0.000000") + Environment.NewLine;
            // TODO
            Log(DDCLogType.Info, DCLN.Log, Content);
        }

        public static void Log(DDCLogType Type, DCLN Namespace, string Content)
        {
            var text = String.Format(LogPattern, Type, DateTime.Now, Namespace, Content);
            using (StreamWriter writer = new StreamWriter(LogFilePath,true))
            {
                writer.WriteLine(text);
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
