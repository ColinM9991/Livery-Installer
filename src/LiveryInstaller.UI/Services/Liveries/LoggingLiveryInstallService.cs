using LiveryInstaller.UI.Models.DTO;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services.Liveries;

public sealed class LoggingLiveryInstallService(
    ILiveryInstallService liveryInstallService,
    ILogger<LoggingLiveryInstallService> logger) : ILiveryInstallService
{
    public async Task InstallLiveryAsync(LiveryInstallRequest request)
    {
        try
        {
            logger.LogInformation("Processing request: {AircraftName} - {VariantName} - {LiveryPath}",
                request.AircraftName, request.VariantName, request.LiveryPath);

            await liveryInstallService.InstallLiveryAsync(request);

            logger.LogInformation("Finished processing request: {AircraftName} - {VariantName} - {LiveryPath}",
                request.AircraftName, request.VariantName, request.LiveryPath);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error installing livery: {AircraftName} - {VariantName} - {LiveryPath}",
                request.AircraftName, request.VariantName, request.LiveryPath);

            throw;
        }
    }

    public async Task UninstallLiveryAsync(LiveryInstallRequest request)
    {
        try
        {
            logger.LogInformation("Processing request: {AircraftName} - {VariantName} - {LiveryPath}",
                request.AircraftName, request.VariantName, request.LiveryPath);

            await liveryInstallService.UninstallLiveryAsync(request);

            logger.LogInformation("Finished processing request: {AircraftName} - {VariantName} - {LiveryPath}",
                request.AircraftName, request.VariantName, request.LiveryPath);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error uninstalling livery: {AircraftName} - {VariantName} - {LiveryPath}",
                request.AircraftName, request.VariantName, request.LiveryPath);

            throw;
        }
    }
}