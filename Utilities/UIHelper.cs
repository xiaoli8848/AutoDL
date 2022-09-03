using System.Runtime.InteropServices;
using Windows.System;
using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using WinRT;
using WinRT.Interop;

namespace AutoDL.Utilities;

public static class UIHelper
{
    private const int WaActive = 0x01;
    private const int WaInactive = 0x00;
    private const int WmActivate = 0x0006;

    private static WindowsSystemDispatcherQueueHelper _mWsdqHelper; // See separate sample below for implementation
    private static MicaController _mMicaController;
    private static SystemBackdropConfiguration _mConfigurationSource;
    private static readonly double Scale = GetScaleAdjustment();

    public static IntPtr SettingsWindowHandle => WindowNative.GetWindowHandle(SettingsWindow);

    public static WindowId SettingsWindowId => Win32Interop.GetWindowIdFromWindow(SettingsWindowHandle);

    public static SettingsWindow SettingsWindow => (SettingsWindow)(Application.Current as App).SettingsWindow;

    public static AppWindow AppWindow =>
        AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(SettingsWindow)));

    public static App App => Application.Current as App;

    [DllImport("Shcore.dll", SetLastError = true)]
    private static extern int GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    // ReSharper disable once MemberCanBePrivate.Global
    public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();

    private static double GetScaleAdjustment()
    {
        var displayArea = DisplayArea.GetFromWindowId(
            Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(SettingsWindow)), DisplayAreaFallback.Primary);
        var hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

        // Get DPI.
        var result = GetDpiForMonitor(hMonitor, MonitorDpiType.MdtDefault, out var dpiX, out var _);
        if (result != 0) throw new Exception("Could not get DPI for monitor.");

        var scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
        return scaleFactorPercent / 100.0;
    }

    public static int GetActualPixel(double pixel)
    {
        return Convert.ToInt32(pixel * Scale);
    }

    public static bool TrySetMicaBackdrop()
    {
        if (!MicaController.IsSupported()) return false; // Mica is not supported on this system
        _mWsdqHelper = new WindowsSystemDispatcherQueueHelper();
        _mWsdqHelper.EnsureWindowsSystemDispatcherQueueController();

        // Hooking up the policy object
        _mConfigurationSource = new SystemBackdropConfiguration();
        SettingsWindow.Activated += Window_Activated;
        ((FrameworkElement)SettingsWindow.Content).ActualThemeChanged += Window_ThemeChanged;

        // Initial configuration state.
        _mConfigurationSource.IsInputActive = true;
        SetConfigurationSourceTheme();

        _mMicaController = new MicaController();

        // Enable the system backdrop.
        // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
        _mMicaController.AddSystemBackdropTarget(((MainWindow)(Application.Current as App)?.m_window)
            .As<ICompositionSupportsSystemBackdrop>());
        _mMicaController.SetSystemBackdropConfiguration(_mConfigurationSource);
        return true; // succeeded
    }

    private static void SetConfigurationSourceTheme()
    {
        switch (((FrameworkElement)SettingsWindow.Content).ActualTheme)
        {
            case ElementTheme.Dark:
                _mConfigurationSource.Theme = SystemBackdropTheme.Dark;
                break;
            case ElementTheme.Light:
                _mConfigurationSource.Theme = SystemBackdropTheme.Light;
                break;
            case ElementTheme.Default:
                _mConfigurationSource.Theme = SystemBackdropTheme.Default;
                break;
        }
    }

    private static void Window_ThemeChanged(FrameworkElement sender, object args)
    {
        if (_mConfigurationSource != null) SetConfigurationSourceTheme();
    }

    private static void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        _mConfigurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
    }

    public static void SetTitleBarTransparent()
    {
        var res = Application.Current.Resources;
        res["WindowCaptionBackground"] = Colors.Transparent;
        res["WindowCaptionForeground"] = Colors.Black;

        // 须获取MainWindow句柄和ActiveWindow句柄实例，详见文末备注。
        var activeWindow = GetActiveWindow();
        if (SettingsWindowHandle == activeWindow)
        {
            SendMessage(SettingsWindowHandle, WmActivate, WaInactive, IntPtr.Zero);
            SendMessage(SettingsWindowHandle, WmActivate, WaActive, IntPtr.Zero);
        }
        else
        {
            SendMessage(SettingsWindowHandle, WmActivate, WaActive, IntPtr.Zero);
            SendMessage(SettingsWindowHandle, WmActivate, WaInactive, IntPtr.Zero);
        }
    }

    private enum MonitorDpiType
    {
        MdtEffectiveDpi = 0,
        MdtAngularDpi = 1,
        MdtRawDpi = 2,
        MdtDefault = MdtEffectiveDpi
    }
}

public class WindowsSystemDispatcherQueueHelper
{
    private object _mDispatcherQueueController;

    [DllImport("CoreMessaging.dll")]
    private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options,
        [In] [Out] [MarshalAs(UnmanagedType.IUnknown)]
        ref object dispatcherQueueController);

    public void EnsureWindowsSystemDispatcherQueueController()
    {
        if (DispatcherQueue.GetForCurrentThread() != null)
            // one already exists, so we'll just use it.
            return;

        if (_mDispatcherQueueController == null)
        {
            DispatcherQueueOptions options;
            options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
            options.threadType = 2; // DQTYPE_THREAD_CURRENT
            options.apartmentType = 2; // DQTAT_COM_STA

            CreateDispatcherQueueController(options, ref _mDispatcherQueueController);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct DispatcherQueueOptions
    {
        internal int dwSize;
        internal int threadType;
        internal int apartmentType;
    }
}