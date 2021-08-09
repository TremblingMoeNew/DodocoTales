using DodocoTales.Common;
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

        public static readonly DependencyProperty UidProperty = DependencyProperty.Register("Uid", typeof(string), typeof(DDCVMainMenuPanel));
        public string Uid
        {
            set { SetValue(UidProperty, value); }
            get { return (string)GetValue(UidProperty); }
        }

        public DDCVMainMenuPanel()
        {
            InitializeComponent();
            Uid = "-----未登录-----";
            DDCS.UidReloadCompleted += new DDCSCommonDelegate(OnUidSwapped);
            
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var btn = sender as RadioButton;
            DDCV.SwapMainScreen(btn.Tag as string);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DDCVWebGachaLogLoadIndicatorDialog
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = DDCV.MainWindow
            }.ShowDialog();
           
        }
        private void OnUidSwapped()
        {
            if (DDCL.Users.CurrentUserUID == 0)
            {
                Action act = () => { Uid = "-----未登录-----"; };
                Dispatcher.BeginInvoke(act);
            }
            else
            {
                // Temp
                Action act = () => { Uid = String.Format("UID:{0}****{1}", DDCL.Users.CurrentUserUID / 10000000, DDCL.Users.CurrentUserUID % 1000); };
                Dispatcher.BeginInvoke(act);
            }
            DDCL.Settings.RefreshLastUid();
        }
    }
}
