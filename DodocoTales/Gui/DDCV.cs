using DodocoTales.Gui.View;
using DodocoTales.Gui.View.Model;
using DodocoTales.Gui.View.Screen;
using DodocoTales.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DodocoTales.Gui
{
    public static partial class DDCV
    {
        public static readonly Dictionary<string, DDCVScreenPtr> MainScreens = new Dictionary<string, DDCVScreenPtr>();
        public static readonly Stack<DDCVScreenPtr> StackedScreens = new Stack<DDCVScreenPtr>();
        public static MainWindow MainWindow;
        public static DDCVMainTitleBar MainTitleBar;
        public static Label MainTitle;
        // TODO: MainBreadcrumb
        public static Grid MainNavigater;

        public static void RegisterMainScreens()
        {
            RegisterMainScreen("Home", "", new DDCVHomeScreen());
            RegisterMainScreen("Version", "", new DDCVVersionViewScreen());
            RegisterMainScreen("Pool", "", new DDCVPoolTypeViewScreen());
        }



        public static void RegisterMainScreen(string tag, string title, UserControl screen)
        {
            var ptr = new DDCVScreenPtr { Title = title, Screen = screen };
            MainScreens.Add(tag, ptr);
            screen.Visibility = Visibility.Collapsed;
            MainNavigater.Children.Add(screen);
            if (StackedScreens.Count == 0)
            {
                StackedScreens.Push(ptr);
                ptr.Screen.Visibility = Visibility.Visible;
                MainTitle.Content = ptr.Title;
            }
        }

        public static void PushScreen(string title,UserControl screen)
        {
            var ptr = new DDCVScreenPtr { Title = title, Screen = screen };
            StackedScreens.Peek().Screen.Visibility = Visibility.Collapsed;
            MainNavigater.Children.Add(ptr.Screen);
            // TODO: Breadcrumb
            ptr.Screen.Visibility = Visibility.Visible;
            MainTitle.Content = ptr.Title;
            StackedScreens.Push(ptr);
        }

        public static bool PopScreen()
        {
            if (StackedScreens.Count == 1) return false;
            // TODO: Breadcrumb
            MainNavigater.Children.Remove(StackedScreens.Pop().Screen);
            MainTitle.Content = StackedScreens.Peek().Title;
            StackedScreens.Peek().Screen.Visibility = Visibility.Visible;
            return true;
        }

        public static void SwapMainScreen(string tag)
        {
            while (PopScreen()) ;
            if (!MainScreens.TryGetValue(tag, out DDCVScreenPtr ns))
            {
                //报错
                return;
            }
            var os = StackedScreens.Pop();
            if (ns != os)
            {
                os.Screen.Visibility = Visibility.Collapsed;
                ns.Screen.Visibility = Visibility.Visible;
                // TODO: Breadcrumb
                MainTitle.Content = ns.Title;
            }
            StackedScreens.Push(ns);

        }

        public static void StartAuthkeyCapture()
        {
            MainTitleBar.SetIsOnProxy(true);
            DDCG.AuthkeyProxy.StartProxy();
        }

        public static void StopAuthkeyCapture()
        {
            DDCG.AuthkeyProxy.EndProxy();
            MainTitleBar.SetIsOnProxy(false);
        }
    }


}
