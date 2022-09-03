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

    public MainWindow()
    {
        InitializeComponent();
        _ = DayTimeCalculation._locator;
        Closed += (_, _) => { TrayIcon.Dispose(); };
    }

    private void ExitApplicationCommand_OnExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        Application.Current.Exit();
    }

    private void ShowHideWindowCommand_OnExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        var window = UIHelper.SettingsWindow;
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