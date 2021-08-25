using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Common.Enums;
using DodocoTales.Library;
using DodocoTales.Loader.Models;
namespace DodocoTales.Loader
{
    public partial class DDCGLocalGachaLogLoader
    {
        public bool CheckImportedLogValidty(DDCGGachaLogImportResult result)
        {
            var fBegin = DDCG.LogMerger.getFirstLog(DDCCPoolType.Beginner, result.Uid);
            var fPer = DDCG.LogMerger.getFirstLog(DDCCPoolType.Permanent, result.Uid);
            var fEventChar = DDCG.LogMerger.getFirstLog(DDCCPoolType.EventCharacter, result.Uid);
            var fEventWeap = DDCG.LogMerger.getFirstLog(DDCCPoolType.EventWeapon, result.Uid);

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

            var userlog = DDCL.Users.getUser(result.Uid);
            if (fBegin == null) 
            {
                var beginWithIdOrEarlyEnough = result.Beginner.FindAll(x => x.idLost = false || DateTime.Compare(fAll.time, fBegin.time) < 0);
                if (beginWithIdOrEarlyEnough.Any())
                {
                    beginWithIdOrEarlyEnough.Last().IsNewItem = true;
                    foreach (var item in result.Beginner)
                    {
                        if (item.IsNewItem) break;
                        item.IsNewItem = true;
                    }
                }
                else
                {
                    result.BeginnerOriginalEmptyAndLastIdLost = true;
                }
            }
            if (fPer == null) 
            {
                var perWithIdOrEarlyEnough = result.Permanent.FindAll(x => x.idLost = false || DateTime.Compare(fAll.time, fBegin.time) < 0);
                if (perWithIdOrEarlyEnough.Any())
                {
                    perWithIdOrEarlyEnough.Last().IsNewItem = true;
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
            }
            if (fEventChar == null) 
            {
                var ecWithIdOrEarlyEnough = result.EventCharacter.FindAll(x => x.idLost = false || DateTime.Compare(fAll.time, fBegin.time) < 0);
                if (ecWithIdOrEarlyEnough.Any())
                {
                    ecWithIdOrEarlyEnough.Last().IsNewItem = true;
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
            }
            if (fEventWeap == null) 
            {
                var epWithIdOrEarlyEnough = result.EventWeapon.FindAll(x => x.idLost = false || DateTime.Compare(fAll.time, fBegin.time) < 0);
                if (epWithIdOrEarlyEnough.Any())
                {
                    epWithIdOrEarlyEnough.Last().IsNewItem = true;
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
            }



            return true;
        }
    }
}
