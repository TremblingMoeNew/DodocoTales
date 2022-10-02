using CommunityToolkit.Mvvm.ComponentModel;
using DodocoTales.Gui.Models;
using DodocoTales.Gui.Views.Screens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DodocoTales.Gui.ViewModels
{
    public class DDCVMainWindowVM: ObservableObject
    {
        private ObservableCollection<DDCVMainPanelItemModel> menuItems;
        public ObservableCollection<DDCVMainPanelItemModel> MenuItems
        {
            get => menuItems;
            set => SetProperty(ref menuItems, value);
        }

        public DDCVMainWindowVM()
        {
            MenuItems = new ObservableCollection<DDCVMainPanelItemModel>()
            {
                new DDCVMainPanelItemModel("首页","Home", 0){ IsSelected = true },
                new DDCVMainPanelItemModel("统计","Stats",0)
                {
                    MenuItems=new ObservableCollection<DDCVMainPanelItemModel>
                    {
                        new DDCVMainPanelItemModel("按照版本","Version", 1),
                        new DDCVMainPanelItemModel("按照类型","PoolType",1),
                    }
                }
            };
        }

    }
}
