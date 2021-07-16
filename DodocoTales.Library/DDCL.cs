using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library
{
    public partial class DDCL
    {
        public static DDCL L = new DDCL();

        public DDCLBannerLibrary Banners { get; set; }
        public DDCLItemLibrary Items { get; set; }
        public DDCL()
        {
            Banners = new DDCLBannerLibrary();
            Items = new DDCLItemLibrary();
        }
    }
}
