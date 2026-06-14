using System.Windows;
using System.Windows.Threading;
using LiveryInstaller.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App(IHost host)
    {
        _host = host;
        
        InitializeComponent();
    }
    
    private ILogger<App> Logger => _host.Services.GetRequiredService<ILogger<App>>();
    
    protected override void OnStartup(StartupEventArgs e)
    {
        RegisterExceptionHandlers();
        
        base.OnStartup(e);
        
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void RegisterExceptionHandlers()
    {
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        DispatcherUnhandledException += OnDispatcherUnhandledException;
    }
    
    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            Logger.LogCritical(ex, "Unhandled exception");
        }
        else
        {
            Logger.LogCritical("Unhandled non-exception object: {Object}", e.ExceptionObject);
        }
    }

    private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        Logger.LogCritical(e.Exception, "Unobserved task exception");
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Logger.LogCritical(e.Exception, "Dispatcher unhandled exception");

        e.Handled = false;
    }
}