using LiveryInstaller.Library.Models;
using LiveryInstaller.Library.Models.Configuration;
using LiveryInstaller.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog.Core;
using Velopack;

namespace LiveryInstaller.UI.Extensions;

public static class HostExtensions
{
    extension(IHost host)
    {
        public void InitializeRuntimeServices()
        {
            var services = host.Services;

            var userSettings = services.GetRequiredService<IOptionsMonitor<UserSettings>>();
            var loggingLevelSwitch = services.GetRequiredService<LoggingLevelSwitch>();
            var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();

            userSettings.OnChangeWithInitial(x => loggingLevelSwitch.MinimumLevel = x.LogLevel.ToSerilogLevel());

            VelopackApp.Build().Run();
            if (!hostEnvironment.IsProduction()) return;
        
            var applicationUpdater = services.GetRequiredService<IApplicationUpdaterService>();
            var updateInfo = applicationUpdater.CheckForUpdatesAsync().GetAwaiter().GetResult();
            if (updateInfo is not null)
            {
                applicationUpdater.ApplyUpdateAsync(updateInfo).GetAwaiter().GetResult();
            }
        }

        public void RunApp()
        {
            _ = new App(host).Run();
        }
    }
}