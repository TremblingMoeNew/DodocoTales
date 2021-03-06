using DodocoTales.Gui;
using DodocoTales.Gui.View.Dialog;
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

namespace DodocoTales
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowMain_StateChanged(object sender, EventArgs e)
        {
            Caption.State = WindowState;
        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            DDCV.MainWindow = this;
            new DDCVMetaUpdateIndicatorDialog
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = DDCV.MainWindow
            }.ShowDialog();
        }
    }
}
