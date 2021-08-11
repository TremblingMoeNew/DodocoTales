using DodocoTales.Common.Enums;
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

namespace DodocoTales.Gui.View.Screen
{
    /// <summary>
    /// DDCVPoolTypeViewScreen.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVPoolTypeViewScreen : UserControl
    {
        public DDCVPoolTypeViewScreen()
        {
            InitializeComponent();
            EventCharacter.InitializeCard(DDCCPoolType.EventCharacter);
            EventWeapon.InitializeCard(DDCCPoolType.EventWeapon);
            Permanent.InitializeCard(DDCCPoolType.Permanent);
            Beginner.InitializeCard(DDCCPoolType.Beginner);
        }
    }
}
