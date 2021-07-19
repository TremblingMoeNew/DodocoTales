using DodocoTales.Gui.View.Dialog;
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

namespace DodocoTales.Gui.View
{
    /// <summary>
    /// DDCVMainMenuPanel.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVMainMenuPanel : UserControl
    {
        public DDCVMainMenuPanel()
        {
            InitializeComponent();
            DDCL.Banners.loadLibrary();
            DDCL.Units.loadLibrary();
            DDCL.Users.loadLocalGachaLogs();
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DDCVWebGachaLogLoadIndicatorDialog
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            }.ShowDialog();
        }
    }
}
