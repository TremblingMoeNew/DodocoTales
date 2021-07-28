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

        public static int CompareLibTimeWithNow(DateTime LibraryTime)
        {
            var now = GetNowDateTimeOffset();
            var libraryTimeOffset = GetTimeOffset(LibraryTime, DDCCTimeZone.DefaultUTCP8);
            return DateTimeOffset.Compare(now, LibraryTime);
        }

        public static int CompareTime(DateTime CustomTime,DateTime LibraryTime)
        {
            var customTimeOffset = GetTimeOffset(CustomTime, Users.getCurrentUser().zone);
            var libraryTimeOffset = GetTimeOffset(LibraryTime, DDCCTimeZone.DefaultUTCP8);
            return DateTimeOffset.Compare(CustomTime, LibraryTime);
        }
        public static int CompareLibTime(DateTime Lib1,DateTime Lib2)
        {
            var library1Offset = GetTimeOffset(Lib1, DDCCTimeZone.DefaultUTCP8);
            var library2Offset = GetTimeOffset(Lib2, DDCCTimeZone.DefaultUTCP8);
            return DateTimeOffset.Compare(library1Offset, library2Offset);
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
        public static DateTimeOffset GetLibraryTimeOffset(DateTime time)
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
    }
}
