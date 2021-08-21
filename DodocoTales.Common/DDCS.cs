using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Common
{
    public delegate void DDCSCommonDelegate();
    public static class DDCS
    {
        public static DDCSCommonDelegate LogReloadCompleted;
        public static DDCSCommonDelegate UidReloadCompleted;
        public static DDCSCommonDelegate UserLibReloadCompleted;
        public static DDCSCommonDelegate UserLibNewUserCreated;
    }
}
