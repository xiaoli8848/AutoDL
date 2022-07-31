using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Win32;

namespace AutoDL.Models;

public sealed class WallpaperEngineWallpaper : Wallpaper
{
    public WallpaperEngineWallpaper(FileInfo wallpaperFile, TimeSpan startTime, TimeSpan endTime)
    {
        WallpaperFile = wallpaperFile;
        PreviewImageSource =
            new BitmapImage(new Uri(WallpaperFile.Directory.GetFiles().First(x => x.Name == "preview.gif").FullName));
        StartTime = startTime;
        EndTime = endTime;
    }

    public override FileInfo WallpaperFile { get; init; }
    public override ImageSource PreviewImageSource { get; init; }
    public override TimeSpan StartTime { get; set; }
    public override TimeSpan EndTime { get; set; }

    public override async void SetWallpaper()
    {
        await Task.Run(() =>
        {
            var wallpaperEnginePath = Registry.CurrentUser.OpenSubKey(@"Software\WallpaperEngine", true)
                .GetValue("installPath").ToString();

            var sb = new StringBuilder().Append("\"").Append(wallpaperEnginePath)
                .Append(" -control openWallpaper -file ").Append("\"").Append(WallpaperFile.FullName).Append("\"");
            var command = sb.ToString();

            //执行cmd命令
            var process = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.StandardInput.WriteLine(command);
            process.StandardInput.WriteLine("exit");
            process.StandardInput.AutoFlush = true;
            process.WaitForExit();
        });
    }
}