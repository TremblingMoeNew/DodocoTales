using DodocoTales.Common;
using DodocoTales.Gui.View.Dialog;
using DodocoTales.Gui.View.Screen;
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

namespace DodocoTales.Gui.View
{
    /// <summary>
    /// DDCVMainMenuPanel.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVMainMenuPanel : UserControl
    {

        public static readonly DependencyProperty UidValProperty = DependencyProperty.Register("UidVal", typeof(long), typeof(DDCVMainMenuPanel));
        public long UidVal
        {
            set { SetValue(UidValProperty, value); }
            get { return (long)GetValue(UidValProperty); }
        }

        public static readonly DependencyProperty UserSwappingProperty = DependencyProperty.Register("UserSwapping", typeof(bool), typeof(DDCVMainMenuPanel));
        public bool UserSwapping
        {
            set { SetValue(UserSwappingProperty, value); }
            get { return (bool)GetValue(UserSwappingProperty); }
        }

        public static readonly DependencyProperty UsersListProperty = DependencyProperty.Register("UsersList", typeof(List<long>), typeof(DDCVMainMenuPanel));
        public List<long> UsersList
        {
            set { SetValue(UsersListProperty, value); }
            get { return (List<long>)GetValue(UsersListProperty); }
        }

        public DDCVMainMenuPanel()
        {
            InitializeComponent();
            UidVal = 0;
            DDCS.UidReloadCompleted += new DDCSCommonDelegate(OnUidSwapping);
            DDCS.LogReloadCompleted += new DDCSCommonDelegate(OnUserSwapCompleted);
            DDCS.UserLibReloadCompleted += new DDCSCommonDelegate(OnUserLibReloadCompleted);
            DDCS.UserLibNewUserCreated += new DDCSCommonDelegate(OnUserLibReloadCompleted);
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var btn = sender as RadioButton;
            DDCV.SwapMainScreen(btn.Tag as string);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(DDCG.MetaLoader.IsUpdateExpired())
            {
                new DDCVMetaUpdateIndicatorDialog
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = DDCV.MainWindow
                }.ShowDialog();
            }
            while (DDCV.PopScreen()) ;
            new DDCVWebGachaLogLoadIndicatorDialog
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = DDCV.MainWindow
            }.ShowDialog();
           
        }
        private void OnUidSwapping()
        {
            Action act = () => { UidVal = DDCL.Users.CurrentUserUID; UserSwapping = true; };
            Dispatcher.BeginInvoke(act);
        }

        private void OnUserSwapCompleted()
        {
            Action act = () => { UidVal = DDCL.Users.CurrentUserUID; UserSwapping = false; };
            Dispatcher.BeginInvoke(act);
            DDCL.Settings.RefreshLastUid();
        }

        private void OnUserLibReloadCompleted()
        {
            Action act = () => { UsersList = DDCL.Users.getUsersList(); if (UsersList.Count == 0) UsersList = null; };
            Dispatcher.BeginInvoke(act);
        }

        private void UidButton_Click(object sender, RoutedEventArgs e)
        {
            UidPanel.IsOpen = true;
        }

        private void UidSelectorButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            try
            {
                long uid = Convert.ToInt64(btn.Tag);
                SwapUserFromUidPanel(uid);
            }
            catch(Exception)
            {

            }

        }

        public async void SwapUserFromUidPanel(long uid)
        {
            if (uid == DDCL.Users.CurrentUserUID) return;
            if(DDCL.Users.userExists(uid))
            {
                Action initact = () => { UidVal = uid; UserSwapping = true; while (DDCV.PopScreen()) ; };
                await Dispatcher.BeginInvoke(initact);


                await DDCL.Users.swapUser(uid);
                if (DDCS.UidReloadCompleted != null)
                {
                    foreach (DDCSCommonDelegate method in DDCS.UidReloadCompleted.GetInvocationList())
                    {
                        method.BeginInvoke(null, null);
                    }
                }
                if (DDCS.LogReloadCompleted != null)
                {
                    foreach (DDCSCommonDelegate method in DDCS.LogReloadCompleted.GetInvocationList())
                    {
                        method.BeginInvoke(null, null);
                    }
                }
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            DDCV.PushScreen("", new DDCVLocalLogImportScreen());
        }
    }
}
