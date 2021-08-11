using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Common.Enums;
namespace DodocoTales.Library
{
    public partial class DDCL
    {
        public static DDCLBannerLibrary Banners = new DDCLBannerLibrary();
        public static DDCLUnitLibrary Units = new DDCLUnitLibrary();
        public static DDCLUserLibrary Users = new DDCLUserLibrary();
        public static DDCLMetaVersionLibrary MetaVersion = new DDCLMetaVersionLibrary();
        public static DDCLSettingsLibrary Settings = new DDCLSettingsLibrary();
        public static int CompareLibTimeWithNow(DateTime LibraryTime, bool sync = false)
        {
            var now = GetNowDateTimeOffset();
            var libraryTimeOffset = GetLibraryTimeOffset(LibraryTime, sync);
            return DateTimeOffset.Compare(now, LibraryTime);
        }

        public static int CompareTime(DateTime CustomTime,DateTime LibraryTime, bool sync = false)
        {
            var customTimeOffset = GetUserTimezoneTimeOffset(CustomTime);
            var libraryTimeOffset = GetLibraryTimeOffset(LibraryTime, sync);
            return DateTimeOffset.Compare(CustomTime, LibraryTime);
        }

        public static int CompareLibTime(DateTime Lib1,DateTime Lib2)
        {
            var library1Offset = GetTimeOffset(Lib1, DDCCTimeZone.DefaultUTCP8);
            var library2Offset = GetTimeOffset(Lib2, DDCCTimeZone.DefaultUTCP8);
            return DateTimeOffset.Compare(library1Offset, library2Offset);
        }

        public static DateTimeOffset GetTimeOffset(DateTime time,DDCCTimeZone zone)
        {
            return new DateTimeOffset(time, GetZoneOffsetTimeSpan(zone));
        }
        public static DateTimeOffset GetUserTimezoneTimeOffset(DateTime time)
        {
            return GetTimeOffset(time, Users.getCurrentUser().zone);
        }
        public static DateTimeOffset GetLibraryTimeOffset(DateTime time, bool sync = false)
        {
            if (sync) return GetSyncTimeOffset(time); else return GetUserTimezoneTimeOffset(time);
        }
        public static DateTimeOffset GetSyncTimeOffset(DateTime time)
        {
            return new DateTimeOffset(time, GetZoneOffsetTimeSpan(DDCCTimeZone.DefaultUTCP8));
        }
        public static TimeSpan GetZoneOffsetTimeSpan(DDCCTimeZone zone)
        {
            switch (zone)
            {
                case DDCCTimeZone.EuropeUTCP1:
                    return new TimeSpan(1, 0, 0);
                case DDCCTimeZone.AmericaUTCM5:
                    return new TimeSpan(-5, 0, 0);
                case DDCCTimeZone.DefaultUTCP8:
                default:
                    return new TimeSpan(8, 0, 0);
            }
        }
        public static DateTimeOffset GetNowDateTimeOffset()
        {
            return new DateTimeOffset(DateTime.Now);
        }

        public static string ConvertPoolTypeToName(DDCCPoolType type)
        {
            switch(type)
            {
                case DDCCPoolType.Beginner:
                    return "新手祈愿";
                case DDCCPoolType.Permanent:
                    return "常驻祈愿";
                case DDCCPoolType.EventCharacter:
                    return "角色活动祈愿";
                case DDCCPoolType.EventWeapon:
                    return "武器活动祈愿";
                default:
                    return "未知祈愿";
            }
        }
    }
}
