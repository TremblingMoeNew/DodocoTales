using DodocoTales.Common.Enums;
using DodocoTales.Common.Models;
using DodocoTales.Gui.Model;
using DodocoTales.Gui.View.Card;
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

namespace DodocoTales.Gui.View.Screen
{
    /// <summary>
    /// DDCVBannerDetailsScreen.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVBannerDetailsScreen : UserControl
    {
        public static readonly DependencyProperty SummaryProperty = DependencyProperty.Register("Summary", typeof(DDCVBanScnBanSummary), typeof(DDCVBannerDetailsScreen));
        public DDCVBanScnBanSummary Summary
        {
            set { SetValue(SummaryProperty, value); }
            get { return (DDCVBanScnBanSummary)GetValue(SummaryProperty); }
        }


        public DDCVBannerDetailsScreen()
        {
            InitializeComponent();
        }

        public async void LoadBanner(DDCVBannerInfo info)
        {
            string bannertime = "";
            int cnt = 0, inheritcnt = 0, roundcnt = 1;
            int r5cnt = 0, r4cnt = 0;

            int inhr5cnt = 0, inhr4cnt = 0;

            List<DDCVUpItemCntInfo> R5Info = null;
            List<int> R5InfoCnt = new List<int> { 0, 0, 0 };
            List<DDCVUpItemCntInfo> R4Info = null;
            List<int> R4InfoCnt = new List<int> { 0, 0, 0, 0, 0, 0 };

            List<DDCVRoundInfo> RoundInfo = new List<DDCVRoundInfo>();
            switch (info.Info.type)
            {
                case DDCCPoolType.Beginner:
                case DDCCPoolType.Permanent:
                    bannertime = String.Format("{0:G} - {1:G}", DDCL.GetLibraryTimeOffset(info.VersionInfo.beginTime).ToLocalTime(), DDCL.GetLibraryTimeOffset(info.VersionInfo.endTime).ToLocalTime());
                    R5Info = new List<DDCVUpItemCntInfo> {
                        new DDCVUpItemCntInfo { Name = "常驻角色" },
                        new DDCVUpItemCntInfo { Name = "常驻武器" },
                        null
                    };
                    R4Info = new List<DDCVUpItemCntInfo> {
                        new DDCVUpItemCntInfo { Name = "常驻角色" },
                        null,
                        new DDCVUpItemCntInfo { Name = "常驻武器" },
                        null,
                        new DDCVUpItemCntInfo { Name = "继承四星" },
                        null
                    };
                    break;
                case DDCCPoolType.EventCharacter:
                case DDCCPoolType.EventWeapon:
                    bannertime = String.Format("{0:G} - {1:G}", DDCL.GetLibraryTimeOffset(info.Info.beginTime).ToLocalTime(), DDCL.GetLibraryTimeOffset(info.Info.endTime).ToLocalTime());
                    R5Info = new List<DDCVUpItemCntInfo>();
                    R4Info = new List<DDCVUpItemCntInfo>();
                    foreach(var unitclass in info.Info.rank5Up)
                    {
                        R5Info.Add(new DDCVUpItemCntInfo { Name = DDCL.Units.getItemById(unitclass)?.name });
                    }
                    R5Info.Add(new DDCVUpItemCntInfo { Name = "常驻/继承五星" });
                    while (R5Info.Count < 3) R5Info.Add(null);
                    foreach (var unitclass in info.Info.rank4Up)
                    {
                        R4Info.Add(new DDCVUpItemCntInfo { Name = DDCL.Units.getItemById(unitclass)?.name });
                    }
                    R4Info.Add(new DDCVUpItemCntInfo { Name = "常驻/继承四星" });
                    while (R4Info.Count < 6) R4Info.Add(null);
                    break;
            }

            DDCVBanScnBanSummary summary = new DDCVBanScnBanSummary
            {
                Title = info.Info.name,
                Subtitle = String.Format("{0} - {1} - {2}", info.VersionInfo.version, DDCL.ConvertPoolTypeToName(info.Info.type), info.Info.hint),
                BannerTime = bannertime,
                R5Items = R5Info,
                R4Items = R4Info
            };

            Action act = () => { Summary = summary; };
            await Dispatcher.BeginInvoke(act);


            foreach (var inr in info.Inherited)
            {
                var logs = inr.Logs.L;
                if (logs.Count == 0) continue;
                inheritcnt += logs.Count;

                var unit = logs.Last();
                if (unit.rank == 5)
                {
                    r5cnt++;
                    inhr5cnt++;
                }

                var t = logs.FindAll(x => x.rank == 4).Count;
                r4cnt++;
                inhr4cnt++;

            }

            switch (info.Info.type)
            {
                case DDCCPoolType.Beginner:
                case DDCCPoolType.Permanent:
                    R4InfoCnt[4] = inhr4cnt;
                    break;
                case DDCCPoolType.EventCharacter:
                case DDCCPoolType.EventWeapon:
                    R5InfoCnt[info.Info.rank5Up.Count] = inhr5cnt;
                    R4InfoCnt[info.Info.rank4Up.Count] = inhr4cnt;
                    break;
            }

            RoundInfo.Add(new DDCVRoundInfo
            {
                VersionInfo = info.VersionInfo,
                BannerInfo = info.Info,
                Inherited = info.Inherited,
                Index = 1,
                Logs = new List<DDCCRoundLogs>()
            });

            foreach (var rnd in info.Logs.R)
            {
                var logs = rnd.L;
                if (logs.Count == 0) continue;
                RoundInfo.Last().Logs.Add(rnd);
                cnt += logs.Count;
                var lu = logs.Last();
                if (lu.rank == 5) r5cnt++;

                var r4ls = logs.FindAll(x => x.rank == 4);

                r4cnt += r4ls.Count;
                switch (info.Info.type)
                {
                    case DDCCPoolType.Beginner:
                    case DDCCPoolType.Permanent:
                        if(lu.rank == 5)
                        {
                            if (lu.unittype == DDCCUnitType.Character) R5InfoCnt[0]++;
                            else if (lu.unittype == DDCCUnitType.Weapon) R5InfoCnt[1]++;
                            roundcnt++;
                            RoundInfo.Add(new DDCVRoundInfo
                            {
                                VersionInfo = info.VersionInfo,
                                BannerInfo = info.Info,
                                Inherited = null,
                                Index = roundcnt,
                                Logs = new List<DDCCRoundLogs>()
                            });
                        }
                        R4InfoCnt[0] += r4ls.FindAll(x => x.unittype == DDCCUnitType.Character).Count;
                        R4InfoCnt[2] += r4ls.FindAll(x => x.unittype == DDCCUnitType.Weapon).Count;
                        break;
                    case DDCCPoolType.EventCharacter:
                    case DDCCPoolType.EventWeapon:
                        if (lu.rank == 5)
                        {
                            bool isup = false;
                            for (int i = 0; i < info.Info.rank5Up.Count; i++) 
                            {
                                var unitclass = info.Info.rank5Up[i];
                                if (unitclass == lu.unitclass)
                                {
                                    isup = true;
                                    R5InfoCnt[i]++;
                                    break;
                                }
                            }
                            if(isup)
                            {
                                roundcnt++;
                                RoundInfo.Add(new DDCVRoundInfo
                                {
                                    VersionInfo = info.VersionInfo,
                                    BannerInfo = info.Info,
                                    Inherited = null,
                                    Index = roundcnt,
                                    Logs = new List<DDCCRoundLogs>()
                                });
                            }
                            else{
                                R5InfoCnt[info.Info.rank5Up.Count]++;
                            }
                        }
                        int tup = 0;
                        for (int i = 0; i < info.Info.rank4Up.Count; i++)
                        {
                            var unitclass = info.Info.rank4Up[i];
                            var uls = r4ls.FindAll(x => x.unitclass == unitclass);
                            tup += uls.Count;
                            R4InfoCnt[i] += uls.Count;
                        }
                        R4InfoCnt[info.Info.rank4Up.Count] += r4ls.Count - tup;
                        break;

                }
            }




            for (int i = 0; i < 3; i++)
            {
                if (R5Info[i] != null)
                {
                    R5Info[i].Cnt = r5cnt == 0 ? "0 [—%]" : String.Format("{0} [{1:P2}]", R5InfoCnt[i], R5InfoCnt[i] * 1.0 / r5cnt);
                }
            }
            for (int i = 0; i < 6; i++)
            {
                if (R4Info[i] != null)
                {
                    R4Info[i].Cnt = r4cnt == 0 ? "0 [—%]" : String.Format("{0} [{1:P1}]", R4InfoCnt[i], R4InfoCnt[i] * 1.0 / r4cnt);
                }

            }


            int totalcnt = cnt + inheritcnt;
            summary.InheritedCnt = inheritcnt.ToString();
            summary.RoundCnt = roundcnt.ToString();
            summary.Cnt = cnt.ToString();
            summary.TotalCnt = totalcnt.ToString();

            summary.R5Cnt = totalcnt == 0 ? "0 [—%]" : String.Format("{0} [{1:P2}]", r5cnt, r5cnt * 1.0 / totalcnt);
            summary.R4Cnt = totalcnt == 0 ? "0 [—%]" : String.Format("{0} [{1:P1}]", r4cnt, r4cnt * 1.0 / totalcnt);

            ApplyRoundCards(RoundInfo);

            //Action act2 = () => {  };
            //Dispatcher.BeginInvoke(act2, DispatcherPriority.ApplicationIdle);
        }

        public async void ApplyRoundCards(List<DDCVRoundInfo> RoundInfo)
        {
            Action act1 = () => { Rounds.Children.Clear(); };
            await Dispatcher.BeginInvoke(act1);
            RoundInfo.Reverse();
            foreach (var info in RoundInfo)
            { 
                Action act2 = () => {
                    DDCVBanScnRoundViewCard card= new DDCVBanScnRoundViewCard();
                    Rounds.Children.Add(card);
                    Action act = () => { card.LoadRound(info);};
                    act.BeginInvoke(null, null);
                };
                try
                {
                    await Dispatcher.BeginInvoke(act2, DispatcherPriority.ApplicationIdle);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
    }
}
