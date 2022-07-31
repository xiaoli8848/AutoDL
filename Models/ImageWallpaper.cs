using System.IO;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Win32;

namespace AutoDL.Models;

public sealed class ImageWallpaper : Wallpaper
{
    public ImageWallpaper(FileInfo wallpaperFile, TimeSpan startTime, TimeSpan endTime)
    {
        WallpaperFile = wallpaperFile;
        PreviewImageSource = new BitmapImage(new Uri(wallpaperFile.FullName));
        StartTime = startTime;
        EndTime = endTime;
    }

    public override FileInfo WallpaperFile { get; init; }

    public override ImageSource PreviewImageSource { get; init; }
    public override TimeSpan StartTime { get; set; }
    public override TimeSpan EndTime { get; set; }

    public override void SetWallpaper()
    {
        Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true).SetValue("WallPaper", WallpaperFile.FullName);
    }
}