using System.Timers;
using AutoDL.Models;
using Microsoft.Win32;

namespace AutoDL.Utilities;

public static class ThemeSwitcher
{
    private static readonly RegistryKey Personalize =
        Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);

    public static readonly Timer Switcher = new(1000);

    static ThemeSwitcher()
    {
        if (Settings.SwitcherEnabled)
            Switcher.Start();
        Switcher.Elapsed += Switcher_Callback;
        Settings.PropertyChanged += (_, args) =>
        {
            if (Settings.SwitcherEnabled)
                Switcher.Start();
            else
                Switcher.Stop();
        };
    }

    private static SettingsViewModel Settings => UIHelper.App.Settings;

    private static Wallpaper GetCurrentWallpaper(TimeSpan now)
    {
        foreach (var item in Settings.Wallpapers)
            if (now.CompareTo(item.StartTime) >= 0 && now.CompareTo(item.EndTime) <= 0)
                return item;
        return null;
    }

    private static void Switcher_Callback(object sender, ElapsedEventArgs args)
    {
        var now = DateTime.Now - DateTime.Today;
        if (Settings.DarkTimeEnd < Settings.DarkTimeStart)
        {
            if (now.CompareTo(Settings.DarkTimeStart) >= 0 || now.CompareTo(Settings.DarkTimeEnd) <= 0)
            {
                if (GetSystemColor() != ColorThemeType.Dark) SetSystemColor(ColorThemeType.Dark);

                if (GetAppColor() != ColorThemeType.Dark) SetAppColor(ColorThemeType.Dark);
            }
            else
            {
                SetSystemColor(ColorThemeType.Light);
                SetAppColor(ColorThemeType.Light);
            }
        }
        else
        {
            if (now.CompareTo(Settings.DarkTimeStart) >= 0 && now.CompareTo(Settings.DarkTimeEnd) <= 0)
            {
                if (GetSystemColor() != ColorThemeType.Dark) SetSystemColor(ColorThemeType.Dark);

                if (GetAppColor() != ColorThemeType.Dark) SetAppColor(ColorThemeType.Dark);
            }
            else
            {
                SetSystemColor(ColorThemeType.Light);
                SetAppColor(ColorThemeType.Light);
            }
        }

        if (Settings.UseCustomWallpaper)
        {
            var wallpaper = GetCurrentWallpaper(now);
            if (wallpaper is not null) wallpaper.SetWallpaper();
        }
    }

    private static ColorThemeType GetAppColor()
    {
        var registyDataOne = Personalize.GetValue("AppsUseLightTheme").ToString();
        if (registyDataOne == "0x00000000") return ColorThemeType.Dark;
        return ColorThemeType.Light;
    }

    private static ColorThemeType GetSystemColor()
    {
        var registyDataOne = Personalize.GetValue("SystemUsesLightTheme").ToString();
        if (registyDataOne == "0x00000000") return ColorThemeType.Dark;
        return ColorThemeType.Light;
    }

    private static void SetAppColor(ColorThemeType type)
    {
        Personalize.SetValue("AppsUseLightTheme", (int)type);
    }

    private static void SetSystemColor(ColorThemeType type)
    {
        Personalize.SetValue("SystemUsesLightTheme", (int)type);
    }
}

public enum ColorThemeType
{
    Dark = 0x00000000,
    Light = 0x00000001
}