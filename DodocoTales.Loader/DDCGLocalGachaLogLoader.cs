using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Common.Enums;
using DodocoTales.Common.Models;
using DodocoTales.Library;
using DodocoTales.Loader.Models;
namespace DodocoTales.Loader
{
    public partial class DDCGLocalGachaLogLoader
    {

        public bool CheckImportedLogValidty(DDCGGachaLogImportResult result)
        {
            var fBegin = DDCG.LogMerger.getFirstLog(DDCCPoolType.Beginner,out DDCCBannerLogs fBeginBan, result.Uid);
            var fPer = DDCG.LogMerger.getFirstLog(DDCCPoolType.Permanent, out DDCCBannerLogs fPerBan, result.Uid);
            var fEventChar = DDCG.LogMerger.getFirstLog(DDCCPoolType.EventCharacter, out DDCCBannerLogs fEventCharBan, result.Uid);
            var fEventWeap = DDCG.LogMerger.getFirstLog(DDCCPoolType.EventWeapon, out DDCCBannerLogs fEventWeapBan, result.Uid);

            var fAll = fBegin;
            if (fAll == null) fAll = fPer; else if (fPer != null && DateTime.Compare(fAll.time, fPer.time) > 0) fAll = fPer;
            if (fAll == null) fAll = fEventChar; else if (fEventChar != null && DateTime.Compare(fAll.time, fEventChar.time) > 0) fAll = fEventChar;
            if (fAll == null) fAll = fEventWeap; else if (fEventWeap != null && DateTime.Compare(fAll.time, fEventWeap.time) > 0) fAll = fEventWeap;

            if (fAll == null)
            {
                result.UserLogNotExist = true;
                var beginWithId = result.Beginner.FindAll(x => x.idLost = false);
                if (beginWithId.Any())
                {
                    beginWithId.Last().IsNewItem = true;
                    foreach(var item in result.Beginner)
                    {
                        if (item.IsNewItem) break;
                        item.IsNewItem = true;
                    }
                }
                else
                {
                    result.BeginnerOriginalEmptyAndLastIdLost = true;
                }

                var perWithId = result.Permanent.FindAll(x => x.idLost = false);
                if (perWithId.Any())
                {
                    perWithId.Last().IsNewItem = true;
                    foreach (var item in result.Permanent)
                    {
                        if (item.IsNewItem) break;
                        item.IsNewItem = true;
                    }
                }
                else
                {
                    result.PermanentOriginalEmptyAndLastIdLost = true;
                }

                var ecWithId = result.EventCharacter.FindAll(x => x.idLost = false);
                if (ecWithId.Any())
                {
                    ecWithId.Last().IsNewItem = true;
                    foreach (var item in result.EventCharacter)
                    {
                        if (item.IsNewItem) break;
                        item.IsNewItem = true;
                    }
                }
                else
                {
                    result.EvnetCharOriginalEmptyAndLastIdLost = true;
                }

                var epWithId = result.EventWeapon.FindAll(x => x.idLost = false);
                if (epWithId.Any())
                {
                    epWithId.Last().IsNewItem = true;
                    foreach (var item in result.EventWeapon)
                    {
                        if (item.IsNewItem) break;
                        item.IsNewItem = true;
                    }
                }
                else
                {
                    result.EventWeapOriginalEmptyAndLastIdLost = true;
                }

                return false;
            }

            if (!CheckPoolValidty(fBegin, fAll, fBeginBan, result.Beginner)) result.BeginnerOriginalEmptyAndLastIdLost = true;
            if (!CheckPoolValidty(fPer, fAll, fPerBan, result.Permanent)) result.PermanentOriginalEmptyAndLastIdLost = true;
            if (!CheckPoolValidty(fEventChar, fAll, fEventCharBan, result.EventCharacter)) result.EvnetCharOriginalEmptyAndLastIdLost = true;
            if (!CheckPoolValidty(fEventWeap, fAll, fEventWeapBan, result.EventWeapon)) result.EventWeapOriginalEmptyAndLastIdLost = true;

            return true;
        }


        public bool CheckPoolValidty(DDCCGachaLogItem first, DDCCGachaLogItem fAll,  DDCCBannerLogs fban, List<DDCGGachaLogImportedItem> items)
        {
            if (first == null)
            {
                var beginWithIdOrEarlyEnough = items.FindAll(x => x.idLost = false || DateTime.Compare(fAll.time, first.time) < 0);
                if (beginWithIdOrEarlyEnough.Any())
                {
                    beginWithIdOrEarlyEnough.Last().IsNewItem = true;
                    foreach (var item in items)
                    {
                        if (item.IsNewItem) break;
                        item.IsNewItem = true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                bool matched = false;
                if (!first.idLost)
                {
                    var fbl = items.FindAll(x => x.id == first.id);
                    if (fbl.Any())
                    {
                        fbl.First().IsNewItem = true;
                        foreach (var item in items)
                        {
                            if (item.IsNewItem)
                            {
                                item.IsNewItem = false;
                                break;
                            }
                            item.IsNewItem = true;
                        }
                        matched = true;
                    }
                }
                if (!matched)
                {
                    var logbefore = items.FindAll(x => DateTime.Compare(x.time, first.time) < 0);
                    foreach (var item in logbefore)
                    {
                        item.IsNewItem = true;
                    }
                    var logat = items.FindAll(x => DateTime.Compare(x.time, first.time) == 0);
                    if (logat.Count > 1) 
                    {
                        int logatptr = 0;
                        while (logatptr < logat.Count
                            && logat[logatptr].unitclass != first.unitclass 
                            && (!logat[logatptr].ItemUnitClassNotFound 
                                || !(logat[logatptr].name==first.name|| logat[logatptr].code==first.code)
                                )
                            )
                        {
                            logatptr++;
                        }
                        if (logat.Count - logatptr > 1) 
                        {
                            List<DDCCGachaLogItem> original = new List<DDCCGachaLogItem>();
                            foreach(var rnd in fban.R)
                            {
                                var ls = rnd.L.FindAll(x => DateTime.Compare(x.time, first.time) == 0);
                                if (ls.Any()) original.AddRange(ls); else break;
                            }
                            while (logatptr < logat.Count)
                            {
                                while (logatptr < logat.Count
                                    && logat[logatptr].unitclass != first.unitclass
                                    && (!logat[logatptr].ItemUnitClassNotFound
                                        || !(logat[logatptr].name == first.name || logat[logatptr].code == first.code)
                                        )
                                    )
                                {
                                    logatptr++;
                                }
                                if (logatptr == logat.Count) break;
                                if (logat.Count - logatptr > original.Count) 
                                {
                                    logatptr++;
                                    continue;
                                }
                                bool exactlymatched = true;
                                for(int i = logatptr, j = 0; i < logat.Count; i++, j++)
                                {
                                    if (logat[i].unitclass != original[j].unitclass
                                    && (!logat[i].ItemUnitClassNotFound
                                        || !(logat[i].name == original[j].name || logat[i].code == original[j].code)
                                        )
                                    )
                                    {
                                        exactlymatched = false;
                                        break;
                                    }
                                }
                                if (!exactlymatched)
                                {
                                    logatptr++;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < logatptr; i++)
                        {
                            items[i].IsNewItem = true;
                        }
                    }
                    
                }
            }
            return true;
        }



    }
}
