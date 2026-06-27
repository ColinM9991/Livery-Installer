using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Services.Configuration;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services.Liveries;

/// <inheritdoc />
public class LiveryInstallService(
    ILogger<LiveryInstallService> logger,
    ILiveryExtractor liveryExtractor,
    ITextureInstallService textureInstallService,
    IAircraftConfigurationService aircraftConfigurationService,
    IVariantConfigurationService variantConfigurationService)
    : ILiveryInstallService
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <inheritdoc />
    public async Task InstallLiveryAsync(LiveryInstallRequest request)
    {
        await _semaphore.WaitAsync();
        try
        {
            logger.LogInformation("Processing request: {AircraftName} - {VariantName} - {LiveryPath}",
                request.AircraftName, request.VariantName, request.LiveryPath);

            var extractionPath = await liveryExtractor.ExtractLiveryAsync(request.LiveryPath);
            
            textureInstallService.InstallTexture(request.SimulatorType, extractionPath, request.VariantName, request.TextureId);
            await aircraftConfigurationService.AddLiveryAsync(request.SimulatorType, request.VariantName,
                extractionPath);
            variantConfigurationService.InstallVariantConfiguration(request.SimulatorType, extractionPath, request.AircraftName, request.AtcId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error installing livery: {AircraftName} - {VariantName} - {LiveryPath}",
                request.AircraftName, request.VariantName, request.LiveryPath);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <inheritdoc />
    public async Task UninstallLiveryAsync(LiveryInstallRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        await _semaphore.WaitAsync();
        
        try
        {
            variantConfigurationService.UninstallVariantConfiguration(request.SimulatorType, request.AircraftName, request.AtcId);
            await aircraftConfigurationService.RemoveLiveryAsync(request.SimulatorType, request.VariantName, request.AtcId);
            textureInstallService.UninstallTexture(request.SimulatorType, request.VariantName, request.TextureId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error uninstalling livery: {AircraftName} - {VariantName} - {TextureId}",
                request.AircraftName, request.VariantName, request.TextureId);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}