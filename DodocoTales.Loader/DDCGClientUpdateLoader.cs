using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Net.Http;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace DodocoTales.Loader
{
    public class DDCGClientUpdateLoader
    {
        public static readonly string CDN = "https://fastly.jsdelivr.net/gh/TremblingMoeNew/DodocoTales-meta@main/";
        public static readonly string UpdateFileWebPath = "Bin/DodocoTales-v{0}";
        public static readonly string UpdateStoragePath = "update/";
        public static readonly string UpdateFilePath = "update/DodocoTales.zip";
        public static readonly string UpdateExtract = "update/DodocoTales/";
        public static readonly string Updater = "DodocoTales.AutoUpdate.exe";

        public bool ExtractClient()
        {
            //if (!Directory.Exists(UpdateStoragePath)) Directory.CreateDirectory(UpdateStoragePath);
            try
            {
                ZipFile.ExtractToDirectory(UpdateFilePath, UpdateExtract);
                //File.Delete(UpdateExtract);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DownloadClient(string version)
        {
            var url = CDN + String.Format(UpdateFileWebPath,version);
            var client = new HttpClient();
            CacheControlHeaderValue cacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true
            };
            client.DefaultRequestHeaders.CacheControl = cacheControl;
            try
            {
                var buf = await client.GetByteArrayAsync(url);
                if (!Directory.Exists(UpdateStoragePath)) Directory.CreateDirectory(UpdateStoragePath);
                File.WriteAllBytes(UpdateFilePath, buf);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }
        public bool MoveUpdater()
        {
            if (!File.Exists(UpdateExtract + Updater)) return false;
            try
            {
                if (File.Exists(Updater)) File.Delete(Updater);
                File.Move(UpdateExtract + Updater, Updater);
            }
            catch(Exception)
            {
                return false;
            }
            
            return true;
        }
        public void ExecUpdater()
        {
            Process.Start(Updater);
        }
    }
}
