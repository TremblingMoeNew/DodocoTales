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

namespace DodocoTales.Gui.View.Screen
{
    /// <summary>
    /// DDCVLocalLogImportScreen.xaml 的交互逻辑
    /// </summary>
    public partial class DDCVLocalLogImportScreen : UserControl
    {
        public static readonly DependencyProperty ImportedDataProperty = DependencyProperty.Register("ImportedData", typeof(List<DDCGGachaLogImportedItem>), typeof(DDCVLocalLogImportScreen));
        public List<DDCGGachaLogImportedItem> ImportedData
        {
            set { SetValue(ImportedDataProperty, value); }
            get { return (List<DDCGGachaLogImportedItem>)GetValue(ImportedDataProperty); }
        }


        public DDCVLocalLogImportScreen()
        {
            InitializeComponent();
            test_load_genshin_wish_export();
            //test_load_keqing_niuza();
        }

        void test_load_genshin_wish_export()
        {
            try
            {
                var stream = File.Open(@"D:\test\gacha-list-.json", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var buffer = reader.ReadToEnd();
                DDCIGenWishExpLogImporter importer = new DDCIGenWishExpLogImporter();
                var result = importer.ConvertToDDCLogs(buffer);
                DDCG.LocalLogLoader.CheckImportedLogValidty(result);

                ImportedData = result.EventCharacter;
            }
            catch (Exception)
            {
                return;
            }
        }
        void test_load_keqing_niuza()
        {
            try
            {
                var stream = File.Open(@"D:\test\WishLog_.json", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(stream);
                var buffer = reader.ReadToEnd();
                DDCIKeqingNiuzaLogImporter importer = new DDCIKeqingNiuzaLogImporter();
                var result = importer.ConvertToDDCLogs(buffer);
                DDCG.LocalLogLoader.CheckImportedLogValidty(result);

                ImportedData = result.EventCharacter;
            }
            catch (Exception)
            {
                return;
            }
}
    }
}
