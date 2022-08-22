using Windows.ApplicationModel;
using Windows.Graphics;
using AutoDL.Dialogs;
using AutoDL.Models;
using AutoDL.Pages;
using AutoDL.Utilities;
using Microsoft.UI.Windowing;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoDL;

/// <summary>
///     An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private readonly List<(string Tag, Type Page)> Pages = new()
    {
        ("ThemeAutomationPage", typeof(ThemeAutomationPage))
    };

    public MainWindow()
    {
        InitializeComponent();
        Activated += (_, _) =>
        {
            UIHelper.AppWindow.Resize(new SizeInt32(UIHelper.GetActualPixel(750), UIHelper.GetActualPixel(800)));
            UIHelper.TrySetMicaBackdrop();
            // UIHelper.SetTitleBarTransparent();
        };
        _ = DayTimeCalculation._locator;
        Title = "AutoDL";
        NavigationView.SelectedItem = ThemeAutomationNavigationViewItem;
        ContentFrame.Navigate(typeof(ThemeAutomationPage));
    }

    private void NavigationView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        var item = Pages.FirstOrDefault(p => p.Tag.Equals(args.InvokedItemContainer.Tag.ToString()));
        if (ContentFrame.CurrentSourcePageType != item.Page)
        {
            ContentFrame.Navigate(item.Page, null, args.RecommendedNavigationTransitionInfo);
        }
    }
}