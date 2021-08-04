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
    /// DDCVVerViewScnVerLogEventBanInfoSubCard.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVVerViewScnVerLogEventBanInfoSubCard : UserControl
    {

        public static readonly DependencyProperty SummaryProperty = DependencyProperty.Register("Summary", typeof(DDCVVerScnVerLogBanSubSummary), typeof(DDCVVerViewScnVerLogEventBanInfoSubCard));
        public DDCVVerScnVerLogBanSubSummary Summary
        {
            set { SetValue(SummaryProperty, value); }
            get { return (DDCVVerScnVerLogBanSubSummary)GetValue(SummaryProperty); }
        }

        public DDCVBannerInfo Banner;

        public DDCVVerViewScnVerLogEventBanInfoSubCard()
        {
            InitializeComponent();
        }


        public void LoadBanner(DDCVBannerInfo info)
        {
            int cnt = 0, inheritcnt = 0, roundcnt = 1;
            int r5cnt = 0, r4cnt = 0, r5inhcnt = 0, r4inhcnt = 0, r5upcnt = 0, r4upcnt = 0;
            bool epconformed = false;
            int epcnt = 0;


            foreach(var inr in info.Inherited)
            {
                var logs = inr.Logs.L;
                if (logs.Count == 0) continue;
                inheritcnt += logs.Count;

                var unit = logs.Last();
                if (unit.rank == 5) 
                {
                    r5inhcnt++;
                }

                r4inhcnt += logs.FindAll(x => x.rank == 4).Count;

            }
            foreach(var rnd in info.Logs.R)
            {
                cnt += rnd.L.Count;
                if (rnd.L.Count == 0) continue;
                var unit = rnd.L[rnd.L.Count - 1];
                if (unit.rank == 5) 
                {
                    r5cnt++;
                    if (info.Info.rank5Up.Contains(unit.unitclass))
                    {
                        r5upcnt++;

                        // TODO: 神铸定轨 设置检查

                        if (info.Info.epitomizedPathEnabled)
                        {
                            if (rnd.epitomizedPathID != 0)
                            {
                                epconformed = true;
                                if (rnd.epitomizedPathID == unit.unitclass) epcnt++;
                            }
                        }
                        roundcnt++;
                    }
                    r4cnt += rnd.L.FindAll(x => x.rank == 4).Count;
                    r4upcnt += rnd.L.FindAll(x => info.Info.rank4Up.Contains(x.unitclass)).Count;
                }
            }
            string upunit = "";
            foreach (var r5up in info.Info.rank5Up)
            {
                upunit += string.Format("{0} ", DDCL.Units.getItemById(r5up).name);
            }
            upunit += "|";
            foreach (var r4up in info.Info.rank4Up)
            {
                upunit += string.Format(" {0}", DDCL.Units.getItemById(r4up).name);
            }

            DDCVVerScnVerLogBanSubSummary summary = new DDCVVerScnVerLogBanSubSummary
            {
                Title = String.Format("{0}", info.Info.name),
                Subtitle = String.Format("{0} - {1} - {2} - {3}", info.VersionInfo.version, DDCL.ConvertPoolTypeToName(info.Info.type), info.Info.name, info.Info.hint),
                BannerTime = String.Format("{0:G} - {1:G}", DDCL.GetLibraryTimeOffset(info.Info.beginTime).ToLocalTime(), DDCL.GetLibraryTimeOffset(info.Info.endTime).ToLocalTime()),
                BannerUpUnits = upunit,
                InheritedCnt = inheritcnt.ToString(),
                Cnt = cnt.ToString(),
                RoundCnt = roundcnt.ToString(),
                TotalCnt = (inheritcnt + cnt).ToString(),
                R5Cnt = String.Format("{0}/{1}", r5cnt, r5cnt + r5inhcnt),
                R4Cnt = String.Format("{0}/{1}", r4cnt, r4cnt + r4inhcnt),
                R5PS = (inheritcnt + cnt) == 0 ? "[—%]" : String.Format("[{0:P1}]", (r5cnt + r5inhcnt) * 1.0 / (inheritcnt + cnt)),
                R4PS = (inheritcnt + cnt) == 0 ? "[—%]" : String.Format("[{0:P1}]", (r4cnt + r4inhcnt) * 1.0 / (inheritcnt + cnt)),
                R5Up = r5upcnt.ToString(),
                R4Up = r4upcnt.ToString()
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
