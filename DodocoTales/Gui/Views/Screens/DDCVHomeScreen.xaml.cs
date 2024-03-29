﻿using System;
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

namespace DodocoTales.Gui.Views.Screens
{
    /// <summary>
    /// DDCVHomeScreen.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVHomeScreen : DDCVSwapableScreen
    {
        public DDCVHomeScreen()
        {
            InitializeComponent();
        }

        public override void Refresh()
        {
            EventChar.Refresh();
            EventWeap.Refresh();
            Permanant.Refresh();
        }
    }
}
