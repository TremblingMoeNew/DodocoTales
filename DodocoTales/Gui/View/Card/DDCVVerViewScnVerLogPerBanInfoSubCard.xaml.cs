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






            DDCVVerScnVerLogBanSubSummary summary = new DDCVVerScnVerLogBanSubSummary
            {
                Title = String.Format("{0}", info.Info.name),
                Subtitle = String.Format("{0} - {1} - {2}", info.VersionInfo.version, DDCL.ConvertPoolTypeToName(info.Info.type), info.Info.name),
                BannerTime = String.Format("{0:G} - {1:G}", DDCL.GetLibraryTimeOffset(info.VersionInfo.beginTime).ToLocalTime(), DDCL.GetLibraryTimeOffset(info.VersionInfo.endTime).ToLocalTime()),
            };
            Action act = () => { Summary = summary; };
            Dispatcher.BeginInvoke(act);
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
