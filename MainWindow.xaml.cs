using System.Drawing;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Navigation;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using Windows.UI;
using Windows.UI.Shell;
using AutoDL.Controls;
using AutoDL.Utilities;
using Microsoft.UI.Text;
using WinRT;
using WinRT.Interop;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoDL
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            MainGrid.DataContext = ((App.Current) as App).SettingsViewModel;
            this.Activated += (_, _) =>
            {
                UIHelper.GetAppWindow().Resize(new SizeInt32(UIHelper.GetActualPixel(700), UIHelper.GetActualPixel(800)));
            };
            _ = DayTimeCalculation._locator;
            
        }
    }
}