using DodocoTales.Common.Enums;
using DodocoTales.Library;
using DodocoTales.Loader;
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

namespace DodocoTales.Gui.View.Dialog
{
    /// <summary>
    /// DDCVWebGachaLogLoadIndicator.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVWebGachaLogLoadIndicatorDialog : Window
    {
        public DDCVWebGachaLogLoadIndicatorDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWebGachaLog();
        }
        private async void LoadWebGachaLog()
        {

            var key=DDCG.WebLogLoader.getAuthKey();
            if (key == null)
            {
                // Temp
                Console.WriteLine("Authkey Notfound");
                this.Close();
                return;
            }
            var uid = await DDCG.WebLogLoader.tryConnectAndGetUid(key);
            if (uid < 0) 
            {
                // Temp
                Console.WriteLine("UID Notfound");
                this.Close();
                return;
            }
            // Temp
            await DDCL.Users.swapUser(uid);

            var usr=DDCL.Users.getCurrentUser();

            //Temp
            Console.WriteLine(usr.V.Count);

            // Temp
            usr.zone = DDCCTimeZone.DefaultUTCP8;

            var initlogs=await DDCG.WebLogLoader.GetGachaLogsAsync(key);
            DDCG.LogMerger.mergeGachaLogs(initlogs);

            await DDCL.Users.saveCurrentUser();
            this.Close();
        }
    }
}
