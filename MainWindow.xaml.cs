using Windows.Graphics;
using Windows.UI.Shell;
using AutoDL.Pages;
using AutoDL.Utilities;
using H.NotifyIcon;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoDL;

/// <summary>
///     An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private readonly List<(string Tag, Type Page)> _pages = new()
    {
        ("ThemeAutomationPage", typeof(ThemeAutomationPage)),
        ("WallpaperAutomationPage", typeof(WallpaperAutomationPage))
    };

    public MainWindow()
    {
        InitializeComponent();
        _ = DayTimeCalculation._locator;
        Title = "AutoDL";
        NavigationView.SelectedItem = ThemeAutomationNavigationViewItem;
        ContentFrame.Navigate(typeof(ThemeAutomationPage));
        Closed += (_, _) => { TrayIcon.Dispose(); };
    }

    private void NavigationView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        var item = _pages.FirstOrDefault(p => p.Tag.Equals(args.InvokedItemContainer.Tag.ToString()));
        if (ContentFrame.CurrentSourcePageType != item.Page)
            ContentFrame.Navigate(item.Page, null, args.RecommendedNavigationTransitionInfo);
    }

    private void ExitApplicationCommand_OnExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        Application.Current.Exit();
    }

    private void ShowHideWindowCommand_OnExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        var window = UIHelper.MainWindow;
        if (window == null)
        {
            return;
        }

        if (window.Visible)
        {
            window.Hide();
        }
        else
        {
            window.Show();
        }
    }
}