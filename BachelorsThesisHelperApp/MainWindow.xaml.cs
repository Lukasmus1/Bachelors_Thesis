using BachelorsThesisHelperApp.Helpers;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BachelorsThesisHelperApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(100);

        timer.Tick += Timer_Tick;

        timer.Start();

        ForceFocus();
    }
    
    private void CloseButton(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    
    private void MinimizeButton(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        
    }

    private async void ForceFocus()
    {
        Type shellType = Type.GetTypeFromProgID("Shell.Application");
        object shellObject = Activator.CreateInstance(shellType);
        shellType.InvokeMember("MinimizeAll", BindingFlags.InvokeMethod, null, shellObject, null);

        // Slight delay to finish the animation
        await Task.Delay(100);

        Topmost = true;
        Activate();
    }
}