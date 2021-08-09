using DodocoTales.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader
{
    public class DDCGMetaLoader
    {
        HttpClient client;

        public readonly string CDN = "https://cdn.jsdelivr.net/gh/TremblingMoeNew/DodocoTales-meta@main/";
        public readonly string localLibPath = "library/";
        public readonly string VersionLib = "Version.json";
        public readonly string BannerLib = "BannerLibrary.json";
        public readonly string UnitLib = "UnitLibrary.json";
        public DateTimeOffset LastUpdateOn;
        
        public DDCGMetaLoader()
        {
            client = new HttpClient();
            LastUpdateOn = DateTimeOffset.MinValue;
        }

        public bool IsUpdateExpired()
        {
            var expireat = LastUpdateOn.AddHours(6);
            return DateTimeOffset.Compare(expireat, DateTimeOffset.Now) < 0;
        }

        public void MarkUpdate()
        {
            LastUpdateOn = DateTimeOffset.Now;
        }

        public async Task<string> GetMetadataFile(string filename)
        {
            string url = CDN + filename;
            try
            {
                return await client.GetStringAsync(url);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<DDCLMetaVersion>FetchNewestVersionData()
        {
            try
            {
                var response = JsonConvert.DeserializeObject<DDCLMetaVersion>(await GetMetadataFile(VersionLib));
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateBannerLib()
        {
            try
            {
                var lib = await GetMetadataFile(BannerLib);
                var libPath = localLibPath + BannerLib;
                var stream = File.Open(libPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter writer = new StreamWriter(stream);
                await writer.WriteAsync(lib);
                await writer.FlushAsync();
                stream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateUnitLib()
        {
            try
            {
                var lib = await GetMetadataFile(UnitLib);
                var libPath = localLibPath + UnitLib;
                var stream = File.Open(libPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter writer = new StreamWriter(stream);
                await writer.WriteAsync(lib);
                await writer.FlushAsync();
                stream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
