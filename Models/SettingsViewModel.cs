using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.Devices.Geolocation;
using AutoDL.Utilities;

namespace AutoDL.Models;

public sealed class SettingsViewModel : INotifyPropertyChanged
{
    public SettingsViewModel()
    {
        if (ProgramSettings.LocalSettings is null) ProgramSettings.CreateSettings();

        var sourceString = ProgramSettings.GetStringSetting(nameof(Wallpapers));
        Wallpapers = new ObservableCollection<Wallpaper>();
        if (!string.IsNullOrEmpty(sourceString))
            foreach (var item in sourceString.Split(';'))
            {
                var strings = item.Split(',');
                var file = new FileInfo(strings[0]);
                if (file.Extension == ".json")
                    Wallpapers.Add(new WallpaperEngineWallpaper(file, TimeSpan.Parse(strings[1]),
                        TimeSpan.Parse(strings[2])));
                else
                    Wallpapers.Add(new ImageWallpaper(file, TimeSpan.Parse(strings[1]), TimeSpan.Parse(strings[2])));
            }

        Wallpapers.CollectionChanged += (sender, args) =>
        {
            var sb = new StringBuilder();
            foreach (var wallpaper in Wallpapers)
                sb.Append(new StringBuilder().Append(wallpaper.WallpaperFile.FullName).Append(',')
                    .Append(wallpaper.StartTime.ToString()).Append(',').Append(wallpaper.EndTime.ToString()).Append(';')
                    .ToString());
            ProgramSettings.SetStringSetting(nameof(Wallpapers), sb.ToString());
        };
    }

    public bool HaveAccessForLocator
    {
        get
        {
            var task = Geolocator.RequestAccessAsync();
            task.AsTask().Wait();
            return task.GetResults() == GeolocationAccessStatus.Allowed;
        }
    }

    public bool FirstTimeToLaunch
    {
        get => Convert.ToBoolean(ProgramSettings.GetStringSetting(nameof(FirstTimeToLaunch)));
        set
        {
            ProgramSettings.SetStringSetting(nameof(FirstTimeToLaunch), value.ToString());
            OnPropertyChanged();
        }
    }

    public bool SwitcherEnabled
    {
        get => Convert.ToBoolean(ProgramSettings.GetStringSetting(nameof(SwitcherEnabled)));
        set
        {
            ProgramSettings.SetStringSetting(nameof(SwitcherEnabled), value.ToString());
            OnPropertyChanged();
        }
    }

    public TimeSpan DarkTimeStart
    {
        get
        {
            TimeSpan start;
            TimeSpan.TryParse(ProgramSettings.GetStringSetting(nameof(DarkTimeStart)), out start);
            return start;
        }
        set
        {
            ProgramSettings.SetStringSetting(nameof(DarkTimeStart), value.ToString());
            OnPropertyChanged();
        }
    }

    public TimeSpan DarkTimeEnd
    {
        get
        {
            TimeSpan start;
            TimeSpan.TryParse(ProgramSettings.GetStringSetting(nameof(DarkTimeEnd)), out start);
            return start;
        }
        set
        {
            ProgramSettings.SetStringSetting(nameof(DarkTimeEnd), value.ToString());
            OnPropertyChanged();
        }
    }

    public bool UseSunRiseAndSunSetTime
    {
        get => Convert.ToBoolean(ProgramSettings.GetStringSetting(nameof(UseSunRiseAndSunSetTime)));
        set
        {
            ProgramSettings.SetStringSetting(nameof(UseSunRiseAndSunSetTime), value.ToString());
            OnPropertyChanged();
        }
    }

    public bool UseCustomWallpaper
    {
        get => Convert.ToBoolean(ProgramSettings.GetStringSetting(nameof(UseCustomWallpaper)));
        set
        {
            ProgramSettings.SetStringSetting(nameof(UseCustomWallpaper), value.ToString());
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Wallpaper> Wallpapers { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}