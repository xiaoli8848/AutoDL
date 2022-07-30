using System.Runtime.InteropServices;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT;
using WinRT.Interop;

namespace AutoDL.Utilities;

public static class UIHelper
{
    private static WindowsSystemDispatcherQueueHelper m_wsdqHelper; // See separate sample below for implementation
    private static Microsoft.UI.Composition.SystemBackdrops.MicaController m_micaController;
    private static Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration m_configurationSource;

    private static MainWindow MainWindow => (MainWindow) (Application.Current as App).m_window;
    private static double _scale = GetScaleAdjustment();
    private enum Monitor_DPI_Type : int
    {
        MDT_Effective_DPI = 0,
        MDT_Angular_DPI = 1,
        MDT_Raw_DPI = 2,
        MDT_Default = MDT_Effective_DPI
    }

    [DllImport("Shcore.dll", SetLastError = true)]
    private static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);

    public static AppWindow GetAppWindow()
    {
        return AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(MainWindow)));
    }

    private static double GetScaleAdjustment()
    {
        var displayArea = DisplayArea.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(MainWindow)), DisplayAreaFallback.Primary);
        var hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

        // Get DPI.
        var result = GetDpiForMonitor(hMonitor, Monitor_DPI_Type.MDT_Default, out var dpiX, out var _);
        if (result != 0) throw new Exception("Could not get DPI for monitor.");

        var scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
        return scaleFactorPercent / 100.0;
    }

    public static int GetActualPixel(double pixel) => Convert.ToInt32(pixel * _scale);

    public static bool TrySetMicaBackdrop()
    {
        if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
        {
            m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Hooking up the policy object
            m_configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
            MainWindow.Activated += Window_Activated;
            ((FrameworkElement)MainWindow.Content).ActualThemeChanged += Window_ThemeChanged;

            // Initial configuration state.
            m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();

            m_micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();

            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            m_micaController.AddSystemBackdropTarget(((MainWindow)(Application.Current as App).m_window)
                .As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            m_micaController.SetSystemBackdropConfiguration(m_configurationSource);
            return true; // succeeded
        }

        return false; // Mica is not supported on this system
    }

    private static void SetConfigurationSourceTheme()
    {
        switch (((FrameworkElement)MainWindow.Content).ActualTheme)
        {
            case ElementTheme.Dark:    m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
            case ElementTheme.Light:   m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
            case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
        }
    }

    private static void Window_ThemeChanged(FrameworkElement sender, object args)
    {
        if (m_configurationSource != null)
        {
            SetConfigurationSourceTheme();
        }
    }

    private static void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
    }


}

public class WindowsSystemDispatcherQueueHelper
{
    [StructLayout(LayoutKind.Sequential)]
    private struct DispatcherQueueOptions
    {
        internal int dwSize;
        internal int threadType;
        internal int apartmentType;
    }

    [DllImport("CoreMessaging.dll")]
    private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options,
        [In] [Out] [MarshalAs(UnmanagedType.IUnknown)]
        ref object dispatcherQueueController);

    private object _mDispatcherQueueController = null;

    public void EnsureWindowsSystemDispatcherQueueController()
    {
        if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
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
}