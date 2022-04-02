﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Loader.Models
{
   public class DDCGGachaLogResponse
    {
        public int retcode { get; set; }
        public string message { get; set; }
        public DDCGGachaLogResponseData data { get; set; }
    }
    public class DDCGGachaLogResponseData
    {
        public int page { get; set; }
        public int size { get; set; }
        public int total { get; set; }
        public string region { get; set; }
        public List<DDCGGachaLogResponseItem> list { get; set; }
    }
}
