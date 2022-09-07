using DodocoTales.Common.Enums;
using DodocoTales.Library.BannerLibrary;
using DodocoTales.Library.CurrentUser;
using DodocoTales.Library.StoragedUser;
using DodocoTales.Library.UnitLibrary;
using DodocoTales.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library
{
    public static class DDCL
    {
        public static DDCLBannerLibrary BannerLib = new DDCLBannerLibrary();
        public static DDCLCurrentUserLibrary CurrentUser = new DDCLCurrentUserLibrary();
        public static DDCLUserDataLibrary UserDataLib = new DDCLUserDataLibrary();
        public static DDCLUnitLibrary UnitLib = new DDCLUnitLibrary();
        public static ulong ToUnixTimestamp(DateTime time)
            => (ulong)((time.ToUniversalTime().Ticks - 621355968000000000) / 10000);



        public static DateTimeOffset GetTimeOffset(DateTime time, DDCCTimeZone zone)
        {
            return new DateTimeOffset(time, GetZoneOffsetTimeSpan(zone));
        }
        public static DateTimeOffset GetSyncTimeOffset(DateTime time)
        {
            return new DateTimeOffset(time, GetZoneOffsetTimeSpan(DDCCTimeZone.DefaultUTCP8));
        }
        public static DateTimeOffset GetNowDateTimeOffset()
        {
            return new DateTimeOffset(DateTime.Now);
        }
        public static int CheckTimeIsBetween(DateTimeOffset begin, DateTimeOffset end, DateTimeOffset time)
        {
            if (DateTimeOffset.Compare(end, time) < 0) return -1;
            else if (DateTimeOffset.Compare(begin, time) > 0) return 1;
            else return 0;
        }
        public static DateTimeOffset GetBannerTimeOffset(DateTime time, bool sync, DDCCTimeZone zone)
        {
            if (sync) return GetSyncTimeOffset(time);
            else return GetTimeOffset(time,zone);
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


        public static string GetPoolTypeName(DDCCPoolType type)
        {
            switch (type)
            {
                case DDCCPoolType.Beginner:
                    return "新手祈愿";
                case DDCCPoolType.Permanent:
                    return "常驻祈愿";
                case DDCCPoolType.EventCharacter:
                    return "角色活动祈愿";
                case DDCCPoolType.EventWeapon:
                    return "角色武器祈愿";
                case DDCCPoolType.EventCharacter2:
                    return "角色活动祈愿-2";
                default:
                    DDCLog.Warning(DCLN.Lib, "Get UNKNOWN type name");
                    return "(类型错误)";
            }
        }

    }
}
