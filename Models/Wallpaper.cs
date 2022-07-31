using System.IO;

namespace AutoDL.Models;

public abstract class Wallpaper
{
    public abstract FileInfo WallpaperFile { get; init; }
    public abstract ImageSource PreviewImageSource { get; init; }
    public abstract TimeSpan StartTime { get; set; }
    public abstract TimeSpan EndTime { get; set; }

    public abstract void SetWallpaper();
}