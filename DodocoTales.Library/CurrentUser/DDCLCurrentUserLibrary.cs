using DodocoTales.Common.Enums;
using DodocoTales.Library.CurrentUser.Models;
using DodocoTales.Library.StoragedUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.CurrentUser
{
    public class DDCLCurrentUserLibrary
    {
        public DDCLUserGachaLog OriginalLogs { get; set; }

        public List<DDCLGachaLogItem> Logs { get; set; }
        public List<DDCLRoundLogItem> BasicRounds { get; set; }
        public List<DDCLRoundLogItem> GreaterRounds { get; set; }
        public List<DDCLBannerLogItem> Banners { get; set; }


        // 查找
        // Banners
        public DDCLBannerLogItem GetBanner(int id)
            => Banners.Find(x => x.BannerId == id);
        public int GetBannerIndex(int id)
            => Banners.FindIndex(x => x.BannerId == id);
        public List<DDCLBannerLogItem> GetBannersByVersion(int versionid)
            => Banners.FindAll(x => x.VersionId == versionid);
        public List<DDCLBannerLogItem> GetBannersByCategorizedType(DDCCPoolType type)
            => Banners.FindAll(x => x.CategorizedGachaType == type);
        public List<DDCLBannerLogItem> GetBannersByVersionAndType(int versionid, DDCCPoolType type)
            => Banners.FindAll(x => x.VersionId == versionid && x.CategorizedGachaType == type);
        // GreaterRounds
        // BasicRounds
        // Logs
        public DDCLGachaLogItem GetItem(ulong id)
            => Logs.Find(x => x.StoragedId == id);
        public int GetItemIndex(ulong id)
            => Logs.FindIndex(x => x.StoragedId == id);
        public int GetLastRank4Distance(ulong id)
        {
            int idx = GetItemIndex(id);
            DDCLGachaLogItem startitem = Logs[idx];
            int dis = 0;
            for(int i = idx - 1; i >= 0; i--)
            {
                if(Logs[i].CategorizedGachaType==startitem.CategorizedGachaType)
                {
                    dis++;
                    if (startitem.Rank == 4) break;
                }
            }
            return dis;
        }
        public int GetLastRank4Distance(ulong id, DDCCUnitType type)
        {
            int idx = GetItemIndex(id);
            DDCLGachaLogItem startitem = Logs[idx];
            int dis = 0;
            for (int i = idx - 1; i >= 0; i--)
            {
                if (Logs[i].CategorizedGachaType == startitem.CategorizedGachaType)
                {
                    dis++;
                    if (Logs[i].Rank == 4 && Logs[i].UnitType == type) break;
                }
            }
            return dis;
        }
        public int GetLastRank4UpDistance(ulong id)
        {
            int idx = GetItemIndex(id);
            DDCLGachaLogItem startitem = Logs[idx];
            int dis = 0;
            for (int i = idx - 1; i >= 0; i--)
            {
                if (Logs[i].CategorizedGachaType == startitem.CategorizedGachaType)
                {
                    dis++;
                    if (Logs[i].Rank == 4)
                    {
                        if (DDCL.BannerLib.GetBanner(Logs[i].BannerId)?.rank4Up.Exists(x => x == Logs[i].UnitClass) ?? false) break;
                    }
                }
            }
            return dis;
        }


        public bool RebuildBasicLibrary()  // TODO
        {
            // TODO: Log
            if (OriginalLogs == null)
            {
                // TODO: Log
                // TODO: Signal
                return false;
            }

            Banners.Clear();
            BasicRounds.Clear();
            Logs.Clear();

            foreach (var vlog in OriginalLogs.V)
            {
                foreach(var blog in vlog.B)
                {
                    DDCLBannerLogItem bannerlog = new DDCLBannerLogItem
                    {
                        VersionId = vlog.id,
                        BannerId = blog.id,
                        CategorizedGachaType = blog.poolType,
                        BasicRounds = new List<DDCLRoundLogItem>(),
                        GreaterRounds = new List<DDCLRoundLogItem>(),
                        Logs = new List<DDCLGachaLogItem>()
                    };
                    Banners.Add(bannerlog);
                    foreach(var rlog in blog.R)
                    {
                        DDCLRoundLogItem basicround = new DDCLRoundLogItem
                        {
                            VersionId = vlog.id,
                            BannerId = blog.id,
                            CategorizedGachaType = blog.poolType,
                            EpitomizedPathID = rlog.epitomizedPathID,
                            Logs = new List<DDCLGachaLogItem>(),
                            Original = rlog
                        };
                        BasicRounds.Add(basicround);
                        bannerlog.BasicRounds.Add(basicround);
                        foreach(var ilog in rlog.L)
                        {
                            DDCLGachaLogItem logitem = new DDCLGachaLogItem
                            {
                                VersionId = vlog.id,
                                BannerId = blog.id,
                                CategorizedGachaType = blog.poolType,
                                Id = ilog.id,
                                StoragedId = ilog.storagedid,
                                Time = ilog.time,
                                GachaType = ilog.gachatype,
                                Rank = ilog.rank,
                                UnitClass = ilog.unitclass,
                                UnitType = ilog.unittype,
                                IdLost = ilog.idLost
                            };
                            Logs.Add(logitem);
                            basicround.Logs.Add(logitem);
                            bannerlog.Logs.Add(logitem);
                        }
                    }

                }
            }

            // TODO: Log
            return true;
        }

        public bool RebuildGreaterRoundsLibrary() // TODO
        {
            // TODO: Log
            GreaterRounds.Clear();
            var noup = new List<DDCCPoolType> { DDCCPoolType.Beginner, DDCCPoolType.Permanent };
            var haveup = new List<DDCCPoolType> { DDCCPoolType.EventCharacter, DDCCPoolType.EventWeapon };
            foreach (var type in noup)
            {
                var banners = GetBannersByCategorizedType(type);
                var buf = new List<DDCLGachaLogItem>();
                foreach (var bannerlog in banners)
                {
                    foreach (var roundlog in bannerlog.BasicRounds)
                    {
                        if (roundlog.Logs.Count == 0) continue;
                        buf.AddRange(roundlog.Logs);
                        if (roundlog.Logs.Last().Rank == 5)
                        {
                            var greater = new DDCLRoundLogItem
                            {
                                BannerId = bannerlog.BannerId,
                                VersionId = bannerlog.VersionId,
                                CategorizedGachaType = bannerlog.CategorizedGachaType,
                                EpitomizedPathID = 0,
                                Logs = new List<DDCLGachaLogItem>(buf),
                                Original = null
                            };
                            bannerlog.GreaterRounds.Add(greater);
                            GreaterRounds.Add(greater);
                            buf.Clear();
                        }
                    }
                    var greaterround = new DDCLRoundLogItem
                    {
                        BannerId = bannerlog.BannerId,
                        VersionId = bannerlog.VersionId,
                        CategorizedGachaType = bannerlog.CategorizedGachaType,
                        EpitomizedPathID = 0,
                        Logs = new List<DDCLGachaLogItem>(buf),
                        Original = null
                    };
                    bannerlog.GreaterRounds.Add(greaterround);
                    GreaterRounds.Add(greaterround);
                }
            }

            foreach(var type in haveup)
            {
                var banners = GetBannersByCategorizedType(type);
                var buf = new List<DDCLRoundLogItem>();
                foreach(var bannerlog in banners)
                {
                    bannerlog.GreaterRounds.Clear();
                    var bannerlibinfo = DDCL.BannerLib.GetBanner(bannerlog.BannerId);

                    if (buf.Count > 0)
                    {
                        int removecnt = 1 + buf.FindIndex(
                            x => x.Logs.Count > 0 && x.Logs.Last().Rank == 5
                            && (DDCL.BannerLib.GetBanner(x.BannerId)?.rank5Up.Contains(x.Logs.Last().UnitClass) ?? false)
                        );
                        buf.RemoveRange(0, removecnt);
                    }
                    foreach(var roundlog in bannerlog.BasicRounds)
                    {
                        if (roundlog.Logs.Count == 0) continue;
                        if (buf.Count > 0)
                        {
                            if (buf.Last().BannerId == roundlog.BannerId
                                && buf.Last().EpitomizedPathID != 0
                                && buf.Last().EpitomizedPathID != roundlog.EpitomizedPathID
                                ) 
                            {
                                int removecnt = 1 + buf.FindIndex(
                                    x => x.Logs.Count > 0 && x.Logs.Last().Rank == 5
                                    && (DDCL.BannerLib.GetBanner(x.BannerId)?.rank5Up.Contains(x.Logs.Last().UnitClass) ?? false)
                                );
                                buf.RemoveRange(0, removecnt);
                            }
                        }
                        buf.Add(roundlog);
                        if (bannerlibinfo.rank5Up.Contains(roundlog.Logs.Last().UnitClass))
                        {
                            if (roundlog.EpitomizedPathID == 0 || roundlog.EpitomizedPathID == roundlog.Logs.Last().UnitClass) 
                            {
                                var greater = new DDCLRoundLogItem
                                {
                                    VersionId = bannerlog.VersionId,
                                    BannerId = bannerlog.BannerId,
                                    CategorizedGachaType = bannerlog.CategorizedGachaType,
                                    EpitomizedPathID = roundlog.EpitomizedPathID,
                                    Logs = new List<DDCLGachaLogItem>()
                                };
                                foreach(var r in buf)
                                {
                                    greater.Logs.AddRange(r.Logs);
                                }
                                bannerlog.GreaterRounds.Add(greater);
                                GreaterRounds.Add(greater);
                                buf.Clear();
                            }
                        }

                    }
                    var greaterround = new DDCLRoundLogItem
                    {
                        VersionId = bannerlog.VersionId,
                        BannerId = bannerlog.BannerId,
                        CategorizedGachaType = bannerlog.CategorizedGachaType,
                        EpitomizedPathID = buf.Count > 0 ? buf.Last().EpitomizedPathID : 0,
                        Logs = new List<DDCLGachaLogItem>()
                    };
                    foreach (var r in buf)
                    {
                        greaterround.Logs.AddRange(r.Logs);
                    }
                    bannerlog.GreaterRounds.Add(greaterround);
                    GreaterRounds.Add(greaterround);
                }
            }
             // TODO: Log
            return true;
        }

        public bool SwapUser(DDCLUserGachaLog userlog)
        {
            // TODO: Log
            // TODO: Signal
            var old = OriginalLogs;
            OriginalLogs = userlog;
            if (!RebuildBasicLibrary())
            {
                OriginalLogs = old;
                // TODO: Log
                // TODO: Signal
                return false;
            }
            RebuildGreaterRoundsLibrary();
            // TODO: Log
            return true;
        }
    }
}
