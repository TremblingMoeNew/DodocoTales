﻿using DodocoTales.Library;
using DodocoTales.Library.GameClient.Models;
using DodocoTales.Loader;
using Panuon.UI.Silver;
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

namespace DodocoTales.Gui.Views.Windows
{
    /// <summary>
    /// DDCVGameClientManagerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVGameClientManagerWindow : WindowX
    {
        public DDCVGameClientManagerWindow()
        {
            InitializeComponent();
            VM.ReloadData();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void LoadFromLog_Click(object sender, RoutedEventArgs e)
        {
            DDCG.GameClientLoader.LoadGameClientFromGameLog();
            VM.ReloadData();
        }

        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            DDCLGameClientItem item = (sender as Button).DataContext as DDCLGameClientItem;
            DDCL.GameClientLib.SetSelectedClient(item);
            DialogResult = true;
            Close();
        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            DDCLGameClientItem item = (sender as Button).DataContext as DDCLGameClientItem;
            DDCG.GameClientLoader.RemoveCacheFile(item);
        }
    }
}
