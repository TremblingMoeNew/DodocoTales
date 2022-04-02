using DodocoTales.Loader.UniversalFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader
{
    public static partial class DDCG
    {
        public static DDCGWebGachaLogLoader WebLogLoader = new DDCGWebGachaLogLoader();
        public static DDCGV0LogUpdater V0Updater = new DDCGV0LogUpdater();
        public static DDCGUniversalFormatExporter UFExporter = new DDCGUniversalFormatExporter();
        public static void Initialize()
        {
            V0Updater.Initialize();
        }
    }
}
