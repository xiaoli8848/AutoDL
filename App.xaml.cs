using AutoDL.Models;
using AutoDL.Utilities;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoDL;

/// <summary>
///     Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    public readonly SettingsViewModel Settings = new();
    public Window m_window;

    /// <summary>
    ///     Initializes the singleton application object.  This is the first line of authored code
    ///     executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
        _ = ThemeSwitcher.Switcher;
    }

    /// <summary>
    ///     Invoked when the application is launched normally by the end user.  Other entry points
    ///     will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();
        UIHelper.TrySetMicaBackdrop();
        UIHelper.AppWindow.Resize(new SizeInt32(UIHelper.GetActualPixel(750), UIHelper.GetActualPixel(800)));
        UIHelper.AppWindow.Title = "AutoDL";
        m_window.Activate();
    }
}