﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader
{
    public partial class DDCG
    {
        public static DDCGWebGachaLogLoader WebLogLoader = new DDCGWebGachaLogLoader();
        public static DDCGGachaLogMerger LogMerger = new DDCGGachaLogMerger();
    }
}