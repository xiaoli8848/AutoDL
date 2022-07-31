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
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using AutoDL.Models;
using AutoDL.Utilities;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoDL.Dialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImportWallpaperEngineWallpaperDialogContent : Page
    {
        public ImportWallpaperEngineWallpaperDialogContent()
        {
            this.InitializeComponent();
        }
    }

    public sealed class ImportWallpaperEngineWallpaperDialog : ContentDialog
    {
        public ImportWallpaperEngineWallpaperDialog(XamlRoot root)
        {
            var res = ResourceLoader.GetForViewIndependentUse();
            Title = res.GetString("ImportWallpaperEngineWallpaperDialogTitle");
            Content = new ImportWallpaperEngineWallpaperDialogContent();
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            CloseButtonText = res.GetString("ImportWallpaperEngineWallpaperDialogCloseButtonText");
            DefaultButton = ContentDialogButton.Close;
            IsPrimaryButtonEnabled = false;
            PrimaryButtonText = res.GetString("ImportWallpaperEngineWallpaperDialogImportButtonText");
            XamlRoot = root;

            (Content as ImportWallpaperEngineWallpaperDialogContent).PickerButton.Click += async (_, _) =>
            {
                FileOpenPicker picker = new()
                {
                    SuggestedStartLocation = PickerLocationId.ComputerFolder,
                    CommitButtonText = res.GetString("ImportWallpaperEngineWallpaperDialogImportButtonText"),
                    SettingsIdentifier = "settingsIdentifier"
                };
                picker.FileTypeFilter.Add(".json");

                var hwnd = WindowNative.GetWindowHandle((Application.Current as App).m_window);
                InitializeWithWindow.Initialize(picker, hwnd);

                var file = await picker.PickSingleFileAsync();

                (Content as ImportWallpaperEngineWallpaperDialogContent).PathBox.Text = file?.Path;
                IsPrimaryButtonEnabled = !string.IsNullOrEmpty((Content as ImportWallpaperEngineWallpaperDialogContent).PathBox.Text);
            };
        }

        public static async Task<WallpaperEngineWallpaper> ImportWallpaperEngineWallpaperAsync(XamlRoot root)
        {
            var dialog = new ImportWallpaperEngineWallpaperDialog(root);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var time = ProgramSettings.GetDefaultTimeSpan();
                return new WallpaperEngineWallpaper(
                    new FileInfo(((ImportWallpaperEngineWallpaperDialogContent)dialog.Content).PathBox.Text), time.Item1,
                    time.Item2);
            }

            return null;
        }
    }
}
