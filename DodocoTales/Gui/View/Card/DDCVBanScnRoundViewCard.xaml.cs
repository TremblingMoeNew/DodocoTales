using DodocoTales.Common.Enums;
using DodocoTales.Gui.Enums;
using DodocoTales.Gui.Model;
using DodocoTales.Gui.View.Indicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// DDCVBanScnRoundViewCard.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVBanScnRoundViewCard : UserControl
    {
        public static readonly DependencyProperty SummaryProperty = DependencyProperty.Register("Summary", typeof(DDCVBanScnRoundCardSummary), typeof(DDCVBanScnRoundViewCard));
        public DDCVBanScnRoundCardSummary Summary
        {
            set { SetValue(SummaryProperty, value); }
            get { return (DDCVBanScnRoundCardSummary)GetValue(SummaryProperty); }
        }

        DDCVRoundLogGridIndicator GridIndicator;
        bool GridIndicatorLoaded = false;

        public DDCVBanScnRoundViewCard()
        {
            InitializeComponent();
            GridIndicator = new DDCVRoundLogGridIndicator();
        }



        public async void LoadRound(DDCVRoundInfo round)
        {
            DDCVGridIndicatorVolume volume = DDCVGridIndicatorVolume.Permanent;
            switch(round.BannerInfo.type)
            {
                case DDCCPoolType.Beginner:
                case DDCCPoolType.Permanent:
                    volume = DDCVGridIndicatorVolume.Permanent;
                    break;
                case DDCCPoolType.EventCharacter:
                    volume = DDCVGridIndicatorVolume.EventCharacterNormal;
                    break;
                case DDCCPoolType.EventWeapon:
                    volume = DDCVGridIndicatorVolume.EventWeaponNormal;
                    break;
            }
            LoadSummary(round);
            Action act = () => { GridIndicator.InitializeIndicators(volume); };
            await Dispatcher.BeginInvoke(act);
            ApplyGridIndicators(round);
        }

        public async void LoadSummary(DDCVRoundInfo round)
        {
            int inheritcnt = 0, cnt = 0, totalcnt = 0;
            if (round.Inherited != null)
            {
                foreach (var inh in round.Inherited)
                {
                    var logs = inh.Logs.L;
                    inheritcnt += logs.Count;
                }
            }
            foreach (var rnd in round.Logs)
            {
                var logs = rnd.L;
                cnt += logs.Count;
            }
            totalcnt = cnt + inheritcnt;
            var summary = new DDCVBanScnRoundCardSummary
            {
                Title = String.Format("轮次 {0}", round.Index),
                InheritedCnt = inheritcnt.ToString(),
                Cnt = cnt.ToString(),
                TotalCnt = totalcnt.ToString()
            };

            Action act = () => { Summary = summary; };
            await Dispatcher.BeginInvoke(act);
        }
        public void ApplyR5Indicators()
        {

        }
        public void ApplyR4Indicators()
        {

        }
        public async void ApplyGridIndicators(DDCVRoundInfo round)
        {

        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            GridIndicatorContainer.Visibility = Visibility.Visible;
            if(!GridIndicatorLoaded)
            {
                LoadGridIndicatorToUI();
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            GridIndicatorContainer.Visibility = Visibility.Collapsed;
            
        }

        private async void LoadGridIndicatorToUI()
        {
            GridIndicatorLoaded = true;
            Thread.SpinWait(100);
            Action act = () =>
            {
                GridIndicatorContainer.Children.Add(GridIndicator);
                GILoadingHint.Visibility = Visibility.Hidden;
            };
            await Dispatcher.BeginInvoke(act, DispatcherPriority.ApplicationIdle);
        }
        
    }
}
