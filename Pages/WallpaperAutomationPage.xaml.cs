using AutoDL.Utilities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoDL.Pages;

/// <summary>
///     An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class WallpaperAutomationPage : Page
{
    public WallpaperAutomationPage()
    {
        InitializeComponent();
        MainGrid.DataContext = UIHelper.App.Settings;
    }
}