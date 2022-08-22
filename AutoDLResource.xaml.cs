

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using AutoDL.Dialogs;
using AutoDL.Models;
using AutoDL.Utilities;

namespace AutoDL;

/// <summary>
///     An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AutoDLResource : ResourceDictionary
{
    private static SettingsViewModel Settings => UIHelper.App.Settings;
    public AutoDLResource()
    {
        InitializeComponent();
    }
    private async void AddImageWallpaper_OnExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        var result = await ImportImageWallpaperDialog.ImportImageWallpaperAsync(UIHelper.MainWindow.Content.XamlRoot);
        if (result is not null) Settings.Wallpapers.Add(result);
    }

    private async void AddWallpaperEngineWallpaper_OnExecuteRequested(XamlUICommand sender,
        ExecuteRequestedEventArgs args)
    {
        var result = await ImportWallpaperEngineWallpaperDialog.ImportWallpaperEngineWallpaperAsync(UIHelper.MainWindow.Content.XamlRoot);
        if (result is not null) Settings.Wallpapers.Add(result);
    }
}

public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool passed)
            return !passed;

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is bool passed)
            return !passed;

        return null;
    }
}