using System.Windows;
using System.Windows.Input;


namespace BachelorsThesisHelperApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Width = SystemParameters.PrimaryScreenWidth - (SystemParameters.PrimaryScreenWidth * 0.25);
    }
    
    private void CloseButton(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    
    private void MinimizeButton(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }
}