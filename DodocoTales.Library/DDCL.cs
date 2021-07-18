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
        public static int CompareTime(DateTime CustomTime,DateTime LibraryTime)
        {
            var customTimeOffset = GetTimeOffset(CustomTime, Users.getCurrentUser().zone);
            var libraryTimeOffset = GetTimeOffset(LibraryTime, DDCCTimeZone.DefaultUTCP8);
            return DateTimeOffset.Compare(CustomTime, LibraryTime);
        }
        public static int CompareTime(DateTime CustomTime,DDCCTimeZone zone,DateTime LibraryTime)
        {
            var customTimeOffset = GetTimeOffset(CustomTime, zone);
            var libraryTimeOffset = GetTimeOffset(LibraryTime, DDCCTimeZone.DefaultUTCP8);
            return DateTimeOffset.Compare(CustomTime, LibraryTime);
        }
        public static DateTimeOffset GetTimeOffset(DateTime time,DDCCTimeZone zone)
        {
            return new DateTimeOffset(time, GetZoneOffsetTimeSpan(zone));
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
    }
}
