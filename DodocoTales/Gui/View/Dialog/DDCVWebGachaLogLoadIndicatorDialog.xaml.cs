using DodocoTales.Common;
using DodocoTales.Common.Enums;
using DodocoTales.Library;
using DodocoTales.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(DDCVWebGachaLogLoadIndicatorDialog));
        public string Hint
        {
            set { SetValue(HintProperty, value); }
            get { return (string)GetValue(HintProperty); }
        }

        public static readonly DependencyProperty OnAskTimezoneProperty = DependencyProperty.Register("OnAskTimezone", typeof(bool), typeof(DDCVWebGachaLogLoadIndicatorDialog));
        public bool OnAskTimezone
        {
            set { SetValue(OnAskTimezoneProperty, value); }
            get { return (bool)GetValue(OnAskTimezoneProperty); }
        }

        string authkey;

        public DDCVWebGachaLogLoadIndicatorDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWebGachaLogInit();
        }
        private async void LoadWebGachaLogInit()
        {
            Action initact = () => { OnAskTimezone = false; Hint = "正在加载祈愿记录……"; };
            await Dispatcher.BeginInvoke(initact);

            var key=DDCG.WebLogLoader.getAuthKey();
            if (key == null)
            {
                // Temp
                Console.WriteLine("Authkey Notfound");
                Action act1 = () => { Hint = "未找到URL，请确认最后一次登录游戏时是否打开过祈愿记录"; };
                await Dispatcher.BeginInvoke(act1);

                Action act2 = () => { delayedCloseDialog(); };
                act2.BeginInvoke(null, null);

                
               
                //this.Close();
                return;
            }
            var uid = await DDCG.WebLogLoader.tryConnectAndGetUid(key);
            if (uid < 0) 
            {
                // Temp
                Console.WriteLine("UID Notfound");
                Action act1 = () => { Hint = "未找到用户UID，请检查网络连接并确认该用户进行过祈愿"; };
                await Dispatcher.BeginInvoke(act1);

                Action act2 = () => { delayedCloseDialog(); };
                act2.BeginInvoke(null, null);
                //this.Close();
                return;
            }
            // Temp
            await DDCL.Users.swapUser(uid);

            if (DDCS.UidReloadCompleted != null)
                foreach (DDCSCommonDelegate method in DDCS.UidReloadCompleted.GetInvocationList())
                {
                    method.BeginInvoke(null, null);
                }

            authkey = key;

            var usr = DDCL.Users.getCurrentUser();

            if (usr.V.Count == 0 && usr.Uncategorized == null) 
            {
                Action act = () => { OnAskTimezone = true; };
                Dispatcher.BeginInvoke(act);
            }
            else
            {
                LoadWebGachaLogFinal();
            }
        }

        private async void LoadWebGachaLogFinal()
        {
            var key = authkey;
            var initlogs = await DDCG.WebLogLoader.GetGachaLogsAsync(key);
            DDCG.LogMerger.mergeGachaLogs(initlogs);

            await DDCL.Users.saveCurrentUser();

            if (DDCS.LogReloadCompleted != null)
                foreach (DDCSCommonDelegate method in DDCS.LogReloadCompleted.GetInvocationList())
                {
                    method.BeginInvoke(null, null);
                }
            this.Close();
        }

        private void delayedCloseDialog()
        {
            Thread.Sleep(1500);
            Action act = () => { this.Close(); };
            Dispatcher.BeginInvoke(act);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            DDCCTimeZone zone = (DDCCTimeZone)Enum.Parse(typeof(DDCCTimeZone), (string)btn.Tag);

            OnAskTimezone = false;
            var usr = DDCL.Users.getCurrentUser();
            usr.zone = zone;
            LoadWebGachaLogFinal();
        }
    }
}
