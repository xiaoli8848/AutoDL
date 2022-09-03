using AutoDL.Pages;
using AutoDL.Utilities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoDL
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsWindow : Window
    {
        private readonly List<(string Tag, Type Page)> _pages = new()
        {
            ("ThemeAutomationPage", typeof(ThemeAutomationPage)),
            ("WallpaperAutomationPage", typeof(WallpaperAutomationPage))
        };

        public SettingsWindow()
        {
            this.InitializeComponent();
            NavigationView.SelectedItem = ThemeAutomationNavigationViewItem;
            ContentFrame.Navigate(typeof(ThemeAutomationPage));
            Activated += (_, _) =>
            {
                UIHelper.AppWindow.Resize(new SizeInt32(UIHelper.GetActualPixel(750), UIHelper.GetActualPixel(800)));
            };
        }

        private void NavigationView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var item = _pages.FirstOrDefault(p => p.Tag.Equals(args.InvokedItemContainer.Tag.ToString()));
            if (ContentFrame.CurrentSourcePageType != item.Page)
                ContentFrame.Navigate(item.Page, null, args.RecommendedNavigationTransitionInfo);
        }
    }
}
