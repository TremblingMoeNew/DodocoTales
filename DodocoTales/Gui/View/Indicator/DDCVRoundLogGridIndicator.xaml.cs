using DodocoTales.Common.Enums;
using DodocoTales.Gui.Enums;
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

            //InitializeIndicators(DDCVGridIndicatorVolume.EventCharacterNormal);
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
            StackPanel hPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Orientation = Orientation.Horizontal
            };
            StackPanel vpanel = null;
            for (int i = 0; i < totalcnt; i++) 
            {
                if (i % colvolume == 0)
                {
                    vpanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };
                    hPanel.Children.Add(vpanel);
                }
                var indicator = new DDCVGridIndicatorUnit
                {
                    Id = i + 1
                };
                Indicators.Add(indicator);
                vpanel.Children.Add(indicator);
            }
            Main.Children.Add(hPanel);
        }

        public void LoadRoundInfo(DDCVRoundInfo Info)
        {
            int idx = 0;
            var timezone = DDCL.Users.getCurrentUser().zone;
            if (Info.Inherited != null) 
            {
                foreach (var inh in Info.Inherited)
                {
                    foreach (var unit in inh.Logs.L)
                    {
                        Indicators[idx].Inherited = true;
                        string hint = "";
                        if (unit.rank == 3)
                        {
                            Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank3;
                            hint = "往期单位/三星";

                        }
                        else if (unit.rank == 4)
                        {
                            if (inh.banner.rank4Up.Contains(unit.unitclass))
                            {
                                Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank4Up;
                                hint = "往期单位/当期四星";
                            }
                            else if (unit.unittype == DDCCUnitType.Character) 
                            {
                                Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank4PerChar;
                                hint = "往期单位/常驻四星角色";
                            }
                            else if (unit.unittype == DDCCUnitType.Weapon)
                            {
                                Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank4PerWep;
                                hint = "往期单位/常驻四星武器";
                            }
                        }
                        else if (unit.rank == 5)
                        {
                            if (inh.banner.rank5Up.Contains(unit.unitclass))
                            {
                                Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank5Up;
                                hint = "往期单位/当期五星";
                            }
                            else if (unit.unittype == DDCCUnitType.Character)
                            {
                                Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank5PerChar;
                                hint = "往期单位/常驻五星角色";
                            }
                            else if (unit.unittype == DDCCUnitType.Weapon)
                            {
                                Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank5PerWep;
                                hint = "往期单位/常驻五星武器";
                            }
                        }
                        Indicators[idx].Hint = String.Format(
                            "{0}\n{1}\n时间: {2:G}\n {3} - {4}",
                            unit.name,
                            hint,
                            DDCL.GetUserTimezoneTimeOffset(unit.time).ToLocalTime(),
                            inh.version.version,
                            inh.banner.name
                        );
                        idx++;
                    }
                }
            }
            foreach (var rnd in Info.Logs)
            {
                foreach (var unit in rnd.L)
                {
                    Indicators[idx].Inherited = false;
                    string hint = "";
                    if (unit.rank == 3)
                    {
                        Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank3;
                        hint = "三星单位";

                    }
                    else if (unit.rank == 4)
                    {
                        if (Info.BannerInfo.rank4Up.Contains(unit.unitclass))
                        {
                            Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank4Up;
                            hint = "当期四星";
                        }
                        else if (unit.unittype == DDCCUnitType.Character)
                        {
                            Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank4PerChar;
                            hint = "常驻四星角色";
                        }
                        else if (unit.unittype == DDCCUnitType.Weapon)
                        {
                            Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank4PerWep;
                            hint = "常驻四星武器";
                        }
                    }
                    else if (unit.rank == 5)
                    {
                        if (Info.BannerInfo.rank5Up.Contains(unit.unitclass))
                        {
                            Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank5Up;
                            hint = "当期五星";
                        }
                        else if (unit.unittype == DDCCUnitType.Character)
                        {
                            Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank5PerChar;
                            hint = "常驻五星角色";
                        }
                        else if (unit.unittype == DDCCUnitType.Weapon)
                        {
                            Indicators[idx].UnitType = DDCVIndicatorUnitType.Rank5PerWep;
                            hint = "常驻五星武器";
                        }
                    }
                    Indicators[idx].Hint = String.Format(
                        "{0}\n{1}\n时间: {2:G}",
                        unit.name,
                        hint,
                        DDCL.GetUserTimezoneTimeOffset(unit.time).ToLocalTime()
                    );
                    idx++;
                }
            }
        }
    }
}
