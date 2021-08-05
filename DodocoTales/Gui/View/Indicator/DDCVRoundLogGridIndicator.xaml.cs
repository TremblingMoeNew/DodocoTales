using DodocoTales.Gui.Enums;
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

namespace DodocoTales.Gui.View.Indicator
{
    /// <summary>
    /// DDCVRoundLogGridIndicator.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVRoundLogGridIndicator : UserControl
    {
        public List<DDCVGridIndicatorUnit> Indicators;

        public DDCVRoundLogGridIndicator()
        {
            InitializeComponent();
            Indicators = new List<DDCVGridIndicatorUnit>();

            InitializeIndicators(DDCVGridIndicatorVolume.Permanent);
        }

        public void InitializeIndicators(DDCVGridIndicatorVolume volume)
        {
            Main.Children.Clear();
            Indicators.Clear();
            int totalcnt = 0, colvolume = 0;
            switch (volume)
            {
                case DDCVGridIndicatorVolume.Permanent:
                    totalcnt = 90;
                    colvolume = 2;
                    break;
                case DDCVGridIndicatorVolume.EventCharacterNormal:
                    totalcnt = 180;
                    colvolume = 5;
                    break;
                case DDCVGridIndicatorVolume.EventWeaponNormal:
                    totalcnt = 160;
                    colvolume = 4;
                    break;
                case DDCVGridIndicatorVolume.EventWeaponEP:
                    totalcnt = 240;
                    colvolume = 5;
                    break;
                case DDCVGridIndicatorVolume.EventWeaponEPExtended:
                    totalcnt = 320;
                    colvolume = 7;
                    break;
            }
            StackPanel vpanel = null;
            for (int i = 0; i < totalcnt; i++) 
            {
                if (i % colvolume == 0)
                {
                    vpanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };
                    Main.Children.Add(vpanel);
                }
                var indicator = new DDCVGridIndicatorUnit
                {
                    Id = i + 1
                };
                Indicators.Add(indicator);
                vpanel.Children.Add(indicator);
            }
        }
    }
}
