using DodocoTales.Common.Enums;
using DodocoTales.Gui;
using DodocoTales.Gui.Models;
using DodocoTales.Library;
using DodocoTales.Loader;
using DodocoTales.Loader.Models;
using DodocoTales.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            DDCV.MainNavigater = MainNavigator;
            DDCV.RegisterMainScreens();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            DDCLog.InitHint();
            DDCG.Initialize();
            await DDCL.BannerLib.LoadLibraryAsync();
            await DDCL.UnitLib.LoadLibraryAsync();
            await DDCL.UserDataLib.LoadLocalGachaLogsAsync();
            DDCL.CurrentUser.SwapUser(0);
            DDCV.RefreshAll();
            //HomeScn.Refresh();
            //BanViewScn.SetBanner(201, 201101);
            //BanViewScn.SetBanner(202, 202102);
            //BanViewScn.Refresh();
            //Card.Refresh();
           // Card2.Refresh();
            ///Card3.Refresh();
            //DDCLog.Info(DCLN.Debug, JsonConvert.SerializeObject(DDCL.CurrentUser.GreaterRounds,Formatting.Indented));
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var authkey = DDCG.WebLogLoader.GetAuthKey();
            DDCL.CurrentUser.SwapUser(7);
            if (authkey != null)
            {
                await DDCG.WebLogLoader.GetGachaLogsAsNormalMode(authkey);
                await DDCL.CurrentUser.SaveUserAsync();
            }
            DDCV.RefreshAll();
            //Card.Refresh();
            //Card2.Refresh();
            //BanViewScn.Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DDCG.UFExporter.ExportAsJson(1,"export");
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var uf = await DDCG.UFImporter.LoadUFJsonAsync("import/xunkong.json");
            DDCG.UFImporter.Import(uf);
        }

        private void MainPanel_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = MainPanel.SelectedItem as DDCVMainPanelItemModel;
            if(item != null)
                DDCV.SwapMainScreen(item.Tag);
        }

        private void TreeViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as Label).DataContext as DDCVMainPanelItemModel;
            if (item == MainPanel.SelectedItem)
                DDCV.SwapMainScreen(item.Tag);
        }
    }
}
