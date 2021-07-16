using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Library.Models;
using Newtonsoft.Json;

namespace DodocoTales.Library
{
    public partial class DDCLBannerLibrary
    {
        public readonly string libPath = @"meta/BannerLibrary.json";
        DDCLBannerLibModel model;

        public List<DDCLBannerInfo> beginnerPools { get { return model?.beginnerPools; } }
        public List<DDCLBannerInfo> permanentPools { get { return model?.permanentPools; } }
        public List<DDCLVersionInfo> eventPools { get { return model?.eventPools; } }

        public bool loadLibrary()
        {
            try
            {
                var stream = File.Open(libPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var buffer = reader.ReadToEnd();
                model = JsonConvert.DeserializeObject<DDCLBannerLibModel>(buffer);
            }
            catch(Exception)
            {
                model = null;
                return false;
            }
            return true;
        }

    }
}
