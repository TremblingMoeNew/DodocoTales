using DodocoTales.Common;
using DodocoTales.Common.Enums;
using DodocoTales.Gui.Enums;
using DodocoTales.Gui.Model;
using DodocoTales.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DodocoTales.Gui.View.Card
{
    /// <summary>
    /// DDCVHomeScreenPermanentLogCard.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVHomeScreenPermanentLogCard : UserControl
    {
        public static readonly DependencyProperty SummaryProperty = DependencyProperty.Register("Summary", typeof(DDCVHomeScnPerLogSummary), typeof(DDCVHomeScreenPermanentLogCard));
        public DDCVHomeScnPerLogSummary Summary
        {
            set { SetValue(SummaryProperty, value); }
            get { return (DDCVHomeScnPerLogSummary)GetValue(SummaryProperty); }
        }

        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register("Current", typeof(DDCVHomeScnPerLogCurrentBanner), typeof(DDCVHomeScreenPermanentLogCard));
        public DDCVHomeScnPerLogCurrentBanner Current
        {
            set { SetValue(CurrentProperty, value); }
            get { return (DDCVHomeScnPerLogCurrentBanner)GetValue(CurrentProperty); }
        }

        public DDCVHomeScreenPermanentLogCard()
        {
            InitializeComponent();
            DDCS.LogReloadCompleted += new DDCSCommonDelegate(LoadSummaryData);
            DDCS.LogReloadCompleted += new DDCSCommonDelegate(LoadCurrentData);
        }

        public void LoadSummaryData()
        {
            var curlog = DDCL.Users.getCurrentUser();

            int cnt = 0;
            int r5charcnt = 0, r5weapcnt = 0, r4charcnt = 0, r4weapcnt = 0;
            int curr5cnt = 0;
            List<DDCVUnitTextIndicatorInfo> info = new List<DDCVUnitTextIndicatorInfo>();

            foreach (var v in curlog.V)
            {
                foreach (var b in v.B.FindAll(x => x.poolType == DDCCPoolType.Permanent))
                {
                    foreach (var r in b.R)
                    {
                        cnt += r.L.Count;
                        curr5cnt += r.L.Count;
                        if (r.L.Count > 0 && r.L[r.L.Count - 1].rank == 5)
                        {
                            var unit = r.L[r.L.Count - 1];
                            info.Add(new DDCVUnitTextIndicatorInfo
                            {
                                name = unit.name,
                                bannerid = b.id,
                                versionid = v.id,
                                id = unit.id,
                                time = unit.time,
                                unitclass = unit.unitclass,
                                cnt = curr5cnt
                            });
                            curr5cnt = 0;
                            if (unit.unittype == DDCCUnitType.Character) r5charcnt++; else if (unit.unittype == DDCCUnitType.Weapon) r5weapcnt++;
                        }
                        foreach(var unit in r.L.FindAll(x => x.rank == 4))
                        {
                            if (unit.unittype == DDCCUnitType.Character) r4charcnt++; else if (unit.unittype == DDCCUnitType.Weapon) r4weapcnt++;
                        }
                    }
                }
            }
            info.Reverse();
            var summary = new DDCVHomeScnPerLogSummary
            {
                TotalCnt = cnt.ToString(),
                R5Cnt = (r5charcnt + r5weapcnt).ToString(),
                R4Cnt = (r4charcnt + r4weapcnt).ToString(),
                R5Avg = (r5charcnt + r5weapcnt) > 0 ? string.Format("{0:F1}", (cnt - curr5cnt) * 1.0 / (r5charcnt + r5weapcnt)) : "——",
                R5PS = cnt > 0 ? string.Format("[{0:P1}]", (r5charcnt + r5weapcnt) * 1.0 / cnt) : "[—%]",
                R4PS = cnt > 0 ? string.Format("[{0:P1}]", (r4charcnt + r4weapcnt) * 1.0 / cnt) : "[—%]",
                R5CWR = string.Format("{0}:{1}", r5charcnt, r5weapcnt),
                R4CWR = string.Format("{0}:{1}", r4charcnt, r4weapcnt),
                IndicatorInfo = info
            };
            Action act = () => { Summary = summary; RebuildIndicator();};
            Dispatcher.BeginInvoke(act);
        }
        public void RebuildIndicator()
        {
            // 临时方案
            IndicatorContainer.Children.Clear();
            foreach(var info in Summary.IndicatorInfo)
            {
                IndicatorContainer.Children.Add(new TextBlock
                {
                    Text = string.Format("{0}[{1}]", info.name, info.cnt)
                });
            }
        }

        public void LoadCurrentData()
        {
            DDCVHomeScnPerLogCurrentBanner current = null;
            foreach(var ver in DDCL.Banners.eventPools)
            {
                if (DDCL.CompareLibTimeWithNow(ver.endTime, true) > 0) continue;
                if (DDCL.CompareLibTimeWithNow(ver.beginTime, true) < 0) break;

                foreach (var banner in DDCL.Banners.permanentPools)
                {
                    if (DDCL.CompareLibTimeWithNow(banner.endTime, banner.endTimeSync) > 0) continue;
                    if (DDCL.CompareLibTimeWithNow(banner.beginTime, banner.beginTimeSync) < 0) break;

                    bool previousr5found = false;
                    int inherit = 0, cnt = 0, roundcnt = 0;
                    int rank5 = 0, rank4 = 0;
                    int lastr5chardis = -1, lastr5weapdis = -1, lastr4chardis = -1, lastr4weapdis = -1, trs = 0;

                    var curlog = DDCL.Users.getCurrentUser();


                    var vlist = curlog.V.FindAll(x => x.id <= ver.id);
                    vlist.Reverse();

                    foreach(var v in vlist)
                    {
                        if (previousr5found && lastr5chardis >= 0 && lastr5weapdis >= 0 && lastr4chardis >= 0 && lastr4weapdis >= 0) break;
                        var blist = v.B.FindAll(x => x.poolType == DDCCPoolType.Permanent && x.id <= banner.id);
                        blist.Reverse();
                        foreach(var b in blist)
                        {
                            if (previousr5found && lastr5chardis >= 0 && lastr5weapdis >= 0 && lastr4chardis >= 0 && lastr4weapdis >= 0) break;
                            var rlist = b.R.FindAll(x => true);
                            rlist.Reverse();
                            if (v.id == ver.id && b.id == banner.id)
                            {
                                roundcnt = b.R.Count;
                                foreach(var r in rlist)
                                {
                                    var llist = r.L.FindAll(x => true);
                                    llist.Reverse();
                                    foreach(var unit in llist)
                                    {
                                        cnt++;
                                        if (unit.rank == 5) 
                                        {
                                            rank5++;
                                            if (unit.unittype == DDCCUnitType.Character && lastr5chardis < 0) lastr5chardis = trs;
                                            if (unit.unittype == DDCCUnitType.Weapon && lastr5weapdis < 0) lastr5weapdis = trs;
                                        }
                                        if (unit.rank == 4)
                                        {
                                            rank4++;
                                            if (unit.unittype == DDCCUnitType.Character && lastr4chardis < 0) lastr4chardis = trs;
                                            if (unit.unittype == DDCCUnitType.Weapon && lastr4weapdis < 0) lastr4weapdis = trs;

                                        }
                                        trs++;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var r in rlist)
                                {
                                    if (previousr5found && lastr5chardis >= 0 && lastr5weapdis >= 0 && lastr4chardis >= 0 && lastr4weapdis >= 0) break;
                                    var llist = r.L.FindAll(x => true);
                                    llist.Reverse();
                                    foreach (var unit in llist)
                                    {
                                        if (previousr5found && lastr5chardis >= 0 && lastr5weapdis >= 0 && lastr4chardis >= 0 && lastr4weapdis >= 0) break;
                                        if (unit.rank == 5)
                                        {
                                            if(!previousr5found)
                                            {
                                                previousr5found = true;
                                            }
                                            if (unit.unittype == DDCCUnitType.Character && lastr5chardis < 0) lastr5chardis = trs;
                                            if (unit.unittype == DDCCUnitType.Weapon && lastr5weapdis < 0) lastr5weapdis = trs;
                                        }
                                        if (unit.rank == 4)
                                        {
                                            if (!previousr5found)
                                            {
                                                rank4++;
                                            }
                                            if (unit.unittype == DDCCUnitType.Character && lastr4chardis < 0) lastr4chardis = trs;
                                            if (unit.unittype == DDCCUnitType.Weapon && lastr4weapdis < 0) lastr4weapdis = trs;

                                        }
                                        if (!previousr5found)
                                        {
                                            inherit++;
                                        }
                                        trs++;
                                    }
                                }
                            }
                        }
                    }

                    int r5dis = Math.Min(lastr5chardis, lastr5weapdis);
                    int r4dis = Math.Min(lastr4chardis, lastr4weapdis);

                    cnt += inherit;
                    current = new DDCVHomeScnPerLogCurrentBanner
                    {
                        Title = String.Format("当前卡池 - {0}", banner.name),
                        Subtitle = String.Format("{0} - {1} - 轮次 {2}", ver.version, banner.hint, roundcnt),
                        BannerTime = String.Format("{0:G} - {1:G}", DDCL.GetLibraryTimeOffset(ver.beginTime,true).ToLocalTime(), DDCL.GetLibraryTimeOffset(ver.endTime,true).ToLocalTime()),
                        InheritedCnt = inherit.ToString(),
                        TotalCnt = cnt.ToString(),
                        R5Cnt = rank5.ToString(),
                        R4Cnt = rank4.ToString(),
                        R5PS = (cnt - r5dis) > 0 ? String.Format("[{0:P1}]", rank5 * 1.0 / (cnt - r5dis)) : "[—%]",
                        R4PS = (cnt - r5dis) > 0 ? String.Format("[{0:P1}]", rank4 * 1.0 / (cnt - r5dis)) : "[—%]",
                        R5NextType = "——",//String.Format("{0} {1}", lastr5chardis, lastr5weapdis),
                        R4NextType = "——"//String.Format("{0} {1}", lastr4chardis, lastr4weapdis)

                    };

                    int lastroundcnt = r5dis;
                    ProgressIndicator.LoadInfo(DDCVIndicatorVolume.Permanent, (roundcnt > 1) ? 0 : inherit, 0, lastroundcnt);
                }

            }

            Action act = () => { Current = current; };
            Dispatcher.BeginInvoke(act);

        }
    }
}
