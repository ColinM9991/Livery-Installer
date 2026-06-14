using System.Windows;
using LiveryInstaller.UI.Views;

namespace LiveryInstaller.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private Window _mainWindow;
    
    public void SetStartupWindow(Window window)
    {
        _mainWindow = (MainWindow)window;
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(_mainWindow);
        
        base.OnStartup(e);
        
        _mainWindow.Show();
    }
}