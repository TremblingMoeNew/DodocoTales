using DodocoTales.Common.Enums;
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
    /// DDCVVersionViewScreenVersionLogCard.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVVersionViewScreenVersionLogCard : UserControl
    {
        public static readonly DependencyProperty SummaryProperty = DependencyProperty.Register("Summary", typeof(DDCVVerScnVerLogSummary), typeof(DDCVVersionViewScreenVersionLogCard));
        public DDCVVerScnVerLogSummary Summary
        {
            set { SetValue(SummaryProperty, value); }
            get { return (DDCVVerScnVerLogSummary)GetValue(SummaryProperty); }
        }


        public DDCVVersionViewScreenVersionLogCard()
        {
            InitializeComponent();
        }

        public void LoadVersionInfo(DDCVVersionInfo versionInfo)
        {
            var bannerinfos = new List<DDCVBannerInfo>();
            DDCVInheritedRounds inherit = new DDCVInheritedRounds
            {
                Beginner = versionInfo.Inherited.Beginner.FindAll(x => true),
                Permanent = versionInfo.Inherited.Permanent.FindAll(x => true),
                EventCharacter = versionInfo.Inherited.EventCharacter.FindAll(x => true),
                EventWeapon = versionInfo.Inherited.EventWeapon.FindAll(x => true)
            };
            List<DDCVUnitTextIndicatorInfo> indicatorinfo = new List<DDCVUnitTextIndicatorInfo>();
            int percnt = 0, eccnt = 0, ewcnt = 0;
            int r5cnt = 0, r4cnt = 0;

            foreach(var ban in versionInfo.Logs.B)
            {
                if (ban.poolType == DDCCPoolType.Beginner) 
                {
                    
                    var banl = DDCL.Banners.beginnerPools.FindAll(x => x.id == ban.id);
                    if (banl.Count != 1) continue;
                    var baninfo = banl[0];

                    bannerinfos.Add(new DDCVBannerInfo
                    {
                        Info = baninfo,
                        Inherited = inherit.Beginner.FindAll(x => true),
                        VersionInfo = versionInfo.Info,
                        Logs = ban
                    });

                    for (int i = 0; i < ban.R.Count; i++)
                    {
                        var r = ban.R[i];
                        if (r.L.Count == 0) continue;
                        if (r.L[r.L.Count - 1].rank == 5)
                        {
                            var unit = r.L[r.L.Count - 1];
                            int curr5cnt = 0;
                            foreach (var inr in inherit.Beginner)
                            {
                                curr5cnt += inr.Logs.L.Count;
                            }
                            curr5cnt += r.L.Count;
                            indicatorinfo.Add(new DDCVUnitTextIndicatorInfo
                            {
                                name = unit.name,
                                bannerid = ban.id,
                                versionid = versionInfo.Info.id,
                                id = unit.id,
                                time = unit.time,
                                unitclass = unit.unitclass,
                                cnt = curr5cnt
                            });


                            inherit.Beginner = new List<DDCVInheritedRound>();
                            r5cnt++;
                        }
                        else
                        {
                            inherit.Beginner.Add(new DDCVInheritedRound
                            {
                                version = versionInfo.Info,
                                banner = baninfo,
                                RoundIndex = i + 1,
                                Logs = r
                            });
                        }
                        r4cnt += r.L.FindAll(x => x.rank == 4).Count;
                    }
                }
                else if (ban.poolType == DDCCPoolType.Permanent)
                {
                    var banl = DDCL.Banners.permanentPools.FindAll(x => x.id == ban.id);
                    if (banl.Count != 1)
                    {
                        // TODO
                        continue;
                    }
                    var baninfo = banl[0];
                    bannerinfos.Add(new DDCVBannerInfo
                    {
                        Info = baninfo,
                        Inherited = inherit.Permanent.FindAll(x => true),
                        VersionInfo = versionInfo.Info,
                        Logs = ban
                    });
                    for (int i = 0; i < ban.R.Count; i++)
                    {
                        var r = ban.R[i];
                        if (r.L.Count == 0) continue;
                        percnt += r.L.Count;

                        if (r.L[r.L.Count - 1].rank == 5)
                        {
                            var unit = r.L[r.L.Count - 1];
                            int curr5cnt = 0;
                            foreach (var inr in inherit.Permanent)
                            {
                                curr5cnt += inr.Logs.L.Count;
                            }
                            curr5cnt += r.L.Count;
                            indicatorinfo.Add(new DDCVUnitTextIndicatorInfo
                            {
                                name = unit.name,
                                bannerid = ban.id,
                                versionid = versionInfo.Info.id,
                                id = unit.id,
                                time = unit.time,
                                unitclass = unit.unitclass,
                                cnt = curr5cnt
                            });


                            inherit.Permanent = new List<DDCVInheritedRound>();
                            r5cnt++;
                        }
                        else
                        {
                            inherit.Permanent.Add(new DDCVInheritedRound
                            {
                                version = versionInfo.Info,
                                banner = baninfo,
                                RoundIndex = i + 1,
                                Logs = r
                            });
                        }
                        r4cnt += r.L.FindAll(x => x.rank == 4).Count;

                    }
                }
                else if (ban.poolType == DDCCPoolType.EventCharacter)
                {
                    var banl = versionInfo.Info.banners.FindAll(x => x.id == ban.id);
                    if (banl.Count != 1)
                    {
                        // TODO
                        continue;
                    }
                    var baninfo = banl[0];
                    bannerinfos.Add(new DDCVBannerInfo
                    {
                        Info = baninfo,
                        Inherited = inherit.EventCharacter.FindAll(x => true),
                        VersionInfo = versionInfo.Info,
                        Logs = ban
                    });
                    for (int i = 0; i < ban.R.Count; i++)
                    {
                        var r = ban.R[i];
                        if (r.L.Count == 0) continue;
                        eccnt += r.L.Count; 
                        if (r.L[r.L.Count - 1].rank == 5)
                        {
                            var unit = r.L[r.L.Count - 1];
                            int curr5cnt = 0;
                            foreach (var inr in inherit.EventCharacter)
                            {
                                curr5cnt += inr.Logs.L.Count;
                                if (inr.Logs.L.Count == 0) continue;
                                if (inr.Logs.L[inr.Logs.L.Count - 1].rank == 5) curr5cnt = 0;
                            }
                            curr5cnt += r.L.Count;
                            indicatorinfo.Add(new DDCVUnitTextIndicatorInfo
                            {
                                name = unit.name,
                                bannerid = ban.id,
                                versionid = versionInfo.Info.id,
                                id = unit.id,
                                time = unit.time,
                                unitclass = unit.unitclass,
                                cnt = curr5cnt
                            });

                            r5cnt++;
                            if (baninfo.rank5Up.FindAll(x => x == unit.unitclass).Count > 0) 
                                inherit.EventCharacter = new List<DDCVInheritedRound>();
                            else
                            {
                                inherit.EventCharacter.Add(new DDCVInheritedRound
                                {
                                    version = versionInfo.Info,
                                    banner = baninfo,
                                    RoundIndex = i + 1,
                                    Logs = r
                                });
                            }
                        }
                        else
                        {
                            inherit.EventCharacter.Add(new DDCVInheritedRound
                            {
                                version = versionInfo.Info,
                                banner = baninfo,
                                RoundIndex = i + 1,
                                Logs = r
                            });
                        }
                        r4cnt += r.L.FindAll(x => x.rank == 4).Count;
                    }
                }
                else if (ban.poolType == DDCCPoolType.EventWeapon)
                {
                    var banl = versionInfo.Info.banners.FindAll(x => x.id == ban.id);
                    if (banl.Count != 1)
                    {
                        // TODO
                        continue;
                    }
                    var baninfo = banl[0];
                    bannerinfos.Add(new DDCVBannerInfo
                    {
                        Info = baninfo,
                        Inherited = inherit.EventWeapon.FindAll(x => true),
                        VersionInfo = versionInfo.Info,
                        Logs = ban
                    });
                    for (int i = 0; i < ban.R.Count; i++)
                    {
                        var r = ban.R[i];
                        if (r.L.Count == 0) continue;
                        ewcnt += r.L.Count;
                        if (r.L[r.L.Count - 1].rank == 5)
                        {
                            var unit = r.L[r.L.Count - 1];
                            int curr5cnt = 0;
                            foreach (var inr in inherit.EventWeapon)
                            {
                                curr5cnt += inr.Logs.L.Count;
                                if (inr.Logs.L.Count == 0) continue;
                                if (inr.Logs.L[inr.Logs.L.Count - 1].rank == 5) curr5cnt = 0;
                            }
                            curr5cnt += r.L.Count;
                            indicatorinfo.Add(new DDCVUnitTextIndicatorInfo
                            {
                                name = unit.name,
                                bannerid = ban.id,
                                versionid = versionInfo.Info.id,
                                id = unit.id,
                                time = unit.time,
                                unitclass = unit.unitclass,
                                cnt = curr5cnt
                            });

                            r5cnt++;
                            if (baninfo.rank5Up.FindAll(x => x == unit.unitclass).Count > 0)
                                inherit.EventWeapon = new List<DDCVInheritedRound>();
                            else
                            {
                                inherit.EventWeapon.Add(new DDCVInheritedRound
                                {
                                    version = versionInfo.Info,
                                    banner = baninfo,
                                    RoundIndex = i + 1,
                                    Logs = r
                                });
                            }
                        }
                        else
                        {
                            inherit.EventWeapon.Add(new DDCVInheritedRound
                            {
                                version = versionInfo.Info,
                                banner = baninfo,
                                RoundIndex = i + 1,
                                Logs = r
                            });
                        }
                        r4cnt += r.L.FindAll(x => x.rank == 4).Count;
                    }
                }
            }

            indicatorinfo.Reverse();
            bannerinfos.Reverse();

            DDCVVerScnVerLogSummary summary = new DDCVVerScnVerLogSummary
            {
                Title = String.Format("{0} - {1}", versionInfo.Info.version, versionInfo.Info.name),
                VersionTime = String.Format("{0:G} - {1:G}", DDCL.GetLibraryTimeOffset(versionInfo.Info.beginTime, true).ToLocalTime(), DDCL.GetLibraryTimeOffset(versionInfo.Info.endTime,true).ToLocalTime()),
                BannerCnt = versionInfo.Logs.B.Count.ToString(),
                PermanentCnt = percnt.ToString(),
                EventCharCnt = eccnt.ToString(),
                EventWeapCnt = ewcnt.ToString(),
                R5Cnt = r5cnt.ToString(),
                R4Cnt = r4cnt.ToString(),
                IndicatorInfo = indicatorinfo
            };
            Action act = () => { Summary = summary; ApplyBannerCards(bannerinfos); RebuildIndicator(); };
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
        public void ApplyBannerCards(List<DDCVBannerInfo> infos)
        {
            Banners.Children.Clear();
            foreach (var info in infos)
            {
                if (info.Info.type == DDCCPoolType.EventCharacter || info.Info.type == DDCCPoolType.EventWeapon)
                {
                    DDCVVerViewScnVerLogEventBanInfoSubCard card = new DDCVVerViewScnVerLogEventBanInfoSubCard();
                    Banners.Children.Add(card);
                    Action act = () => { card.LoadBanner(info); };
                    act.BeginInvoke(null, null);
                }
                else if (info.Info.type == DDCCPoolType.Beginner || info.Info.type == DDCCPoolType.Permanent)
                {
                    DDCVVerViewScnVerLogPerBanInfoSubCard card = new DDCVVerViewScnVerLogPerBanInfoSubCard();
                    Banners.Children.Add(card);
                    Action act = () => { card.LoadBanner(info); };
                    act.BeginInvoke(null, null);
                }
            }
        }
    }
}
