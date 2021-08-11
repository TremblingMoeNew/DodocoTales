using DodocoTales.Common;
using DodocoTales.Library;
using DodocoTales.Loader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace DodocoTales.Gui.View.Dialog
{
    /// <summary>
    /// DDCVMetaUpdateIndicatorDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVMetaUpdateIndicatorDialog : Window
    {
        public static readonly DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(DDCVMetaUpdateIndicatorDialog));
        public string Hint
        {
            set { SetValue(HintProperty, value); }
            get { return (string)GetValue(HintProperty); }
        }
        public DDCVMetaUpdateIndicatorDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckMetaUpdates();
        }

        public async void CheckMetaUpdates()
        {
            Action initact = () => {  Hint = "正在检查更新……"; };
            Action connfailedact = () => { Hint = "更新失败：请检查您的网络连接"; };
            Action delayclosedialogact = () => { delayedCloseDialog(); };
            await Dispatcher.BeginInvoke(initact);

            var newestver = await DDCG.MetaLoader.FetchNewestVersionData();
            if (newestver == null)
            {
                await Dispatcher.BeginInvoke(connfailedact);
                delayclosedialogact.BeginInvoke(null, null);
                return;
            }
            var libexists = await DDCL.MetaVersion.LoadLibraryAsync();
            DDCL.MetaVersion.ConvertClientVerToInt64(newestver.ClientVersion);
            if (!libexists || DDCL.MetaVersion.ConvertClientVerToInt64(DDCL.MetaVersion.ClientVersion) < DDCL.MetaVersion.ConvertClientVerToInt64(newestver.ClientVersion)) 
            {
                Action newclientver = () => { Hint = String.Format("检测到新版本:{0}，正在获取更新……",newestver.ClientVersion); };
                await Dispatcher.BeginInvoke(newclientver);
                Action updatefailedact = () => { Hint = "错误: 更新失败"; };
                Action delayquitact = () => { delayedQuitProgram(); };
                
                if (!await DDCG.ClientUpdater.DownloadClient())
                {
                    await Dispatcher.BeginInvoke(updatefailedact);
                    delayquitact.BeginInvoke(null, null);
                    return;
                }
                if (!DDCG.ClientUpdater.ExtractClient())
                {
                    await Dispatcher.BeginInvoke(updatefailedact);
                    delayquitact.BeginInvoke(null, null);
                    return;
                }
                if (!DDCG.ClientUpdater.MoveUpdater())
                {
                    await Dispatcher.BeginInvoke(updatefailedact);
                    delayquitact.BeginInvoke(null, null);
                    return;
                }
                DDCG.ClientUpdater.ExecUpdater();
                Application.Current.Shutdown();
                return;

            }
            if (DDCL.MetaVersion.ConvertLibVerToInt64(DDCL.MetaVersion.UnitLibraryVersion) < DDCL.MetaVersion.ConvertLibVerToInt64(newestver.UnitLibraryVersion)) 
            {
                if(!await DDCG.MetaLoader.UpdateUnitLib())
                {
                    await Dispatcher.BeginInvoke(connfailedact);
                    delayclosedialogact.BeginInvoke(null, null);
                    return;
                }
            }
            if (DDCL.MetaVersion.ConvertLibVerToInt64(DDCL.MetaVersion.BannerLibraryVersion) < DDCL.MetaVersion.ConvertLibVerToInt64(newestver.BannerLibraryVersion)) 
            {
                if (!await DDCG.MetaLoader.UpdateBannerLib())
                {
                    await Dispatcher.BeginInvoke(connfailedact);
                    delayclosedialogact.BeginInvoke(null, null);
                    return;
                }
            }

            if (!DDCL.Units.loadLibrary())
            {
                if (!await DDCG.MetaLoader.UpdateUnitLib())
                {
                    await Dispatcher.BeginInvoke(connfailedact);
                    delayclosedialogact.BeginInvoke(null, null);
                    return;
                }
                if (!DDCL.Units.loadLibrary())
                {
                    Action unitlibdamagedact = () => { Hint = "更新失败：单位数据库损坏"; };
                    await Dispatcher.BeginInvoke(unitlibdamagedact);
                    delayclosedialogact.BeginInvoke(null, null);
                    return;
                }
            }
            if(!DDCL.Banners.loadLibrary())
            {
                if (!await DDCG.MetaLoader.UpdateBannerLib())
                {
                    await Dispatcher.BeginInvoke(connfailedact);
                    delayclosedialogact.BeginInvoke(null, null);
                    return;
                }
                if (!DDCL.Banners.loadLibrary())
                {
                    Action bannerlibdamagedact = () => { Hint = "更新失败：卡池数据库损坏"; };
                    await Dispatcher.BeginInvoke(bannerlibdamagedact);
                    delayclosedialogact.BeginInvoke(null, null);
                    return;
                }
            }
            DDCG.MetaLoader.MarkUpdate();

            if(!DDCL.Settings.IsLoaded())
            {
                await DDCL.Settings.LoadSettingsAsync();
                Action loadinguserlibact = () => { Hint = "正在载入本地用户数据……"; };
                await Dispatcher.BeginInvoke(loadinguserlibact);
                await DDCL.Users.loadLocalGachaLogs();
                if(DDCL.Users.userExists(DDCL.Settings.LastLoadedUid))
                {
                    await DDCL.Users.swapUser(Convert.ToInt64(DDCL.Settings.LastLoadedUid));
                    if (DDCS.UidReloadCompleted != null)
                        foreach (DDCSCommonDelegate method in DDCS.UidReloadCompleted.GetInvocationList())
                        {
                            method.BeginInvoke(null, null);
                        }
                    if (DDCS.LogReloadCompleted != null)
                        foreach (DDCSCommonDelegate method in DDCS.LogReloadCompleted.GetInvocationList())
                        {
                            method.BeginInvoke(null, null);
                        }
                }
            }

            this.Close();
        }

        private void delayedCloseDialog()
        {
            Thread.Sleep(3000);
            Action act = () => { this.Close(); };
            Dispatcher.BeginInvoke(act);
        }

        private void delayedQuitProgram()
        {
            Thread.Sleep(5000);
            Action act = () => { Application.Current.Shutdown(); };
            Dispatcher.BeginInvoke(act);
        }
    }
}
