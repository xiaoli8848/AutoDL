using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using AutoDL.Models;
using Microsoft.Win32;

namespace AutoDL.Utilities
{
    /// <summary>
    /// 描述程序偏好设置。
    /// <br/>
    /// - FirstTimeToLaunch 是否是第一次启动
    /// <br/>
    /// - DarkTimeStart 启用深色模式的时间
    /// </summary>
    public class ProgramSettings
    {
        public static RegistryKey LocalSettings => Registry.CurrentUser.OpenSubKey(@"Software\AutoDL", true);

#if DEBUG
        static ProgramSettings()
        {
            if (LocalSettings is not null)
            {
                Registry.CurrentUser.OpenSubKey(@"Software", true).DeleteSubKeyTree("AutoDL");
            }
        }
#endif

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
            SetStringSetting(nameof(SettingsViewModel.SwitcherEnabled), true.ToString());
            var task = Geolocator.RequestAccessAsync();
            task.AsTask().Wait();
            SetStringSetting(nameof(SettingsViewModel.DarkTimeStart), new TimeSpan(18, 0, 0).ToString());
            SetStringSetting(nameof(SettingsViewModel.DarkTimeEnd), new TimeSpan(7, 0, 0).ToString());
            SetStringSetting(nameof(SettingsViewModel.UseSunRiseAndSunSetTime), false.ToString());
        }
    }
}
