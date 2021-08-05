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
    /// DDCVGridIndicatorUnit.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVGridIndicatorUnit : UserControl
    {
        public static readonly DependencyProperty InheritedProperty = DependencyProperty.Register("Inherited", typeof(bool), typeof(DDCVGridIndicatorUnit));
        public bool Inherited
        {
            set { SetValue(InheritedProperty, value); }
            get { return (bool)GetValue(InheritedProperty); }
        }

        public static readonly DependencyProperty UnitTypeProperty = DependencyProperty.Register("UnitType", typeof(DDCVIndicatorUnitType), typeof(DDCVGridIndicatorUnit));
        public DDCVIndicatorUnitType UnitType
        {
            set { SetValue(UnitTypeProperty, value); }
            get { return (DDCVIndicatorUnitType)GetValue(UnitTypeProperty); }
        }
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(DDCVGridIndicatorUnit));
        public int Id
        {
            set { SetValue(IdProperty, value); }
            get { return (int)GetValue(IdProperty); }
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(DDCVGridIndicatorUnit));
        public string Hint
        {
            set { SetValue(HintProperty, value); }
            get { return (string)GetValue(HintProperty); }
        }


        private void Indicator_MouseEnter(object sender, MouseEventArgs e)
        {
            Pop.IsOpen = true;
        }

        private void Indicator_MouseLeave(object sender, MouseEventArgs e)
        {
            Pop.IsOpen = false;
        }




        public DDCVGridIndicatorUnit()
        {
            InitializeComponent();
            //Inherited = true;
            UnitType = DDCVIndicatorUnitType.Default;
        }

    }
}
