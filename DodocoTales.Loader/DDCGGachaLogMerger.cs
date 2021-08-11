using DodocoTales.Loader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Library;
using DodocoTales.Common.Models;
using DodocoTales.Common.Enums;
namespace DodocoTales.Loader
{
    public partial class DDCGGachaLogMerger
    {
        public void mergeGachaLogs(DDCGGachaInitialLogs initialLogs)
        {
            // 临时解决方案
            mergeUncategorizedLogs();


            var curlog = DDCL.Users.getCurrentUser();
            int bpidx = 0, ppidx = 0, ecpidx = 0, ewpidx = 0;
            int bps = initialLogs.Beginner.Count, pps = initialLogs.Permanent.Count;
            int ecps = initialLogs.EventCharacter.Count, ewps = initialLogs.EventWeapon.Count;
            foreach(var version in DDCL.Banners.eventPools)
            {
                if (DDCL.CompareLibTimeWithNow(version.beginTime,true) < 0) break;

                DDCCVersionLogs ver;
                var verl= curlog.V.FindAll(x => x.id == version.id);
                if (verl.Count > 0)
                {
                    ver = verl[0];
                }
                else
                {
                    ver = new DDCCVersionLogs() { id = version.id, B = new List<DDCCBannerLogs>() };
                    curlog.V.Add(ver);
                }

                // Beginner
                foreach (var banner in DDCL.Banners.beginnerPools)
                {
                    if (DDCL.CompareLibTimeWithNow(banner.beginTime,banner.beginTimeSync) < 0) break;
                    if (DDCL.CompareLibTime(banner.endTime, version.beginTime) < 0) continue;
                    if (DDCL.CompareLibTime(version.endTime, banner.beginTime) < 0) continue;
                    DDCCBannerLogs beginner = null;
                    while (bpidx < bps && DDCL.CompareTime(initialLogs.Beginner[bpidx].time, version.endTime,true) <0 
                        && DDCL.CompareTime(initialLogs.Beginner[bpidx].time, banner.endTime,banner.endTimeSync) < 0)
                    {
                        var item = ConvertToDDCCLogItem(initialLogs.Beginner[bpidx++]);
                        if (beginner == null)
                        {
                            var bl = ver.B.FindAll(x => x.id==banner.id);
                            if (bl.Count == 0)
                            {
                                beginner = new DDCCBannerLogs()
                                {
                                    id = banner.id,
                                    poolType = banner.type,
                                    R = new List<DDCCRoundLogs>() { new DDCCRoundLogs() { epitomizedPathID = 0, L = new List<DDCCGachaLogItem>() } }
                                };
                                ver.B.Add(beginner);
                            }
                            else beginner = bl[0];
                        }
                        beginner.R[beginner.R.Count - 1].L.Add(item);
                        if (item.rank == 5)
                        {
                            beginner.R.Add(new DDCCRoundLogs() { epitomizedPathID = 0, L = new List<DDCCGachaLogItem>() });
                        }
                    }
                }

                // Permanent
                foreach(var banner in DDCL.Banners.permanentPools)
                {
                    if (DDCL.CompareLibTimeWithNow(banner.beginTime,banner.beginTimeSync) < 0) break;
                    if (DDCL.CompareLibTime(banner.endTime, version.beginTime) < 0) continue;
                    if (DDCL.CompareLibTime(version.endTime,banner.beginTime) < 0) continue;
                    DDCCBannerLogs permanent = null;
                    var pl = ver.B.FindAll(x => x.id == banner.id);
                    if (pl.Count == 0)
                    {
                        permanent = new DDCCBannerLogs()
                        {
                            id = banner.id,
                            poolType = banner.type,
                            R = new List<DDCCRoundLogs>() { new DDCCRoundLogs() { epitomizedPathID = 0, L = new List<DDCCGachaLogItem>() } }
                        };
                        ver.B.Add(permanent);
                    }
                    else permanent = pl[0];
                    while (ppidx < pps && DDCL.CompareTime(initialLogs.Permanent[ppidx].time, version.endTime,true) < 0
                         && DDCL.CompareTime(initialLogs.Permanent[ppidx].time, banner.endTime,banner.endTimeSync) < 0)
                    {
                        var item = ConvertToDDCCLogItem(initialLogs.Permanent[ppidx++]);
                        permanent.R[permanent.R.Count - 1].L.Add(item);
                        if (item.rank == 5)
                        {
                            permanent.R.Add(new DDCCRoundLogs() { epitomizedPathID = 0, L = new List<DDCCGachaLogItem>() });
                        }
                    }
                }

                // Event (Character/Weapon)
                foreach (var banner in version.banners)
                {
                    if (DDCL.CompareLibTimeWithNow(banner.beginTime,banner.beginTimeSync) < 0) break;
                    DDCCBannerLogs eventpool = null;
                    var el = ver.B.FindAll(x => x.id == banner.id);
                    if (el.Count == 0)
                    {
                        eventpool = new DDCCBannerLogs()
                        {
                            id = banner.id,
                            poolType = banner.type,
                            R = new List<DDCCRoundLogs>() { new DDCCRoundLogs() { epitomizedPathID = 0, L = new List<DDCCGachaLogItem>() } }
                        };
                        ver.B.Add(eventpool);
                    }
                    else eventpool = el[0];
                    var initLogEvent = (banner.type == DDCCPoolType.EventCharacter )? initialLogs.EventCharacter : initialLogs.EventWeapon;
                    int epidx = (banner.type == DDCCPoolType.EventCharacter) ? ecpidx : ewpidx;
                    int eps = (banner.type == DDCCPoolType.EventCharacter) ? ecps : ewps;

                    while (epidx < eps && DDCL.CompareTime(initLogEvent[epidx].time, banner.endTime,banner.endTimeSync) < 0)
                    {
                        var item = ConvertToDDCCLogItem(initLogEvent[epidx++]);
                        eventpool.R[eventpool.R.Count - 1].L.Add(item);
                        if (item.rank == 5)
                        {
                            eventpool.R.Add(new DDCCRoundLogs() {
                                epitomizedPathID = eventpool.R[eventpool.R.Count - 1].epitomizedPathID,
                                L = new List<DDCCGachaLogItem>()
                            });
                        }
                    }
                    if (banner.type == DDCCPoolType.EventCharacter)
                        ecpidx = epidx;
                    else
                        ewpidx = epidx;
                }
            }

            for(int i = bpidx; i < bps; i++)
            {
                if (curlog.Uncategorized == null) curlog.Uncategorized = new DDCCUncategorizedLogs();
                if (curlog.Uncategorized.Beginner == null) curlog.Uncategorized.Beginner = new List<DDCCGachaLogItem>();
                var item = ConvertToDDCCLogItem(initialLogs.Beginner[i]);
                curlog.Uncategorized.Beginner.Add(item);
            }
            for (int i = ppidx; i < pps; i++)
            {
                if (curlog.Uncategorized == null) curlog.Uncategorized = new DDCCUncategorizedLogs();
                if (curlog.Uncategorized.Permanent == null) curlog.Uncategorized.Permanent = new List<DDCCGachaLogItem>();
                var item = ConvertToDDCCLogItem(initialLogs.Permanent[i]);
                curlog.Uncategorized.Permanent.Add(item);
            }
            for (int i = ecpidx; i < ecps; i++)
            {
                if (curlog.Uncategorized == null) curlog.Uncategorized = new DDCCUncategorizedLogs();
                if (curlog.Uncategorized.EventCharacter == null) curlog.Uncategorized.EventCharacter = new List<DDCCGachaLogItem>();
                var item = ConvertToDDCCLogItem(initialLogs.EventCharacter[i]);
                curlog.Uncategorized.EventCharacter.Add(item);
            }
            for (int i = ewpidx; i < ewps; i++)
            {
                if (curlog.Uncategorized == null) curlog.Uncategorized = new DDCCUncategorizedLogs();
                if (curlog.Uncategorized.EventWeapon == null) curlog.Uncategorized.EventWeapon = new List<DDCCGachaLogItem>();
                var item = ConvertToDDCCLogItem(initialLogs.EventWeapon[i]);
                curlog.Uncategorized.EventWeapon.Add(item);
            }
        }

        public void mergeUncategorizedLogs()
        {
            var curlog = DDCL.Users.getCurrentUser();
            var uncategorizedLogs = curlog.Uncategorized;
            curlog.Uncategorized = null;
            if (uncategorizedLogs == null) return;



            int bpidx = 0, ppidx = 0, ecpidx = 0, ewpidx = 0;
            int bps = uncategorizedLogs.Beginner == null ? 0 : uncategorizedLogs.Beginner.Count;
            int pps = uncategorizedLogs.Permanent == null ? 0 : uncategorizedLogs.Permanent.Count;
            int ecps = uncategorizedLogs.EventCharacter == null ? 0 : uncategorizedLogs.EventCharacter.Count;
            int ewps = uncategorizedLogs.EventWeapon == null ? 0 : uncategorizedLogs.EventWeapon.Count;

            foreach (var version in DDCL.Banners.eventPools)
            {
                if (DDCL.CompareLibTimeWithNow(version.beginTime,true) < 0) break;

                DDCCVersionLogs ver;
                var verl = curlog.V.FindAll(x => x.id == version.id);
                if (verl.Count > 0)
                {
                    ver = verl[0];
                }
                else
                {
                    ver = new DDCCVersionLogs() { id = version.id, B = new List<DDCCBannerLogs>() };
                    curlog.V.Add(ver);
                }

                // Beginner
                foreach (var banner in DDCL.Banners.beginnerPools)
                {
                    if (DDCL.CompareLibTimeWithNow(banner.beginTime,banner.beginTimeSync) < 0) break;
                    if (DDCL.CompareLibTime(banner.endTime, version.beginTime) < 0) continue;
                    if (DDCL.CompareLibTime(version.endTime, banner.beginTime) < 0) continue;
                    DDCCBannerLogs beginner = null;
                    while (bpidx < bps && DDCL.CompareTime(uncategorizedLogs.Beginner[bpidx].time, version.endTime,true) < 0
                        && DDCL.CompareTime(uncategorizedLogs.Beginner[bpidx].time, banner.endTime,banner.endTimeSync) < 0)
                    {
                        var item = uncategorizedLogs.Beginner[bpidx++];
                        if (beginner == null)
                        {
                            var bl = ver.B.FindAll(x => x.id == banner.id);
                            if (bl.Count == 0)
                            {
                                beginner = new DDCCBannerLogs()
                                {
                                    id = banner.id,
                                    poolType = banner.type,
                                    R = new List<DDCCRoundLogs>() { new DDCCRoundLogs() { epitomizedPathID = 0, L = new List<DDCCGachaLogItem>() } }
                                };
                                ver.B.Add(beginner);
                            }
                            else beginner = bl[0];
                        }
                        beginner.R[beginner.R.Count - 1].L.Add(item);
                        if (item.rank == 5)
                        {
                            beginner.R.Add(new DDCCRoundLogs() { epitomizedPathID = 0, L = new List<DDCCGachaLogItem>() });
                        }
                    }
                }

                // Permanent
                foreach (var banner in DDCL.Banners.permanentPools)
                {
                    if (DDCL.CompareLibTimeWithNow(banner.beginTime,banner.beginTimeSync) < 0) break;
                    if (DDCL.CompareLibTime(banner.endTime, version.beginTime) < 0) continue;
                    if (DDCL.CompareLibTime(version.endTime, banner.beginTime) < 0) continue;
                    DDCCBannerLogs permanent = null;
                    var pl = ver.B.FindAll(x => x.id == banner.id);
                    if (pl.Count == 0)
                    {
                        permanent = new DDCCBannerLogs()
                        {
                            id = banner.id,
                            poolType = banner.type,
                            R = new List<DDCCRoundLogs>() { new DDCCRoundLogs() { epitomizedPathID = 0, L = new List<DDCCGachaLogItem>() } }
                        };
                        ver.B.Add(permanent);
                    }
                    else permanent = pl[0];
                    while (ppidx < pps && DDCL.CompareTime(uncategorizedLogs.Permanent[ppidx].time, version.endTime,true) < 0
                         && DDCL.CompareTime(uncategorizedLogs.Permanent[ppidx].time, banner.endTime,banner.endTimeSync) < 0)
                    {
                        var item = uncategorizedLogs.Permanent[ppidx++];
                        permanent.R[permanent.R.Count - 1].L.Add(item);
                        if (item.rank == 5)
                        {
                            permanent.R.Add(new DDCCRoundLogs() { epitomizedPathID = 0, L = new List<DDCCGachaLogItem>() });
                        }
                    }
                }

                // Event (Character/Weapon)
                foreach (var banner in version.banners)
                {
                    if (DDCL.CompareLibTimeWithNow(banner.beginTime,banner.beginTimeSync) < 0) break;
                    DDCCBannerLogs eventpool = null;
                    var el = ver.B.FindAll(x => x.id == banner.id);
                    if (el.Count == 0)
                    {
                        eventpool = new DDCCBannerLogs()
                        {
                            id = banner.id,
                            poolType = banner.type,
                            R = new List<DDCCRoundLogs>() { new DDCCRoundLogs() { epitomizedPathID = 0, L = new List<DDCCGachaLogItem>() } }
                        };
                        ver.B.Add(eventpool);
                    }
                    else eventpool = el[0];
                    var initLogEvent = (banner.type == DDCCPoolType.EventCharacter) ? uncategorizedLogs.EventCharacter : uncategorizedLogs.EventWeapon;
                    int epidx = (banner.type == DDCCPoolType.EventCharacter) ? ecpidx : ewpidx;
                    int eps = (banner.type == DDCCPoolType.EventCharacter) ? ecps : ewps;

                    while (epidx < eps && DDCL.CompareTime(initLogEvent[epidx].time, banner.endTime,banner.endTimeSync) < 0)
                    {
                        var item = initLogEvent[epidx++];
                        eventpool.R[eventpool.R.Count - 1].L.Add(item);
                        if (item.rank == 5)
                        {
                            eventpool.R.Add(new DDCCRoundLogs()
                            {
                                epitomizedPathID = eventpool.R[eventpool.R.Count - 1].epitomizedPathID,
                                L = new List<DDCCGachaLogItem>()
                            });
                        }
                    }
                    if (banner.type == DDCCPoolType.EventCharacter)
                        ecpidx = epidx;
                    else
                        ewpidx = epidx;
                }
            }

            for (int i = bpidx; i < bps; i++)
            {
                if (curlog.Uncategorized == null) curlog.Uncategorized = new DDCCUncategorizedLogs();
                if (curlog.Uncategorized.Beginner == null) curlog.Uncategorized.Beginner = new List<DDCCGachaLogItem>();
                var item = uncategorizedLogs.Beginner[i];
                curlog.Uncategorized.Beginner.Add(item);
            }
            for (int i = ppidx; i < pps; i++)
            {
                if (curlog.Uncategorized == null) curlog.Uncategorized = new DDCCUncategorizedLogs();
                if (curlog.Uncategorized.Permanent == null) curlog.Uncategorized.Permanent = new List<DDCCGachaLogItem>();
                var item = uncategorizedLogs.Permanent[i];
                curlog.Uncategorized.Permanent.Add(item);
            }
            for (int i = ecpidx; i < ecps; i++)
            {
                if (curlog.Uncategorized == null) curlog.Uncategorized = new DDCCUncategorizedLogs();
                if (curlog.Uncategorized.EventCharacter == null) curlog.Uncategorized.EventCharacter = new List<DDCCGachaLogItem>();
                var item = uncategorizedLogs.EventCharacter[i];
                curlog.Uncategorized.EventCharacter.Add(item);
            }
            for (int i = ewpidx; i < ewps; i++)
            {
                if (curlog.Uncategorized == null) curlog.Uncategorized = new DDCCUncategorizedLogs();
                if (curlog.Uncategorized.EventWeapon == null) curlog.Uncategorized.EventWeapon = new List<DDCCGachaLogItem>();
                var item = uncategorizedLogs.EventWeapon[i];
                curlog.Uncategorized.EventWeapon.Add(item);
            }

        }


        public DDCCGachaLogItem ConvertToDDCCLogItem(DDCGGachaLogResponseItem DDCGItem)
        {
            var unitclass = DDCL.Units.getItemByCode(DDCGItem.name);
            return new DDCCGachaLogItem()
            {
                id = DDCGItem.id,
                code = DDCGItem.name,
                rank = DDCGItem.rank_type,
                time = DDCGItem.time,
                unittype = DDCGItem.item_type,
                unitclass = unitclass.id,
                name = unitclass.type == DDCCUnitType.Unknown ? DDCGItem.name : unitclass.name

            };
        }

        public ulong getLastLogId(DDCCPoolType type)
        {
            var curlog = DDCL.Users.getCurrentUser();
            if (curlog.Uncategorized != null)
            {
                List<DDCCGachaLogItem> uncategorized=null;
                switch (type)
                {
                    case DDCCPoolType.Beginner:
                        uncategorized = curlog.Uncategorized.Beginner;
                        break;
                    case DDCCPoolType.Permanent:
                        uncategorized = curlog.Uncategorized.Permanent;
                        break;
                    case DDCCPoolType.EventCharacter:
                        uncategorized = curlog.Uncategorized.EventCharacter;
                        break;
                    case DDCCPoolType.EventWeapon:
                        uncategorized = curlog.Uncategorized.EventWeapon;
                        break;
                }
                if (uncategorized != null && uncategorized.Count > 0)
                {
                    return uncategorized[uncategorized.Count - 1].id;
                }
            }
            for (int i=curlog.V.Count-1;i>=0;i--)
            {
                var banners = curlog.V[i].B.FindAll(x => x.poolType == type);
                for (int j = banners.Count - 1; j >= 0; j--)
                {
                    if (banners[j].R[banners[j].R.Count - 1].L.Count > 0)
                    {
                        var l = banners[j].R[banners[j].R.Count - 1].L;
                        return l[l.Count - 1].id;
                    }
                }
            }
            return 0;
        }
    }
}
