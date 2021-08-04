using DodocoTales.Common.Enums;
using DodocoTales.Gui.Model;
using DodocoTales.Gui.View.Screen;
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

namespace DodocoTales.Gui.View.Card
{
    /// <summary>
    /// DDCVVerViewScnVerLogPerBanInfoSubCard.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVVerViewScnVerLogPerBanInfoSubCard : UserControl
    {

        public static readonly DependencyProperty SummaryProperty = DependencyProperty.Register("Summary", typeof(DDCVVerScnVerLogBanSubSummary), typeof(DDCVVerViewScnVerLogPerBanInfoSubCard));
        public DDCVVerScnVerLogBanSubSummary Summary
        {
            set { SetValue(SummaryProperty, value); }
            get { return (DDCVVerScnVerLogBanSubSummary)GetValue(SummaryProperty); }
        }

        public DDCVBannerInfo Banner;
        public DDCVVerViewScnVerLogPerBanInfoSubCard()
        {
            InitializeComponent();
        }

        public void LoadBanner(DDCVBannerInfo info)
        {

            int cnt = 0, inheritcnt = 0, roundcnt = 1;
            int r5cnt = 0, r5inhcnt = 0, r4cnt = 0, r4inhcnt = 0, r5chrcnt = 0, r5wepcnt = 0, r4chrcnt = 0, r4wepcnt = 0;

            foreach (var inr in info.Inherited)
            {
                var logs = inr.Logs.L;
                if (logs.Count == 0) continue;
                inheritcnt += logs.Count;
                if (logs.Last().rank == 5)
                {
                    r5inhcnt++;
                    if (logs.Last().unittype == DDCCUnitType.Character) r5chrcnt++; else if (logs.Last().unittype == DDCCUnitType.Weapon) r5wepcnt++;
                }
                var r4chrl = logs.FindAll(x => x.rank == 4 && x.unittype == DDCCUnitType.Character);
                r4inhcnt += r4chrl.Count;
                r4chrcnt += r4chrl.Count;
                var r4wepl = logs.FindAll(x => x.rank == 4 && x.unittype == DDCCUnitType.Weapon);
                r4inhcnt += r4wepl.Count;
                r4wepcnt += r4wepl.Count;
                
            }

            foreach(var rnd in info.Logs.R)
            {
                var logs = rnd.L;
                if (logs.Count == 0) continue;
                cnt += logs.Count;
                if (logs.Last().rank == 5)
                {
                    r5cnt++;
                    if (logs.Last().unittype == DDCCUnitType.Character) r5chrcnt++; else if (logs.Last().unittype == DDCCUnitType.Weapon) r5wepcnt++;
                }
                var r4chrl = logs.FindAll(x => x.rank == 4 && x.unittype == DDCCUnitType.Character);
                r4cnt += r4chrl.Count;
                r4chrcnt += r4chrl.Count;
                var r4wepl = logs.FindAll(x => x.rank == 4 && x.unittype == DDCCUnitType.Weapon);
                r4cnt += r4wepl.Count;
                r4wepcnt += r4wepl.Count;
            }

            DDCVVerScnVerLogBanSubSummary summary = new DDCVVerScnVerLogBanSubSummary
            {
                Title = String.Format("{0}", info.Info.name),
                Subtitle = String.Format("{0} - {1} - {2}", info.VersionInfo.version, DDCL.ConvertPoolTypeToName(info.Info.type), info.Info.name),
                BannerTime = String.Format("{0:G} - {1:G}", DDCL.GetLibraryTimeOffset(info.VersionInfo.beginTime).ToLocalTime(), DDCL.GetLibraryTimeOffset(info.VersionInfo.endTime).ToLocalTime()),
                RoundCnt = String.Format("{0}", roundcnt),
                InheritedCnt = String.Format("{0}", inheritcnt),
                Cnt = String.Format("{0}", cnt),
                TotalCnt = String.Format("{0}", inheritcnt + cnt),
                R5Cnt = String.Format("{0}", r5cnt),
                R4Cnt = String.Format("{0}/{1}", r4cnt, r4cnt + r4inhcnt),
                R5PS = (inheritcnt + cnt) == 0 ? "[—%]" : String.Format("[{0:P1}]", (r5cnt + r5inhcnt) * 1.0 / (inheritcnt + cnt)),
                R4PS = (inheritcnt + cnt) == 0 ? "[—%]" : String.Format("[{0:P1}]", (r4cnt + r4inhcnt) * 1.0 / (inheritcnt + cnt)),
                R5CWR = String.Format("{0}/{1}", r5chrcnt, r5wepcnt),
                R4CWR = String.Format("{0}/{1}", r4chrcnt, r4wepcnt)
            };
            Action act = () => { Summary = summary; };
            Dispatcher.BeginInvoke(act);
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DDCVBannerDetailsScreen newscn = new DDCVBannerDetailsScreen();
            Action act = () => { newscn.LoadBanner(Banner); };
            act.BeginInvoke(null, null);
            DDCV.PushScreen("", newscn);
        }
    }
}
