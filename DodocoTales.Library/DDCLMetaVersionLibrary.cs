using DodocoTales.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    }
}
