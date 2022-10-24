using CommunityToolkit.Mvvm.ComponentModel;
using DodocoTales.Common.Enums;
using DodocoTales.Library;
using DodocoTales.Library.Enums;
using DodocoTales.Library.GameClient.Models;
using DodocoTales.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.ViewModels.Dialogs
{
    public class DDCVGameClientManagerWindowEditDialogVM : ObservableObject
    {
        private Dictionary<string, DDCLGameClientType> clientOptions;
        public Dictionary<string, DDCLGameClientType> ClientOptions
        {
            get => clientOptions;
            set => SetProperty(ref clientOptions, value);
        }
        private Dictionary<string, DDCCTimeZone> timeZoneOptions;
        public Dictionary<string, DDCCTimeZone> TimeZoneOptions
        {
            get => timeZoneOptions;
            set => SetProperty(ref timeZoneOptions, value);
        }

        private DDCLGameClientType clientType;
        public DDCLGameClientType ClientType
        {
            get => clientType;
            set => SetProperty(ref clientType, value);
        }
        private DDCCTimeZone timeZone;
        public DDCCTimeZone TimeZone
        {
            get => timeZone;
            set => SetProperty(ref timeZone, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string path;
        public string Path
        {
            get => path;
            set => SetProperty(ref path, value);
        }

        public DDCLGameClientItem GameClientItem { get; set; }

        public DDCVGameClientManagerWindowEditDialogVM()
        {
            ClientOptions = new Dictionary<string, DDCLGameClientType>
            {
                { DDCL.GetGameClientTypeName(DDCLGameClientType.CN),DDCLGameClientType.CN },
                { DDCL.GetGameClientTypeName(DDCLGameClientType.Global), DDCLGameClientType.Global }
            };
            TimeZoneOptions = new Dictionary<string, DDCCTimeZone>
            {
                { DDCL.GetTimeZoneName(DDCCTimeZone.DefaultUTCP8), DDCCTimeZone.DefaultUTCP8 },
                { DDCL.GetTimeZoneName(DDCCTimeZone.EuropeUTCP1), DDCCTimeZone.EuropeUTCP1 },
                { DDCL.GetTimeZoneName(DDCCTimeZone.AmericaUTCM5), DDCCTimeZone.AmericaUTCM5 }
            };
            
        }

        public void NewGameClientItem()
        {
            GameClientItem = null;
        }

        public void LoadGameClientItem(DDCLGameClientItem item)
        {
            GameClientItem = item;
            Name = item.Name;
            ClientType = item.ClientType;
            TimeZone = item.TimeZone;
            Path = item.Path;
        }

        readonly string filefilter = "Genshin Impact Executable|YuanShen.exe;GenshinImpact.exe";
        public void EditGamePath()
        {
            var dialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = filefilter,
                
            };
            if (Path != null) dialog.InitialDirectory = Directory.GetParent(Path).Parent.FullName;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var item = DDCG.GameClientLoader.LoadGameClientItemFromExecutablePath(dialog.FileName);
                if (item != null)
                {
                    if (Name == null) Name = item.Name;
                    if (ClientType == DDCLGameClientType.Unknown) ClientType = item.ClientType;
                    if (GameClientItem == null) TimeZone = item.TimeZone;
                    Path = item.Path;
                }
                
            }
        }
        public bool CheckValid()
        {
            return !string.IsNullOrEmpty(Name)
                && ClientType != DDCLGameClientType.Unknown
                && !string.IsNullOrEmpty(Path);
        }


        public void SaveAsCopy()
        {
            if (!CheckValid()) return;
            var item = new DDCLGameClientItem
            {
                Name = Name,
                ClientType = ClientType,
                Path = Path,
                TimeZone = TimeZone
            };
            DDCL.GameClientLib.AddClients(new List<DDCLGameClientItem> { item });
        }

        public async Task Save()
        {
            if (GameClientItem == null)
            {
                SaveAsCopy();
                return;
            }
            if (!CheckValid()) return;
            GameClientItem.Name = Name;
            GameClientItem.ClientType = ClientType;
            GameClientItem.TimeZone = TimeZone;
            GameClientItem.Path = Path;
            await DDCL.GameClientLib.SaveLibraryAsync();
        }
        public void Delete()
        {
            if (GameClientItem == null) return;
            DDCL.GameClientLib.RemoveClients(new List<DDCLGameClientItem> { GameClientItem }); 
        }
    }
}
