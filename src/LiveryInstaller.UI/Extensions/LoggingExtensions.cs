using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace LiveryInstaller.UI.Extensions;

public static class LoggingExtensions
{
    public static HostApplicationBuilder UseAppLogging(this HostApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        builder.Services.AddSingleton<LoggingLevelSwitch>();

        builder.Services.AddSerilog((services, logger) =>
        {
            var levelSwitch = services.GetRequiredService<LoggingLevelSwitch>();

            logger
                .ReadFrom.Configuration(builder.Configuration)
                .MinimumLevel.ControlledBy(levelSwitch);
        });

        return builder;
    }
}