using Windows.ApplicationModel;
using Windows.Graphics;
using AutoDL.Dialogs;
using AutoDL.Models;
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
    public MainWindow()
    {
        InitializeComponent();
        MainGrid.DataContext = (Application.Current as App).SettingsViewModel;
        Activated += (_, _) =>
        {
            UIHelper.GetAppWindow()
                .Resize(new SizeInt32(UIHelper.GetActualPixel(750), UIHelper.GetActualPixel(800)));
        };
        _ = DayTimeCalculation._locator;
        Title = "AutoDL";
    }

    private SettingsViewModel Settings => MainGrid.DataContext as SettingsViewModel;

    private async void AddImageWallpaper_OnExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        var result = await ImportImageWallpaperDialog.ImportImageWallpaperAsync(Content.XamlRoot);
        if (result is not null) Settings.Wallpapers.Add(result);
    }

    private async void AddWallpaperEngineWallpaper_OnExecuteRequested(XamlUICommand sender,
        ExecuteRequestedEventArgs args)
    {
        var result = await ImportWallpaperEngineWallpaperDialog.ImportWallpaperEngineWallpaperAsync(Content.XamlRoot);
        if (result is not null) Settings.Wallpapers.Add(result);
    }
}