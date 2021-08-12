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
    /// DDCVRoundLogProgressIndicator.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVRoundLogProgressIndicator : UserControl
    {
        public static readonly DependencyProperty InheritedPermanentProperty = DependencyProperty.Register("InheritedPermanent", typeof(double), typeof(DDCVRoundLogProgressIndicator));
        public double InheritedPermanent
        {
            set { SetValue(InheritedPermanentProperty, value); }
            get { return (double)GetValue(InheritedPermanentProperty); }
        }
        public static readonly DependencyProperty PermanentProperty = DependencyProperty.Register("Permanent", typeof(double), typeof(DDCVRoundLogProgressIndicator));
        public double Permanent
        {
            set { SetValue(PermanentProperty, value); }
            get { return (double)GetValue(PermanentProperty); }
        }
        public static readonly DependencyProperty InheritedProperty = DependencyProperty.Register("Inherited", typeof(double), typeof(DDCVRoundLogProgressIndicator));
        public double Inherited
        {
            set { SetValue(InheritedProperty, value); }
            get { return (double)GetValue(InheritedProperty); }
        }
        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register("Current", typeof(double), typeof(DDCVRoundLogProgressIndicator));
        public double Current
        {
            set { SetValue(CurrentProperty, value); }
            get { return (double)GetValue(CurrentProperty); }
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(DDCVRoundLogProgressIndicator));
        public string Hint
        {
            set { SetValue(HintProperty, value); }
            get { return (string)GetValue(HintProperty); }
        }

        public DDCVRoundLogProgressIndicator()
        {
            InitializeComponent();
            InheritedPermanent = 0.2;
            Inherited = 0.1;
            Permanent = 0.3;
            Current = 0.2;
        }

        public void LoadInfo(DDCVIndicatorVolume volume,int inherit,int permanent,int total)
        {
            int ip = (permanent > inherit) ? 0 : permanent;
            int i = (permanent > inherit) ? inherit : inherit - permanent;
            int p = (permanent > inherit) ? permanent - inherit : 0;
            int c = total - ip - i - p;
            int v = (int)volume;
            Action act = () =>
            {
                InheritedPermanent = ip * 1.0 / v;
                Inherited = i * 1.0 / v;
                Permanent = p * 1.0 / v;
                Current = c * 1.0 / v;
                Hint = String.Format(
                    "抽数: {0}/{1} \n 继承抽数: {2} \n 距离上一五星: {3}",
                    total,
                    v,
                    inherit,
                    total-permanent
                );
            };
            Dispatcher.BeginInvoke(act);
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Pop.IsOpen = true;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            Pop.IsOpen = false;
        }
    }
}
