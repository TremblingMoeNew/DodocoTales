using DodocoTales.Library;
using DodocoTales.Logs;
using Newtonsoft.Json;
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            DDCLog.InitHint();
            await DDCL.BannerLib.LoadLibraryAsync();
            await DDCL.UnitLib.LoadLibraryAsync();
            await DDCL.UserDataLib.LoadLocalGachaLogsAsync();
            DDCL.CurrentUser.SwapUser(178450343);
            //DDCLog.Info(DCLN.Debug, JsonConvert.SerializeObject(DDCL.CurrentUser.GreaterRounds,Formatting.Indented));
        }
    }
}
