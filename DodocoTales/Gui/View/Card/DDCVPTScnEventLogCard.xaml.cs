using DodocoTales.Common;
using DodocoTales.Common.Enums;
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
    /// DDCVPTScnEventLogCard.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVPTScnEventLogCard : UserControl
    {

        public static readonly DependencyProperty SummaryProperty = DependencyProperty.Register("Summary", typeof(DDCVPTScnPoolLogSummary), typeof(DDCVPTScnEventLogCard));
        public DDCVPTScnPoolLogSummary Summary
        {
            set { SetValue(SummaryProperty, value); }
            get { return (DDCVPTScnPoolLogSummary)GetValue(SummaryProperty); }
        }

        DDCCPoolType PoolType;

        public DDCVPTScnEventLogCard()
        {
            InitializeComponent();
            
        }

        public void InitializeCard(DDCCPoolType type)
        {
            PoolType = type;
            Summary = new DDCVPTScnPoolLogSummary
            {
                Title = DDCL.ConvertPoolTypeToName(PoolType)
            };
            DDCS.LogReloadCompleted += new DDCSCommonDelegate(LoadData);
        }
        public void LoadData()
        {
            var bannerinfos = new List<DDCVBannerInfo>();
            List<DDCVInheritedRound> inherit = new List<DDCVInheritedRound>();
            List<DDCVUnitTextIndicatorInfo> indicatorinfo = new List<DDCVUnitTextIndicatorInfo>();

            int totalcnt = 0;
            int r5cnt = 0, r4cnt = 0, r5up = 0, r4up = 0;
            bool firstfound = false;

            var curlog = DDCL.Users.getCurrentUser();
            foreach (var version in curlog.V)
            {
                
                var verl = DDCL.Banners.eventPools.FindAll(x => x.id == version.id);
                if (verl.Count != 1) continue;
                var verinfo = verl[0];
                foreach(var banner in version.B.FindAll(x => x.poolType == PoolType))
                {
                    var banl = verinfo.banners.FindAll(x => x.id == banner.id);
                    if (banl.Count != 1) continue;
                    var baninfo = banl[0];

                    
                    var model = new DDCVBannerInfo
                    {
                        Info = baninfo,
                        Inherited = inherit.FindAll(x => true),
                        VersionInfo = verinfo,
                        Logs = banner
                    };

                    foreach(var r in banner.R)
                    {
                        if (r.L.Count == 0) continue;
                        firstfound = true;
                        var logs = r.L;
                        var unit = logs.Last();

                        totalcnt += logs.Count;
                        if (unit.rank == 5)
                        {
                            int curr5cnt = 0;
                            foreach (var inr in inherit)
                            {
                                curr5cnt += inr.Logs.L.Count;
                                if (inr.Logs.L.Count == 0) continue;
                                if (inr.Logs.L.Last().rank == 5) curr5cnt = 0;
                            }
                            curr5cnt += logs.Count;
                            indicatorinfo.Add(new DDCVUnitTextIndicatorInfo
                            {
                                name = unit.name,
                                bannerid = banner.id,
                                versionid = verinfo.id,
                                id = unit.id,
                                time = unit.time,
                                unitclass = unit.unitclass,
                                cnt = curr5cnt
                            });
                            r5cnt++;
                            if (baninfo.rank5Up.Contains(unit.unitclass))
                            {
                                inherit = new List<DDCVInheritedRound>();
                                r5up++;
                            }     
                            else
                            {
                                inherit.Add(new DDCVInheritedRound
                                {
                                    version = verinfo,
                                    banner = baninfo,
                                    Logs = r
                                });
                            }
                        }
                        else
                        {
                            inherit.Add(new DDCVInheritedRound
                            {
                                version = verinfo,
                                banner = baninfo,
                                Logs = r
                            });
                        }
                        var r4l = logs.FindAll(x => x.rank == 4);
                        r4cnt += r4l.Count;
                        r4up += r4l.FindAll(x => baninfo.rank4Up.Contains(x.unitclass)).Count;
                    }

                    if (firstfound) bannerinfos.Add(model);
                }

            }

            DDCVPTScnPoolLogSummary summary = new DDCVPTScnPoolLogSummary
            {
                Title = DDCL.ConvertPoolTypeToName(PoolType),
                BannerCnt = bannerinfos.Count.ToString(),
                TotalCnt = totalcnt.ToString(),
                R5Cnt = totalcnt == 0 ? "0 [—%]" : String.Format("{0} [{1:P2}]", r5cnt, r5cnt * 1.0 / totalcnt),
                R4Cnt = totalcnt == 0 ? "0 [—%]" : String.Format("{0} [{1:P1}]", r4cnt, r4cnt * 1.0 / totalcnt),
                R5Up = totalcnt == 0 ? "0 [—%]" : String.Format("{0} [{1:P2}]", r5up, r5up * 1.0 / totalcnt),
                R4Up = totalcnt == 0 ? "0 [—%]" : String.Format("{0} [{1:P1}]", r4up, r4up * 1.0 / totalcnt)
            };
            Action act = () => { Summary = summary; };
            Dispatcher.BeginInvoke(act);
            Action act2 = () => { ApplyIndicators(indicatorinfo); };
            Dispatcher.BeginInvoke(act2);
            Action act3 = () => { ApplyBannerCards(bannerinfos); };
            Dispatcher.BeginInvoke(act3);

        }

        public void ApplyIndicators(List<DDCVUnitTextIndicatorInfo> infos)
        {
            // 临时方案
            infos.Reverse();
            IndicatorContainer.Children.Clear();
            foreach (var info in infos)
            {
                IndicatorContainer.Children.Add(new TextBlock
                {
                    Text = String.Format("{0}[{1}]", info.name, info.cnt)
                });
            }
        }

        public void ApplyBannerCards(List<DDCVBannerInfo> infos)
        {
            infos.Reverse();
            Banners.Children.Clear();
            foreach (var info in infos)
            {
                DDCVVerViewScnVerLogEventBanInfoSubCard card = new DDCVVerViewScnVerLogEventBanInfoSubCard();
                Banners.Children.Add(card);
                Action act = () => { card.LoadBanner(info); };
                act.BeginInvoke(null, null);
            }
        }
    }
}
