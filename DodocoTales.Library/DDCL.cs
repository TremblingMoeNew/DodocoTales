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

        public static ulong GenerateInternalId(DateTime time, int round, int index)
            => ToUnixTimestamp(time) * 1000 + (ulong)round * 100 + (ulong)index;
    }
}
