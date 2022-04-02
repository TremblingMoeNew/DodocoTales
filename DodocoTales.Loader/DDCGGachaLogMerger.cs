using DodocoTales.Common.Enums;
using DodocoTales.Library;
using DodocoTales.Library.BannerLibrary.Models;
using DodocoTales.Library.StoragedUser.Models;
using DodocoTales.Library.Utils;
using DodocoTales.Loader.Models;
using DodocoTales.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader
{
    public class DDCGGachaLogMerger
    {
        public SortedList<ulong, DDCLGachaLogItem> GachaLogSet;
        public HashSet<ulong> LoggedTimestamp;
        DDCLUserGachaLog UserLog;
        public DDCGGachaLogMerger(DDCLUserGachaLog userlog)
        {
            UserLog = userlog;
            LoggedTimestamp = new HashSet<ulong>();
            GachaLogSet = new SortedList<ulong, DDCLGachaLogItem>();
            foreach (var item in userlog.Logs)
            {
                GachaLogSet.Add(item.internal_id, item);
                LoggedTimestamp.Add(DDCL.ToUnixTimestamp(item.time));
            }
        }

        public ulong GetLastItemMihoyoIdByType(DDCCPoolType type)
        {
            var last = GachaLogSet.Values.LastOrDefault(x => x.pooltype == type);
            return last == null ? 0 : last.id;
        }

        public void MergeLogToDict(List<DDCLGachaLogItem>imported)
        {
            var conflicted = imported.FindAll(x => LoggedTimestamp.Contains(DDCL.ToUnixTimestamp(x.time)));
            var normal = imported.Except(conflicted);
            conflicted.RemoveAll(x => x.idlost); //对无id记录进行冲突判断过于困难，直接放弃
            DDCLInternalIdGenerator idgen = new DDCLInternalIdGenerator();
            foreach (var logs_at_a_time in conflicted.GroupBy(x => x.time))
            {
                var start = DDCLInternalIdGenerator.MinInternalId(logs_at_a_time.Key);
                var end = DDCLInternalIdGenerator.MaxInternalId(logs_at_a_time.Key);
                List<DDCLGachaLogItem> old = new List<DDCLGachaLogItem>();
                for(var i = start; i <= end; i++)
                {
                    if (GachaLogSet.ContainsKey(i)) old.Add(GachaLogSet[i]);
                }
                var old_idlost = old.FindAll(x => x.idlost); //对无id记录进行冲突判断过于困难，直接放弃
                foreach (var item in old_idlost) GachaLogSet.Remove(item.internal_id);
                var remains = old.Except(old_idlost);
                var res = new SortedList<ulong, DDCLGachaLogItem>();
                foreach (var item in remains) res.Add(item.id, item);
                foreach (var item in logs_at_a_time) if (!res.ContainsKey(item.id)) res.Add(item.id, item);
                foreach (var item in res.Values)
                {
                    item.internal_id = idgen.GenerateNextInternalId(item.time);
                    GachaLogSet[item.internal_id] = item;
                }
            }
            foreach(var item in normal)
            {
                item.internal_id = idgen.GenerateNextInternalId(item.time);
                GachaLogSet[item.internal_id] = item;
            }
        }

        public void RebuildClassifiers()
        {
            int versionidx = 0, versioncnt = DDCL.BannerLib.Versions.Count;
            var version = DDCL.BannerLib.Versions[versionidx];
            Dictionary<DDCCPoolType, int> banneridx = new Dictionary<DDCCPoolType, int>
            {
                { DDCCPoolType.Beginner, 0 },
                { DDCCPoolType.Permanent, 0 },
                { DDCCPoolType.EventCharacter, 0 },
                { DDCCPoolType.EventWeapon, 0 }
            };
            Dictionary<DDCCPoolType, ulong> round = new Dictionary<DDCCPoolType, ulong>
            {
                { DDCCPoolType.Beginner, 0 },
                { DDCCPoolType.Permanent, 0 },
                { DDCCPoolType.EventCharacter, 0 },
                { DDCCPoolType.EventWeapon, 0 }
            };
            Dictionary<DDCCPoolType, ulong> round_replacing = new Dictionary<DDCCPoolType, ulong>
            {
                { DDCCPoolType.Beginner, 0 },
                { DDCCPoolType.Permanent, 0 },
                { DDCCPoolType.EventCharacter, 0 },
                { DDCCPoolType.EventWeapon, 0 }
            };
            foreach (var item in GachaLogSet.Values)
            {
                if (round[item.pooltype] == 0) round[item.pooltype] = item.internal_id;
                if (item.round_id != round[item.pooltype])
                {
                    if (item.round_id != 0 && item.round_id != round_replacing[item.pooltype])
                    {
                        round_replacing[item.pooltype] = item.round_id;
                        foreach(var epitem in UserLog.EpitomizedPath.FindAll(x => x.round_id == item.round_id))
                        {
                            epitem.round_id = round[item.pooltype];
                        }
                    }
                    item.round_id = round[item.pooltype];
                    
                }
                if (item.rank == 5) round[item.pooltype] = 0;
                if (item.version_id == 0)
                {
                    while (version != null && CheckTimeWithVersion(version, item.time) < 0)
                    {
                        versionidx++;
                        if (versioncnt > versionidx) version = DDCL.BannerLib.Versions[versionidx]; else version = null;
                        banneridx[DDCCPoolType.Beginner] = 0;
                        banneridx[DDCCPoolType.Permanent] = 0;
                        banneridx[DDCCPoolType.EventCharacter] = 0;
                        banneridx[DDCCPoolType.EventWeapon] = 0;
                    }
                    if (version == null || CheckTimeWithVersion(version, item.time) > 0)
                    {
                        DDCLog.Error(DCLN.Loader, String.Format("Log item classification failed. {0}", JsonConvert.SerializeObject(item)));
                        continue;
                    }
                    item.version_id = version.id;
                    if (item.banner_id == 0)
                    {
                        var bannercnt = version.banners.Count;
                        var bidx = banneridx[item.pooltype];
                        while (bidx < bannercnt && (version.banners[bidx].type != item.pooltype 
                            || CheckTimeWithBanner(version.banners[bidx], item.time) < 0)) bidx++;
                        banneridx[item.pooltype] = bidx;
                        var banner = bidx < bannercnt ? version.banners[bidx] : null;
                        if (banner == null || CheckTimeWithBanner(banner, item.time) > 0)
                        {
                            DDCLog.Error(DCLN.Loader, String.Format("Log item classification failed. {0}", JsonConvert.SerializeObject(item)));
                            continue;
                        }
                        item.banner_id = banner.id;

                    }
                }
            }
        }
        public void WriteToUserlog()
        {
            RebuildClassifiers();
            UserLog.Logs = GachaLogSet.Values.ToList();
            DDCL.UserDataLib.SaveUserAsync(UserLog);
        }

        public void Merge(List<DDCLGachaLogItem> imported)
        {
            MergeLogToDict(imported);
            WriteToUserlog();
        }



        public int CheckTimeIsBetween(DateTimeOffset begin,DateTimeOffset end,DateTimeOffset time)
        {
            if (DateTimeOffset.Compare(end, time) < 0) return -1;
            else if (DateTimeOffset.Compare(begin, time) > 0) return 1;
            else return 0;
        }

        public int CheckTimeWithVersion(DDCLVersionInfo version,DateTime time)
        {
            var item = DDCL.GetTimeOffset(time, UserLog.zone);
            var version_begin = DDCL.GetSyncTimeOffset(version.beginTime);
            var version_end = DDCL.GetSyncTimeOffset(version.endTime);
            return CheckTimeIsBetween(version_begin, version_end, item);
        }
        public int CheckTimeWithBanner(DDCLBannerInfo banner,DateTime time)
        {
            var item = DDCL.GetTimeOffset(time, UserLog.zone);
            var banner_begin = GetBannerTimeOffset(banner.beginTime, banner.beginTimeSync);
            var banner_end = GetBannerTimeOffset(banner.endTime, banner.endTimeSync);
            return CheckTimeIsBetween(banner_begin, banner_end, item);
        }

        public DateTimeOffset GetBannerTimeOffset(DateTime time,bool sync)
        {
            if (sync) return DDCL.GetSyncTimeOffset(time);
            else return DDCL.GetTimeOffset(time, UserLog.zone);
        }
        


    }
}
