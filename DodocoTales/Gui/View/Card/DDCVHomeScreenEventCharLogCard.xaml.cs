using DodocoTales.Common;
using DodocoTales.Common.Enums;
using DodocoTales.Gui.Enums;
using DodocoTales.Gui.Model;
using DodocoTales.Library;
using DodocoTales.Library.Models;
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

namespace DodocoTales.Gui.View.Card
{
    /// <summary>
    /// DDCVHomeScreenEventCharLogCard.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVHomeScreenEventCharLogCard : UserControl
    {

        public static readonly DependencyProperty SummaryProperty = DependencyProperty.Register("Summary", typeof(DDCVHomeScnECLogSummary), typeof(DDCVHomeScreenEventCharLogCard));
        public DDCVHomeScnECLogSummary Summary
        {
            set { SetValue(SummaryProperty, value); }
            get { return (DDCVHomeScnECLogSummary)GetValue(SummaryProperty); }
        }

        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register("Current", typeof(DDCVHomeScnECLogCurrentBanner), typeof(DDCVHomeScreenEventCharLogCard));
        public DDCVHomeScnECLogCurrentBanner Current
        {
            set { SetValue(CurrentProperty, value); }
            get { return (DDCVHomeScnECLogCurrentBanner)GetValue(CurrentProperty); }
        }

        public static readonly DependencyProperty NextProperty = DependencyProperty.Register("Next", typeof(DDCVHomeScnNextBannerInfo), typeof(DDCVHomeScreenEventCharLogCard));
        public DDCVHomeScnNextBannerInfo Next
        {
            set { SetValue(NextProperty, value); }
            get { return (DDCVHomeScnNextBannerInfo)GetValue(NextProperty); }
        }
        
        public DDCVHomeScreenEventCharLogCard()
        {
            InitializeComponent();
            DDCS.LogReloadCompleted += new DDCSCommonDelegate(LoadSummaryData);
            DDCS.LogReloadCompleted += new DDCSCommonDelegate(LoadCurrentData);
        }
        public void LoadSummaryData()
        {
            var curlog = DDCL.Users.getCurrentUser();

            int cnt = 0;
            int r5cnt = 0, r4cnt = 0, upr5cnt = 0, upr4cnt = 0;
            int curr5cnt = 0;
            List<DDCVUnitTextIndicatorInfo> info = new List<DDCVUnitTextIndicatorInfo>();

            foreach (var v in curlog.V)
            {
                foreach (var b in v.B.FindAll(x => x.poolType == DDCCPoolType.EventCharacter))
                {
                    var lvs = DDCL.Banners.eventPools.FindAll(x => x.id == v.id);
                    if (lvs.Count != 1) 
                    {
                        // TODO: 报错
                        continue;
                    }
                    var lv = lvs[0];
                    var lbs = lv.banners.FindAll(x => x.id == b.id);
                    if (lbs.Count != 1) 
                    {
                        // TODO: 报错
                        continue;
                    }
                    var lb = lbs[0];
                    foreach (var r in b.R)
                    {
                        cnt += r.L.Count;
                        curr5cnt += r.L.Count;
                        
                        if (r.L.Count > 0 && r.L[r.L.Count - 1].rank == 5)
                        {
                            r5cnt++;
                            var unit = r.L[r.L.Count - 1];
                            if (lb.rank5Up.Contains(unit.unitclass))
                            {
                                upr5cnt++;
                            }
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
                            
                        }
                        foreach (var unit in r.L.FindAll(x => x.rank == 4))
                        {
                            r4cnt++;
                            if (lb.rank4Up.Contains(unit.unitclass))
                            {
                                upr4cnt++;
                            }
                        }
                    }
                }
            }
            info.Reverse();
            var summary = new DDCVHomeScnECLogSummary
            {
                TotalCnt = cnt.ToString(),
                R5Cnt = r5cnt.ToString(),
                R4Cnt = r4cnt.ToString(),
                R5Avg = r5cnt > 0 ? string.Format("{0:F1}", (cnt - curr5cnt) * 1.0 / (r5cnt)) : "——",
                R5PS = cnt > 0 ? string.Format("[{0:P1}]", r5cnt * 1.0 / cnt) : "[—%]",
                R4PS = cnt > 0 ? string.Format("[{0:P1}]", r4cnt * 1.0 / cnt) : "[—%]",
                R5Up = upr5cnt.ToString(),
                R4Up = upr4cnt.ToString(),
                IndicatorInfo = info
            };
            Action act = () => { Summary = summary; RebuildIndicator(); };
            Dispatcher.BeginInvoke(act);
        }
        public void RebuildIndicator()
        {
            // 临时方案
            IndicatorContainer.Children.Clear();
            foreach (var info in Summary.IndicatorInfo)
            {
                IndicatorContainer.Children.Add(new TextBlock
                {
                    Text = string.Format("{0}[{1}]", info.name, info.cnt)
                });
            }
        }

        public void LoadCurrentData()
        {
            DDCVHomeScnECLogCurrentBanner current = null;
            DDCLBannerInfo next = null;DDCLVersionInfo nextatver = null;

            foreach (var ver in DDCL.Banners.eventPools)
            {
                if (DDCL.CompareLibTimeWithNow(ver.endTime, true) > 0) continue;
                
                foreach(var banner in ver.banners.FindAll(x=>x.type==DDCCPoolType.EventCharacter))
                {
                    if (DDCL.CompareLibTimeWithNow(banner.endTime, banner.endTimeSync) > 0) continue;
                    if (DDCL.CompareLibTimeWithNow(banner.beginTime, banner.beginTimeSync) < 0) 
                    {
                        next = banner;
                        nextatver = ver;
                        break;
                    }

                    bool previousr5found = false;
                    int inherit = 0, cnt = 0, roundcnt = 1;
                    int rank5 = 0, rank4 = 0, rank5up = 0, rank4up = 0;
                    int lastr5updis = -1, lastr5perdis = -1, lastr4updis = -1, lastr4perdis = -1, trs = 0;


                    var curlog = DDCL.Users.getCurrentUser();


                    var vlist = curlog.V.FindAll(x => x.id <= ver.id);
                    vlist.Reverse();

                    foreach (var v in vlist)
                    {
                        if (previousr5found && lastr5updis >= 0 && lastr5perdis >= 0 && lastr4updis >= 0 && lastr4perdis >= 0) break;
                        var blist = v.B.FindAll(x => x.poolType == DDCCPoolType.EventCharacter && x.id <= banner.id);
                        blist.Reverse();
                        foreach (var b in blist)
                        {
                            if (previousr5found && lastr5updis >= 0 && lastr5perdis >= 0 && lastr4updis >= 0 && lastr4perdis >= 0) break;
                            var rlist = b.R.FindAll(x => true);
                            rlist.Reverse();
                            if (v.id == ver.id && b.id == banner.id)
                            {

                                foreach (var r in rlist)
                                {
                                    var llist = r.L.FindAll(x => true);
                                    llist.Reverse();
                                    foreach (var unit in llist)
                                    {
                                        cnt++;
                                        if (unit.rank == 5)
                                        {
                                            rank5++;
                                            if (banner.rank5Up.Contains(unit.unitclass))
                                            {
                                                rank5up++;
                                                roundcnt++;
                                                if (lastr5updis < 0) lastr5updis = trs;
                                            }
                                            else if (lastr5perdis < 0) lastr5perdis = trs;
                                            
                                        }
                                        if (unit.rank == 4)
                                        {
                                            rank4++;
                                            if (banner.rank4Up.Contains(unit.unitclass))
                                            {
                                                rank4up++;
                                                if (lastr4updis < 0) lastr4updis = trs;
                                            }
                                            else if (lastr4perdis < 0) lastr4perdis = trs;

                                        }
                                        trs++;
                                    }
                                }
                            }
                            else
                            {
                                var lvs = DDCL.Banners.eventPools.FindAll(x => x.id == v.id);
                                if (lvs.Count != 1)
                                {
                                    // TODO: 报错
                                    continue;
                                }
                                var lv = lvs[0];
                                var lbs = lv.banners.FindAll(x => x.id == b.id);
                                if (lbs.Count != 1)
                                {
                                    // TODO: 报错
                                    continue;
                                }
                                var lb = lbs[0];


                                foreach (var r in rlist)
                                {
                                    if (previousr5found && lastr5updis >= 0 && lastr5perdis >= 0 && lastr4updis >= 0 && lastr4perdis >= 0) break;
                                    var llist = r.L.FindAll(x => true);
                                    llist.Reverse();
                                    foreach (var unit in llist)
                                    {
                                        if (previousr5found && lastr5updis >= 0 && lastr5perdis >= 0 && lastr4updis >= 0 && lastr4perdis >= 0) break;
                                        if (unit.rank == 5)
                                        {
                                            if (lb.rank5Up.Contains(unit.unitclass))
                                            {
                                                if (!previousr5found)
                                                {
                                                    previousr5found = true;
                                                }
                                                if (lastr5updis < 0) lastr5updis = trs;
                                            }
                                            else if (lastr5perdis < 0) lastr5perdis = trs;
                                        }
                                        if (unit.rank == 4)
                                        {
                                            if (!previousr5found)
                                            {
                                                rank4++;
                                                if (lb.rank4Up.Contains(unit.unitclass))
                                                {
                                                    rank4up++;
                                                    if (lastr4updis < 0) lastr4updis = trs;
                                                }
                                                else if (lastr4perdis < 0) lastr4perdis = trs;
                                            }

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

                    if (lastr5perdis < 0) lastr5perdis = trs;
                    if (lastr5updis < 0) lastr5updis = trs;
                    if (lastr4updis < 0) lastr4updis = trs;
                    if (lastr4perdis < 0) lastr4perdis = trs;

                    cnt += inherit;
                    string upunit = "";
                    foreach(var r5up in banner.rank5Up)
                    {
                        upunit += string.Format("{0} ", DDCL.Units.getItemById(r5up).name);
                    }
                    upunit += "|";
                    foreach (var r4up in banner.rank4Up)
                    {
                        upunit += string.Format(" {0}", DDCL.Units.getItemById(r4up).name);
                    }

                    int r5dis = Math.Min(lastr5updis, lastr5perdis);
                    int r4dis = Math.Min(lastr4updis, lastr4perdis);
                    current = new DDCVHomeScnECLogCurrentBanner
                    {
                        Title = String.Format("当前卡池 - {0}", banner.name),
                        Subtitle = String.Format("{0} - {1} - 轮次 {2}", ver.version, banner.hint, roundcnt),
                        BannerTime = String.Format("{0:G} - {1:G}", DDCL.GetLibraryTimeOffset(banner.beginTime, banner.beginTimeSync).ToLocalTime(), DDCL.GetLibraryTimeOffset(banner.endTime, banner.endTimeSync).ToLocalTime()),
                        BannerUpUnits = upunit,
                        InheritedCnt = inherit.ToString(),
                        TotalCnt = cnt.ToString(),
                        R5Cnt = rank5.ToString(),
                        R4Cnt = rank4.ToString(),
                        R5PS = (cnt - r5dis) > 0 ? String.Format("[{0:P1}]", rank5 * 1.0 / (cnt - r5dis)) : "[—%]",
                        R4PS = (cnt - r4dis) > 0 ? String.Format("[{0:P1}]", rank4 * 1.0 / (cnt - r4dis)) : "[—%]",
                        R5NextPS = "——/——/——",//string.Format("{0} {1}", lastr5updis, lastr5perdis),
                        R4NextPS = "——/——/——",//string.Format("{ 0} {1}", lastr4updis, lastr4perdis),
                        R5Up = rank5up.ToString(),
                        R4Up = rank4up.ToString()
                    };
                    int lastroundcnt = lastr5updis;
                    var per = lastroundcnt - lastr5perdis;
                    if (per < 0) per = 0;
                    ProgressIndicator.LoadInfo(DDCVIndicatorVolume.EventCharacterNormal, (roundcnt > 1) ? 0 : inherit, per, lastroundcnt);
                }
                if (DDCL.CompareLibTimeWithNow(ver.beginTime, true) < 0) break;
            }
            LoadNextData(nextatver, next);
            Action act = () => { Current = current; };
            Dispatcher.BeginInvoke(act);
        }
        public void LoadNextData(DDCLVersionInfo version,DDCLBannerInfo banner)
        {
            DDCVHomeScnNextBannerInfo next = null;
            if (banner != null)
            {
                string upunit = "";
                foreach (var r5up in banner.rank5Up)
                {
                    upunit += string.Format("{0} ", DDCL.Units.getItemById(r5up).name);
                }
                upunit += "|";
                foreach (var r4up in banner.rank4Up)
                {
                    upunit += string.Format(" {0}", DDCL.Units.getItemById(r4up).name);
                }
                next = new DDCVHomeScnNextBannerInfo
                {
                    Title = String.Format("下期卡池 - {0}", banner.name),
                    Subtitle = String.Format("{0} - {1}", version.version, banner.hint),
                    BannerTime = String.Format("{0:G} - {1:G}", DDCL.GetLibraryTimeOffset(banner.beginTime,banner.beginTimeSync).ToLocalTime(), DDCL.GetLibraryTimeOffset(banner.endTime,banner.endTimeSync).ToLocalTime()),
                    BannerUpUnits = upunit
                };
            }
            Action act = () => { Next = next; };
            Dispatcher.BeginInvoke(act);
        }
    }
}
