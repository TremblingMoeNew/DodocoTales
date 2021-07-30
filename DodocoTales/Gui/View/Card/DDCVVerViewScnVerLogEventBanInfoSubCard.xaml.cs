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
            };
            Action act = () => { Summary = summary; };
            Dispatcher.BeginInvoke(act);
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
