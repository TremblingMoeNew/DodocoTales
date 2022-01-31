using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DodocoTales.Common.Signals;

namespace DodocoTales.Common
{
    public static partial class DDCS
    {
        public static DDCSCommonDelegate BannerLibReloadFailed;
        public static void Emit_BannerLibReloadFailed()
            => ExecCommonDelegate(BannerLibReloadFailed);

        public static DDCSCommonDelegate BannerLibDeserialized;
        public static void Emit_BannerLibDeserialized()
            => ExecCommonDelegate(BannerLibDeserialized);

        public static DDCSCommonDelegate BannerLibReloadCompleted;
        public static void Emit_BannerLibReloadCompleted()
            => ExecCommonDelegate(BannerLibReloadCompleted);


        public static DDCSCommonDelegate UnitLibReloadFailed;
        public static void Emit_UnitLibReloadFailed()
            => ExecCommonDelegate(UnitLibReloadFailed);

        public static DDCSCommonDelegate UnitLibReloadCompleted;
        public static void Emit_UnitLibReloadCompleted()
            => ExecCommonDelegate(UnitLibReloadCompleted);

        public static DDCSUidParamDelegate CurUserSwapping;
        public static void Emit_CurUserSwapping(ulong uid)
            => ExecUidParamDelegate(CurUserSwapping, uid);

        public static DDCSCommonDelegate CurUserSwapReverted;
        public static void Emit_CurUserSwapReverted()
            => ExecCommonDelegate(CurUserSwapReverted);

        public static DDCSUidParamDelegate CurUserSwapCompleted;
        public static void Emit_CurUserSwapCompleted(ulong uid)
            => ExecUidParamDelegate(CurUserSwapCompleted, uid);

        public static DDCSUidParamDelegate UserLibUidDeplicated;
        public static void Emit_UserLibUidDeplicated(ulong uid)
            => ExecUidParamDelegate(UserLibUidDeplicated, uid);

        public static DDCSCommonDelegate UserLibReloadCompleted;
        public static void Emit_UserLibReloadCompleted()
            => ExecCommonDelegate(UserLibReloadCompleted);

        public static DDCSCommonDelegate UserlogSaveCompleted;
        public static void Emit_UserlogSaveCompleted()
            => ExecCommonDelegate(UserlogSaveCompleted);

        public static DDCSCommonDelegate UserlogSaveFailed;
        public static void Emit_UserlogSaveFailed()
            => ExecCommonDelegate(UserlogSaveFailed);
    }
}
