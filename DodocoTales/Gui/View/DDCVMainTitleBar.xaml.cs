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

namespace DodocoTales.Gui.View
{
    /// <summary>
    /// DDCVMainTitleBar.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVMainTitleBar : UserControl
    {
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(WindowState), typeof(DDCVMainTitleBar));
        public WindowState State
        {
            set { SetValue(StateProperty, value); }
            get { return (WindowState)GetValue(StateProperty); }
        }
        public DDCVMainTitleBar()
        {
            InitializeComponent();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            DDCV.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (DDCV.MainWindow.WindowState == WindowState.Normal) DDCV.MainWindow.WindowState = WindowState.Maximized;
            else DDCV.MainWindow.WindowState = WindowState.Normal;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
