using DodocoTales.Common;
using DodocoTales.Common.Enums;
using DodocoTales.Library.CurrentUser.Models;
using DodocoTales.Library.StoragedUser.Models;
using DodocoTales.Logs;
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

        public SortedList<ulong,DDCLGachaLogItem> Logs { get; set; }
        public List<DDCLRoundLogItem> BasicRounds { get; set; }
        public List<DDCLRoundLogItem> GreaterRounds { get; set; }
        public List<DDCLBannerLogItem> Banners { get; set; }

        public DDCLCurrentUserLibrary()
        {
            Logs = new SortedList<ulong, DDCLGachaLogItem>();
            BasicRounds = new List<DDCLRoundLogItem>();
            GreaterRounds = new List<DDCLRoundLogItem>();
            Banners = new List<DDCLBannerLogItem>();
        }

        // 查找
        // Banners
        public DDCLBannerLogItem GetBanner(ulong versionid, ulong bannerid)
            => Banners.Find(x => x.VersionId == versionid && x.BannerId == bannerid);
        public int GetBannerIndex(ulong versionid, ulong bannerid)
            => Banners.FindIndex(x => x.VersionId == versionid && x.BannerId == bannerid);
        public List<DDCLBannerLogItem> GetBannersByVersion(ulong versionid)
            => Banners.FindAll(x => x.VersionId == versionid);
        public List<DDCLBannerLogItem> GetBannersByCategorizedType(DDCCPoolType type)
            => Banners.FindAll(x => x.CategorizedGachaType == type);
        public List<DDCLBannerLogItem> GetBannersByVersionAndType(ulong versionid, DDCCPoolType type)
            => Banners.FindAll(x => x.VersionId == versionid && x.CategorizedGachaType == type);
        // GreaterRounds
        // BasicRounds
        // Logs
        public DDCLGachaLogItem GetItem(ulong internalid)
            => Logs[internalid];
        public int GetItemIndex(ulong id)
            => Logs.IndexOfKey(id);
        public int GetLastRank4Distance(ulong id)
        {
            int idx = GetItemIndex(id);
            var logs_values = Logs.Values;
            DDCLGachaLogItem startitem = logs_values[idx];
            int dis = 0;
            for(int i = idx - 1; i >= 0; i--)
            {
                if(logs_values[i].CategorizedGachaType==startitem.CategorizedGachaType)
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
            var logs_values = Logs.Values;
            DDCLGachaLogItem startitem = logs_values[idx];
            int dis = 0;
            for (int i = idx - 1; i >= 0; i--)
            {
                var currentitem = logs_values[i];
                if (currentitem.CategorizedGachaType == startitem.CategorizedGachaType)
                {
                    dis++;
                    if (currentitem.Rank == 4 && currentitem.UnitType == type) break;
                }
            }
            return dis;
        }
        public int GetLastRank4UpDistance(ulong id)
        {
            int idx = GetItemIndex(id);
            var logs_values = Logs.Values;
            DDCLGachaLogItem startitem = logs_values[idx];
            int dis = 0;
            for (int i = idx - 1; i >= 0; i--)
            {
                var currentitem = logs_values[i];
                if (currentitem.CategorizedGachaType == startitem.CategorizedGachaType)
                {
                    dis++;
                    if (currentitem.Rank == 4)
                    {
                        if (DDCL.BannerLib.GetBanner(currentitem.VersionId, currentitem.BannerId)?.rank4Up.Exists(x => x == currentitem.UnitClass) ?? false) break;
                    }
                }
            }
            return dis;
        }


        public bool RebuildBasicLibrary()  // TODO
        {
            if (OriginalLogs == null)
            {
                DDCLog.Warning(DCLN.Lib, String.Format("Failed to rebuild cache: nullptr"));
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
                    int t_idx = 0;
                    DDCLBannerLogItem bannerlog = new DDCLBannerLogItem
                    {
                        VersionId = vlog.id,
                        BannerId = blog.id,
                        CategorizedGachaType = blog.poolType,
                        BasicRounds = new List<DDCLRoundLogItem>(),
                        GreaterRounds = new List<DDCLRoundLogItem>(),
                        Logs = new List<DDCLGachaLogItem>(),
                        OriginalVersion = vlog,
                        OriginalBanner = blog
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
                            OriginalRound = rlog
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
                                InternalId = GenerateNextInternalId(ilog.time, ref t_idx),
                                Time = ilog.time,
                                GachaType = ilog.gachatype,
                                Rank = ilog.rank,
                                UnitClass = ilog.unitclass,
                                UnitType = ilog.unittype,
                                IdLost = ilog.idLost
                            };
                            Logs.Add(logitem.InternalId, logitem);
                            basicround.Logs.Add(logitem);
                            bannerlog.Logs.Add(logitem);
                        }
                    }

                }
            }

            DDCLog.Info(DCLN.Lib, String.Format("Userdata cache rebuilded"));
            return true;
        }

        public bool RebuildGreaterRoundsLibrary()
        {
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
                                OriginalRound = null
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
                        OriginalRound = null
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
                    var bannerlibinfo = DDCL.BannerLib.GetBanner(bannerlog.VersionId,bannerlog.BannerId);

                    if (buf.Count > 0)
                    {
                        int removecnt = 1 + buf.FindIndex(
                            x => x.Logs.Count > 0 && x.Logs.Last().Rank == 5
                            && (DDCL.BannerLib.GetBanner(x.VersionId,x.BannerId)?.rank5Up.Contains(x.Logs.Last().UnitClass) ?? false)
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
                                    && (DDCL.BannerLib.GetBanner(x.VersionId,x.BannerId)?.rank5Up.Contains(x.Logs.Last().UnitClass) ?? false)
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

            // TODO: Signal
            DDCLog.Info(DCLN.Lib, String.Format("Round cache rebuilded"));
            return true;
        }

        public bool SwapUser(DDCLUserGachaLog userlog)
        {
            if (userlog == null)
            {
                DDCLog.Warning(DCLN.Lib, String.Format("Failed to swap user: nullptr"));
                return false;
            }
            if (userlog == OriginalLogs)
            {
                return true;
            }
            DDCLog.Info(DCLN.Lib, String.Format("Swapping user. UID:{0}", userlog.uid));
            DDCS.Emit_CurUserSwapping(userlog.uid);
            var old = OriginalLogs;
            OriginalLogs = userlog;
            if (!RebuildBasicLibrary())
            {
                OriginalLogs = old;
                DDCLog.Info(DCLN.Lib, String.Format("Swap cancelled."));
                DDCS.Emit_CurUserSwapReverted();
                return false;
            }
            RebuildGreaterRoundsLibrary();
            DDCS.Emit_CurUserSwapCompleted(userlog.uid);
            DDCLog.Info(DCLN.Lib, String.Format("Swap completed."));
            return true;
        }

        public bool SwapUser(ulong uid)
        {
            return SwapUser(DDCL.UserDataLib.GetUserLogByUid(uid));
        }

        DateTime _PreviousGeneratedIIDTime=DateTime.Now;
        public ulong GenerateNextInternalId(DateTime time,ref int index)
        {
            if (DateTime.Compare(_PreviousGeneratedIIDTime, time) != 0)
            {
                index = 0;
                _PreviousGeneratedIIDTime = time;
            }
            return DDCL.GenerateInternalId(time, 5, index++);

        }
    }
}
