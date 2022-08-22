using Windows.Devices.Geolocation;
using AutoDL.Models;
using Microsoft.Win32;

namespace AutoDL.Utilities;

/// <summary>
///     描述程序偏好设置。
///     <br />
///     - FirstTimeToLaunch 是否是第一次启动
///     <br />
///     - DarkTimeStart 启用深色模式的时间
/// </summary>
public class ProgramSettings
{
#if DEBUG
    static ProgramSettings()
    {
        if (LocalSettings is not null) Registry.CurrentUser.OpenSubKey(@"Software", true).DeleteSubKeyTree("AutoDL");
    }
#endif
    private static SettingsViewModel Settings => UIHelper.App.Settings;
    public static RegistryKey LocalSettings => Registry.CurrentUser.OpenSubKey(@"Software\AutoDL", true);

    public static string GetStringSetting(string key)
    {
        try
        {
            return LocalSettings.GetValue(key).ToString();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static void SetStringSetting(string key, string value)
    {
        LocalSettings.SetValue(key, value);
    }

    public static void CreateSettings()
    {
        Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("AutoDL");
        SetStringSetting(nameof(SettingsViewModel.FirstTimeToLaunch), true.ToString());
        SetStringSetting(nameof(SettingsViewModel.SwitcherEnabled), false.ToString());
        var task = Geolocator.RequestAccessAsync();
        task.AsTask().Wait();
        SetStringSetting(nameof(SettingsViewModel.DarkTimeStart), new TimeSpan(18, 0, 0).ToString());
        SetStringSetting(nameof(SettingsViewModel.DarkTimeEnd), new TimeSpan(7, 0, 0).ToString());
        SetStringSetting(nameof(SettingsViewModel.UseSunRiseAndSunSetTime), false.ToString());
        SetStringSetting(nameof(SettingsViewModel.UseCustomWallpaper), false.ToString());
        SetStringSetting(nameof(SettingsViewModel.Wallpapers), string.Empty);
    }

    public static TimeSpan[,] GetDefaultTimeSpans()
    {
        var numbers = Settings.Wallpapers.Count + 1;
        if (numbers == 1) return new[,] { { TimeSpan.Zero, new TimeSpan(0, 24, 0, 0) } };
        var result = new TimeSpan[numbers, 2];
        var span = Convert.ToInt32(Math.Floor((double)24 / numbers));
        result[0, 0] = TimeSpan.Zero;
        result[0, 1] = new TimeSpan(0, span, 0, 0);
        for (var i = 1; i < numbers; i++)
        {
            result[i, 0] = result[i - 1, 1];
            result[i, 1] = i == numbers - 1
                ? result[i, 0] + new TimeSpan(0, span * i, 0, 0)
                : new TimeSpan(0, 24, 0, 0);
        }

        return result;
    }

    public static (TimeSpan, TimeSpan) GetDefaultTimeSpan(int index)
    {
        var result = GetDefaultTimeSpans();
        return (result[index, 0], result[index, 1]);
    }

    public static (TimeSpan, TimeSpan) GetDefaultTimeSpan()
    {
        return GetDefaultTimeSpan(Settings.Wallpapers.Count);
    }
}