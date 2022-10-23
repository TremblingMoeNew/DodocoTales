using CommunityToolkit.Mvvm.ComponentModel;
using DodocoTales.Common.Enums;
using DodocoTales.Gui.Models;
using DodocoTales.Gui.Views.Screens;
using DodocoTales.Gui.Views.Windows;
using DodocoTales.Library;
using DodocoTales.Library.Enums;
using DodocoTales.Loader;
using Panuon.UI.Silver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        private bool isProxyMode;
        public bool IsProxyMode
        {
            get => isProxyMode;
            set
            {
                if (value)
                {

                }
                else
                {

                }
                SetProperty(ref isProxyMode, value);
            }
        }

        private bool isProxyModeOn;
        public bool IsProxyModeOn
        {
            get => isProxyModeOn;
            set
            {
                if (value)
                {


                    IsWaiting = value;
                }
                else
                {

                    IsWaiting = false;
                }
                SetProperty(ref isProxyModeOn, value);
            }
        }

        private bool isInUpdate;
        public bool IsInUpdate
        {
            get => isInUpdate;
            set
            {
                SetProperty(ref isInUpdate, value);
                IsWaiting = value;
            }
        }
        private bool isWaiting;
        public bool IsWaiting
        {
            get => isWaiting;
            set => SetProperty(ref isWaiting, value);
        }

        private long currentUID;
        public long CurrentUID
        {
            get => currentUID;
            set => SetProperty(ref currentUID, value);
        }

        private DDCCTimeZone currentUserTimeZone;
        public DDCCTimeZone CurrentUserTimeZone
        {
            get => currentUserTimeZone;
            set => SetProperty(ref currentUserTimeZone, value);
        }
        private DDCLGameClientType currentUserClientType;
        public DDCLGameClientType CurrentUserClientType
        {
            get => currentUserClientType;
            set => SetProperty(ref currentUserClientType, value);
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
            CurrentUID = -1;
        }

        public void RefreshCurrentUID()
        {
            var user = DDCL.CurrentUser.OriginalLogs;
            if (user == null) CurrentUID = -1;
            else
            {
                CurrentUID = user.uid;
                CurrentUserTimeZone = user.zone;
                CurrentUserClientType = user.ClientType;
            }
            DDCV.RefreshAll();
        }

        public async Task WishLogUpdateAppended()
        {
            if (IsProxyMode)
                ;
            else
                await WishLogUpdateAppendedFromCache();
        }

        public async Task WishLogUpdateFull()
        {
            if (IsProxyMode)
                ;
            else
                await WishLogUpdateFullFromCache();
        }
        public async Task WishLogUpdateAppendedFromCache()
        {
            var client = DDCL.GameClientLib.GetSelectedClient();
            if (client == null)
                if (!DDCV.ShowWindowDialog(new DDCVGameClientManagerWindow()))
                    return;

            client = DDCL.GameClientLib.GetSelectedClient();
            var authkey = DDCG.GameClientLoader.GetAuthkeyFromWebCache(client);

            if (authkey == null)
            {
                Notice.Show("更新失败，未能找到祈愿记录网址。\n请确认您的原神客户端地址设置是否正确，且是否在游戏中正确打开祈愿历史记录。", "祈愿记录更新失败", MessageBoxIcon.Error);
                return;
            }
            IsInUpdate = true;
            // TODO
            var uid = await DDCG.WebLogLoader.TryConnectAndGetUid(authkey, client.ClientType);
            Console.WriteLine(uid);
            if (uid < 0)
            {
                Notice.Show("更新失败，未能获取到祈愿记录\n请确认网络是否连接正常，设置的客户端类型是否正确，近六个月内是否进行过祈愿。\n请在游戏中重新打开祈愿历史记录页面。若仍然失败，可尝试在客户端管理中清除缓存。", "祈愿记录更新失败", MessageBoxIcon.Error);
                IsInUpdate = false;
                return;
            }
            var user = DDCL.UserDataLib.GetUserLogByUid(uid);
            if (user.Logs.Count == 0)
            {
                user.zone = client.TimeZone;
            }
            if (user.ClientType == DDCLGameClientType.Unknown)
            {
                user.ClientType = client.ClientType;
            }
            DDCL.CurrentUser.SwapUser(user);
            await DDCG.WebLogLoader.GetGachaLogsAsNormalMode(authkey, client.ClientType);
            IsInUpdate = false;
            DDCV.RefreshAll();
            Notice.Show("祈愿记录常规更新完毕", "祈愿记录更新完毕", MessageBoxIcon.Success);
        }
        public async Task WishLogUpdateFullFromCache()
        {
            var client = DDCL.GameClientLib.GetSelectedClient();
            if (client == null)
                if (!DDCV.ShowWindowDialog(new DDCVGameClientManagerWindow()))
                    return;

            client = DDCL.GameClientLib.GetSelectedClient();
            var authkey = DDCG.GameClientLoader.GetAuthkeyFromWebCache(client);

            if (authkey == null)
            {
                Notice.Show("更新失败，未能找到祈愿记录网址。\n请确认您的原神客户端地址设置是否正确，且是否在游戏中正确打开祈愿历史记录。", "祈愿记录更新失败", MessageBoxIcon.Error);
                return;
            }
            IsInUpdate = true;
            var uid = await DDCG.WebLogLoader.GetUidFromWeb(authkey);
            Console.WriteLine(uid);
            if (uid < 0)
            {
                Notice.Show("更新失败，未能获取到祈愿记录\n请确认网络是否连接正常，设置的客户端类型是否正确，近六个月内是否进行过祈愿。\n请在游戏中重新打开祈愿历史记录页面。若仍然失败，可尝试在客户端管理中清除缓存。", "祈愿记录更新失败", MessageBoxIcon.Error);
                IsInUpdate = false;
                return;
            }

            var user = DDCL.UserDataLib.GetUserLogByUid(uid);
            if (user.Logs.Count == 0)
            {
                user.zone = client.TimeZone;
            }
            if (user.ClientType == DDCLGameClientType.Unknown)
            {
                user.ClientType = client.ClientType;
            }
            DDCL.CurrentUser.SwapUser(user);
            await DDCG.WebLogLoader.GetGachaLogsAsFullMode(authkey, client.ClientType);
            IsInUpdate = false;
            DDCV.RefreshAll();
            Notice.Show("祈愿记录全量更新完毕", "祈愿记录更新完毕", MessageBoxIcon.Success);
        }
    }
}
