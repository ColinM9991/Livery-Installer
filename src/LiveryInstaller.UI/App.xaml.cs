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
    private readonly IServiceScope _scope;

    public App(IHost host)
    {
        _scope = host.Services.CreateScope();
        
        InitializeComponent();
    }
    
    private ILogger<App> Logger => _scope.ServiceProvider.GetRequiredService<ILogger<App>>();
    
    protected override void OnStartup(StartupEventArgs e)
    {
        RegisterExceptionHandlers();
        
        base.OnStartup(e);
        
        var mainWindow = _scope.ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _scope?.Dispose();
        base.OnExit(e);
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