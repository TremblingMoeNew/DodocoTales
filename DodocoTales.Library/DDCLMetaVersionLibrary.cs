using DodocoTales.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DodocoTales.Library
{
    public class DDCLMetaVersionLibrary
    {
        public readonly string libPath = @"library/Version.json";
        DDCLMetaVersion model;
        public string ClientVersion { get { return model?.ClientVersion; } }
        public string BannerLibraryVersion { get { return model?.BannerLibraryVersion; } set { model.BannerLibraryVersion = value; SaveLibrary(); } }
        public string UnitLibraryVersion { get { return model?.UnitLibraryVersion; } set { model.UnitLibraryVersion = value; SaveLibrary(); } }

        public async Task<bool> LoadLibraryAsync()
        {
            try
            {
                var stream = File.Open(libPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var buffer = await reader.ReadToEndAsync();
                stream.Close();
                model = JsonConvert.DeserializeObject<DDCLMetaVersion>(buffer);
            }
            catch (Exception)
            {
                model = null;
                return false;
            }
            return true;
        }
        public bool SaveLibrary()
        {
            if (model == null) return false;
            try
            {
                var stream = File.Open(libPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter writer = new StreamWriter(stream);
                var serialized = JsonConvert.SerializeObject(model, Formatting.Indented);
                writer.Write(serialized);
                writer.Flush();
                stream.Close();
            }
            catch (Exception)
            {
                /// TODO: 报告保存错误？
                return false;
            }
            return true;
        }
        public long ConvertClientVerToInt64(string clientver)
        {
            if (clientver == null) return 0;
            var res = Regex.Match(clientver, @"^.*(\d+)\.(\d+)\.(\d+)\.{0,1}(\d*).*");
            if (res.Groups.Count < 4) return 0;
            long intver = 0;
            intver += Convert.ToInt32(res.Groups[1].Value);
            intver = intver * 100 + Convert.ToInt32(res.Groups[2].Value);
            intver = intver * 100 + Convert.ToInt32(res.Groups[3].Value);
            intver *= 1000000;
            var last = res.Groups[4].Value;
            if (last != "") 
            {
                intver += Convert.ToInt32(last);
            }
            return intver;
        }
        public long ConvertLibVerToInt64(string libver)
        {
            if (libver == null) return 0;
            var res = Regex.Match(libver, @"^.*(\d+)\.(\d+)\.(\d+)-.*(\d+)\.(\d+)\.(\d+)-(\d+).*");
            if (res.Groups.Count < 7) return 0;
            long intver = 0;
            for (int i = 1; i < 7; i++) 
            {
                intver = intver * 100 + Convert.ToInt32(res.Groups[i].Value);
            }
            intver = intver * 1000000 + Convert.ToInt32(res.Groups[7].Value);
            return intver;
        }
    }
}
