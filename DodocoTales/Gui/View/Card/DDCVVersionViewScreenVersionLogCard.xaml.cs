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
            DDCVVerScnVerLogSummary summary = new DDCVVerScnVerLogSummary
            {
                Title = String.Format("{0} - {1}", versionInfo.Info.version, versionInfo.Info.name),
                VersionTime = String.Format("{0:G} - {1:G}", DDCL.GetLibraryTimeOffset(versionInfo.Info.beginTime).ToLocalTime(), DDCL.GetLibraryTimeOffset(versionInfo.Info.endTime).ToLocalTime()),
            };
            Action act = () => { Summary = summary; };
            Dispatcher.BeginInvoke(act);
        }
    }
}
