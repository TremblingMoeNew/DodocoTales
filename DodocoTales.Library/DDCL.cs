using DodocoTales.Library.BannerLibrary;
using DodocoTales.Library.CurrentUser;
using DodocoTales.Library.StoragedUser;
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
    }
}
