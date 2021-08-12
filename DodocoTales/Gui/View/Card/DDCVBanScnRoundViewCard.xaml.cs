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
            DDCVIndicatorVolume volume = DDCVIndicatorVolume.Permanent;
            switch(round.BannerInfo.type)
            {
                case DDCCPoolType.Beginner:
                case DDCCPoolType.Permanent:
                    volume = DDCVIndicatorVolume.Permanent;
                    break;
                case DDCCPoolType.EventCharacter:
                    volume = DDCVIndicatorVolume.EventCharacterNormal;
                    break;
                case DDCCPoolType.EventWeapon:
                    volume = DDCVIndicatorVolume.EventWeaponNormal;
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
            List<DDCVUnitTextIndicatorInfo> R5Info = new List<DDCVUnitTextIndicatorInfo>();
            List<DDCVUnitTextIndicatorInfo> R4Info = new List<DDCVUnitTextIndicatorInfo>();
            int curr5cnt = 0;
            if (round.Inherited != null)
            {
                foreach (var inh in round.Inherited)
                {
                    var logs = inh.Logs.L;
                    inheritcnt += logs.Count;
                    curr5cnt += logs.Count;
                    if (logs.Count > 0 && logs.Last().rank == 5) curr5cnt = 0;
                }
            }
            foreach (var rnd in round.Logs)
            {
                var logs = rnd.L;
                cnt += logs.Count;
                curr5cnt += logs.Count;
                if (logs.Count > 0 && logs.Last().rank == 5)
                {
                    R5Info.Add(new DDCVUnitTextIndicatorInfo { name = logs.Last().name, time = logs.Last().time, cnt = curr5cnt });
                    curr5cnt = 0;
                }
                foreach (var r4 in logs.FindAll(x => x.rank == 4)) 
                {
                    R4Info.Add(new DDCVUnitTextIndicatorInfo { name = r4.name, time = r4.time });
                }
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
            Action act2 = () => { ApplyR5Indicators(R5Info); ApplyR4Indicators(R4Info); };
            await Dispatcher.BeginInvoke(act2, DispatcherPriority.ApplicationIdle);
        }
        public void ApplyR5Indicators(List<DDCVUnitTextIndicatorInfo> R5Info)
        {
            R5Info.Reverse();
            R5IndicatorContainer.Children.Clear();
            foreach (var info in R5Info)
            {
                R5IndicatorContainer.Children.Add(new TextBlock
                {
                    Text = String.Format("{0}[{1}]", info.name, info.cnt)
                });
            }
        }
        public void ApplyR4Indicators(List<DDCVUnitTextIndicatorInfo> R4Info)
        {
            R4Info.Reverse();
            R4IndicatorContainer.Children.Clear();
            foreach (var info in R4Info)
            {
                R4IndicatorContainer.Children.Add(new TextBlock
                {
                    Text = String.Format("{0} /", info.name)
                });
            }
        }
        public async void ApplyGridIndicators(DDCVRoundInfo round)
        {
            Action act = () => { GridIndicator.LoadRoundInfo(round); };
            await Dispatcher.BeginInvoke(act, DispatcherPriority.ApplicationIdle);
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
