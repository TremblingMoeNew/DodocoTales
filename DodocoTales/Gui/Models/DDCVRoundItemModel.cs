using CommunityToolkit.Mvvm.ComponentModel;
using DodocoTales.Library.CurrentUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.Models
{
    public class DDCVRoundItemModel : ObservableObject
    {
        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        private ulong versionId;
        public ulong VersionId
        {
            get => versionId;
            set => SetProperty(ref versionId, value);
        }
        private ulong bannerId;
        public ulong BannerId
        {
            get => bannerId;
            set => SetProperty(ref bannerId, value);
        }

        private int index;
        public int Index
        {
            get => index;
            set => SetProperty(ref index, value);
        }

        private DDCLRoundLogItem logItem;
        public DDCLRoundLogItem LogItem
        {
            get => logItem;
            set => SetProperty(ref logItem, value);
        }

        public int Count
        {
            get => LogItem?.Logs.Count ?? 0;
        }

        public int CountCurrent
        {
            get => LogItem?.Logs.Count(x => x.version_id == VersionId && x.banner_id == BannerId) ?? 0;
        }

        public int CountInherited
        {
            get => Count - CountCurrent;
        }

        public int Rank5
        {
            get => LogItem?.Logs.Count(x => x.rank == 5) ?? 0;
        }

        public int Rank4
        {
            get => LogItem?.Logs.Count(x => x.rank == 4) ?? 0;
        }

    }
}
