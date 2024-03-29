﻿using DodocoTales.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.StoragedUser.Models.V0
{
    public class DDCLStoragedBannerLogs
    {
        public ulong id { get; set; }
        public List<DDCLStoragedRoundLogs> R { get; set; }

        // as backup
        public DDCCPoolType poolType { get; set; }
    }
}
