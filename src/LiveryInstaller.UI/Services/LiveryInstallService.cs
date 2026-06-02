using System.IO;
using LiveryInstaller.UI.Models.DTO;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services;

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

            var extractionPath = await Task.Run(() => liveryExtractor.ExtractLivery(request.LiveryPath));
            
            textureInstallService.InstallTexture(extractionPath, request.VariantName, request.TextureId);
            await aircraftConfigurationService.AddLiveryAsync(request.VariantName,
                extractionPath);
            variantConfigurationService.InstallVariantConfiguration(extractionPath, request.AircraftName, request.AtcId);
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
            variantConfigurationService.UninstallVariantConfiguration(request.AircraftName, request.AtcId);
            await aircraftConfigurationService.RemoveLiveryAsync(request.VariantName, request.AtcId);
            textureInstallService.UninstallTexture(request.VariantName, request.TextureId);
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