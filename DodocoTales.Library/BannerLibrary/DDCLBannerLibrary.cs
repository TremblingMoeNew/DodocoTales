using DodocoTales.Common;
using DodocoTales.Common.Enums;
using DodocoTales.Library.BannerLibrary.Models;
using DodocoTales.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.BannerLibrary
{
    public class DDCLBannerLibrary
    {
        public readonly string libPath = @"library/BannerLibrary.json";
        DDCLBannerLibModel model;

        public List<DDCLVersionInfo> Versions { get; set; }
        public List<DDCLBannerInfo> Banners { get; set; }

        public DDCLBannerLibrary()
        {
            Versions = new List<DDCLVersionInfo>();
            Banners = new List<DDCLBannerInfo>();
        }

        public DDCLVersionInfo GetVersion(ulong versionid)
            => Versions.Find(x => x.id == versionid);

        public DDCLBannerInfo GetBanner(ulong versionid, ulong bannerid)
            => GetBanner(ConvertToInternalBannerId(versionid, bannerid));
        public DDCLBannerInfo GetBanner(ulong internalbannerid)
            => Banners.Find(x => x.InternalId == internalbannerid);

        public List<DDCLBannerInfo> GetBannersByType(DDCCPoolType type)
            => Banners.FindAll(x => x.type == type);
        public List<DDCLBannerInfo> GetBannersByR5Up(ulong unitclass)
            => Banners.FindAll(x => x.rank5Up.Contains(unitclass));
        public List<DDCLBannerInfo> GetBannersByR4Up(ulong unitclass)
            => Banners.FindAll(x => x.rank4Up.Contains(unitclass));

        public ulong ConvertToInternalBannerId(ulong versionid, ulong bannerid)
            => versionid * 10000000 + bannerid;


        public async Task<bool> LoadModelAsync()
        {
            try
            {
                var stream = File.Open(libPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var buffer = await reader.ReadToEndAsync();
                model = JsonConvert.DeserializeObject<DDCLBannerLibModel>(buffer);
            }
            catch (Exception)
            {
                model = null;
                DDCLog.Error(DCLN.Lib, "Failed to load bannerlib.");
                DDCS.Emit_BannerLibReloadFailed();
                return false;
            }
            DDCLog.Info(DCLN.Lib, "Bannerlib deserialized.");
            DDCS.Emit_BannerLibDeserialized();
            return true;
        }

        public bool RebuildLibrary()
        {
            if (model == null)
            {
                return false;
            }
            var beginners = model.beginnerPools;
            var permanents = model.permanentPools;
            int bpidx = 0, ppidx = 0;
            int bplen = beginners.Count, pplen = permanents.Count;
            foreach (var version in model.eventPools)
            {
                Versions.Add(version);
                while (ppidx < pplen && DateTime.Compare(permanents[ppidx].endTime, version.beginTime) < 0) ppidx++;
                while (ppidx < pplen && DateTime.Compare(permanents[ppidx].beginTime, version.endTime) < 0) 
                {
                    var new_permanenent = permanents[ppidx].Copy();
                    if(DateTime.Compare(permanents[ppidx].beginTime, version.beginTime) < 0)
                    {
                        new_permanenent.beginTime = version.beginTime;
                        new_permanenent.beginTimeSync = true;
                    }
                    if (DateTime.Compare(permanents[ppidx].endTime, version.endTime) > 0)
                    {
                        new_permanenent.endTime = version.endTime;
                        new_permanenent.endTimeSync = true;
                    }
                    version.banners.Insert(0, new_permanenent);
                    ppidx++;
                }
                ppidx--;
                while (bpidx < bplen && DateTime.Compare(beginners[ppidx].endTime, version.beginTime) < 0) bpidx++;
                while (bpidx < bplen && DateTime.Compare(beginners[bpidx].beginTime, version.endTime) < 0)
                {
                    var new_beginner = beginners[bpidx].Copy();
                    if (DateTime.Compare(beginners[bpidx].beginTime, version.beginTime) < 0)
                    {
                        new_beginner.beginTime = version.beginTime;
                        new_beginner.beginTimeSync = true;
                    }
                    if (DateTime.Compare(beginners[bpidx].endTime, version.endTime) > 0)
                    {
                        new_beginner.endTime = version.endTime;
                        new_beginner.endTimeSync = true;
                    }
                    version.banners.Insert(0, new_beginner);
                    bpidx++;
                }
                bpidx--;

                foreach(var banner in version.banners)
                {
                    banner.InternalId = ConvertToInternalBannerId(version.id, banner.id);
                    banner.VersionId = version.id;
                    Banners.Add(banner);
                }
            }

            return true;
        }
        public async Task<bool> LoadLibraryAsync()
        {
            if(!await LoadModelAsync())
            {
                return false;
            }
            RebuildLibrary();
            DDCLog.Info(DCLN.Lib, "BannerLib successfully loaded.");
            DDCS.Emit_BannerLibReloadCompleted();
            return true;
        }
    }
}
