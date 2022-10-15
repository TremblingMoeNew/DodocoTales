using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Library.Utils
{
    public class DDCLInternalIdGenerator
    {
        public static ulong GenerateInternalId(DateTime time, ulong index)
            => DDCL.ToUnixTimestamp(time) * 100000 + index;

        public static ulong MinInternalId(DateTime time)
            => GenerateInternalId(time, 0);
        public static ulong MaxInternalId(DateTime time)
            => GenerateInternalId(time, 99);

        DateTime _PreviousGeneratedIIDTime = DateTime.Now;
        ulong index = 0;
        public ulong GenerateNextInternalId(DateTime time)
        {
            if (DateTime.Compare(_PreviousGeneratedIIDTime, time) != 0)
            {
                index = 0;
                _PreviousGeneratedIIDTime = time;
            }
            return GenerateInternalId(time, index++);
        }

    }
}
