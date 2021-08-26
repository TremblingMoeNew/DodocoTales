using DodocoTales.Loader;
using DodocoTales.Loader.Compatibility.GenWishExport;
using DodocoTales.Loader.Compatibility.KeqingNiuza;
using DodocoTales.Loader.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace DodocoTales.Gui.View.Screen
{
    /// <summary>
    /// DDCVLocalLogImportScreen.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVLocalLogImportScreen : UserControl
    {
        public static readonly DependencyProperty DisplayedDataProperty = DependencyProperty.Register("DisplayedData", typeof(List<DDCGGachaLogImportedItem>), typeof(DDCVLocalLogImportScreen));
        public List<DDCGGachaLogImportedItem> DisplayedData
        {
            set { SetValue(DisplayedDataProperty, value); }
            get { return (List<DDCGGachaLogImportedItem>)GetValue(DisplayedDataProperty); }
        }


        public DDCGGachaLogImportResult ImportedData = null;

        readonly string filefilter_DodocoTales = "嘟嘟可故事集记录文件|userlog_*.json|Json文件|*.json";
        readonly string filefilter_GenshinWishExport = "原神祈愿导出工具记录文件|gacha-list-*.json|Json文件|*.json";
        readonly string filefilter_KeqingNiuza = "刻记牛杂店记录文件|WishLog_*.json|Json文件|*.json";
        readonly string filefilter_Excel = "Excel Worksheet|*.xlsx";


        public DDCVLocalLogImportScreen()
        {
            InitializeComponent();
            DisplayedData = null;

        }

        private void LoadFile_Button_Click(object sender, RoutedEventArgs e)
        {
            string filter = "";
            if (FTBtn_DodocoTales.IsChecked == true) filter = filefilter_DodocoTales;
            else if (FTBtn_GenshinWishExport.IsChecked == true) filter = filefilter_GenshinWishExport;
            else if (FTBtn_KeqingNiuza.IsChecked == true) filter = filefilter_KeqingNiuza;
            else if (FTBtn_Excel.IsChecked == true) filter = filefilter_Excel;
            else return;    
            var dialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = filter
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (FTBtn_DodocoTales.IsChecked == true) return;
                else if (FTBtn_GenshinWishExport.IsChecked == true) LoadFromGenshinWishExport(dialog.FileName);
                else if (FTBtn_KeqingNiuza.IsChecked == true) LoadFromKeqingNiuza(dialog.FileName);
                else if (FTBtn_Excel.IsChecked == true) return;
                else return;
            }
        }

        private async void LoadFromGenshinWishExport(string FileName)
        {
            var buffer = await ReadJsonFile(FileName);
            if (buffer == null) return;
            DDCIGenWishExpLogImporter importer = new DDCIGenWishExpLogImporter();
            var result = importer.ConvertToDDCLogs(buffer);
            LoadResult(result);
        }
        private async void LoadFromKeqingNiuza(string FileName)
        {
            var buffer = await ReadJsonFile(FileName);
            if (buffer == null) return;
            DDCIKeqingNiuzaLogImporter importer = new DDCIKeqingNiuzaLogImporter();
            var result = importer.ConvertToDDCLogs(buffer);
            LoadResult(result);
        }

        private async void LoadResult(DDCGGachaLogImportResult result)
        {
            var isnewuser = DDCG.LocalLogLoader.CheckImportedLogValidty(result);
            ImportedData = result;
            // Temp
            Action action = () => { RefreshDisplay(); };
            await Dispatcher.BeginInvoke(action, DispatcherPriority.Background);
        }

        private void RefreshDisplay()
        {
            // Temp
            DisplayedData = ImportedData.EventCharacter;


        }



        private async Task<string> ReadJsonFile(string FileName)
        {
            try
            {
                using (Stream stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    StreamReader reader = new StreamReader(stream);
                    return await reader.ReadToEndAsync();
                }
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
