using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage.Pickers;
using AutoDL.Models;
using AutoDL.Utilities;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoDL.Dialogs;

/// <summary>
///     An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ImportImageWallpaperDialogContent : Page
{
    public ImportImageWallpaperDialogContent()
    {
        InitializeComponent();
    }
}

public sealed class ImportImageWallpaperDialog : ContentDialog
{
    public ImportImageWallpaperDialog(XamlRoot root)
    {
        var res = ResourceLoader.GetForViewIndependentUse();
        Title = res.GetString("ImportImageWallpaperDialogTitle");
        Content = new ImportImageWallpaperDialogContent();
        Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        CloseButtonText = res.GetString("ImportImageWallpaperDialogCloseButtonText");
        DefaultButton = ContentDialogButton.Close;
        IsPrimaryButtonEnabled = false;
        PrimaryButtonText = res.GetString("ImportImageWallpaperDialogImportButtonText");
        XamlRoot = root;

        (Content as ImportImageWallpaperDialogContent).PickerButton.Click += async (_, _) =>
        {
            FileOpenPicker picker = new()
            {
                SuggestedStartLocation = PickerLocationId.ComputerFolder,
                CommitButtonText = res.GetString("ImportImageWallpaperDialogImportButtonText"),
                SettingsIdentifier = "settingsIdentifier"
            };
            picker.FileTypeFilter.Add(".png");

            var hwnd = WindowNative.GetWindowHandle((Application.Current as App).m_window);
            InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();

            (Content as ImportImageWallpaperDialogContent).PathBox.Text = file?.Path;
            IsPrimaryButtonEnabled = !string.IsNullOrEmpty((Content as ImportImageWallpaperDialogContent).PathBox.Text);
        };
    }

    public static async Task<ImageWallpaper> ImportImageWallpaperAsync(XamlRoot root)
    {
        var dialog = new ImportImageWallpaperDialog(root);
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            var time = ProgramSettings.GetDefaultTimeSpan();
            return new ImageWallpaper(
                new FileInfo(((ImportImageWallpaperDialogContent)dialog.Content).PathBox.Text), time.Item1,
                time.Item2);
        }

        return null;
    }
}