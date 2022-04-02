using DodocoTales.Common.Enums;
using DodocoTales.Library.BannerLibrary;
using DodocoTales.Library.CurrentUser;
using DodocoTales.Library.StoragedUser;
using DodocoTales.Library.UnitLibrary;
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

    }
}
