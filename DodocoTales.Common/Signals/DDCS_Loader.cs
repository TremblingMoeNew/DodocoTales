﻿using DodocoTales.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Common.Signals;

namespace DodocoTales.Common.Signals
{
    public delegate void DDCSImportStatusDelegate(DDCCPoolType type, int idx);
}

namespace DodocoTales.Common
{
    public static partial class DDCS
    {
        public static DDCSImportStatusDelegate ImportStatusFromWebRefreshed;
        public static void Emit_ImportStatusFromWebRefreshed(DDCCPoolType type, int current_page)
            => ExecImportStatusDelegate(ImportStatusFromWebRefreshed, type, current_page);


        public static void ExecImportStatusDelegate(DDCSImportStatusDelegate dele, DDCCPoolType type, int idx)
        {
            if (dele != null)
            {
                foreach (DDCSImportStatusDelegate method in dele.GetInvocationList())
                {
                    method.BeginInvoke(type, idx, null, null);
                }
            }
        }
    }
}
