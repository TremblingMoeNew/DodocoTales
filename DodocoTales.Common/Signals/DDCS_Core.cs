﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Common.Signals;

namespace DodocoTales.Common.Signals
{
    public delegate void DDCSCommonDelegate();
    public delegate void DDCSUidParamDelegate(ulong uid);
}
namespace DodocoTales.Common
{
    public static partial class DDCS
    {
        public static void ExecCommonDelegate(DDCSCommonDelegate dele)
        {
            if (dele != null)
            {
                foreach (DDCSCommonDelegate method in dele.GetInvocationList())
                {
                    method.BeginInvoke(null, null);
                }
            }
        }
        public static void ExecUidParamDelegate(DDCSUidParamDelegate dele, ulong uid)
        {
            if (dele != null)
            {
                foreach (DDCSUidParamDelegate method in dele.GetInvocationList())
                {
                    method.BeginInvoke(uid, null, null);
                }
            }
        }
    }
}
