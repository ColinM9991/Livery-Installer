using LiveryInstaller.Library.Models.DTO;
using LiveryInstaller.Library.Services.Pipeline;
using LiveryInstaller.Library.Services.Pipeline.Steps.InstallLiverySteps;
using LiveryInstaller.Library.Services.Pipeline.Steps.UninstallLiverySteps;

namespace LiveryInstaller.Library.Services.Liveries;

/// <inheritdoc />
public class LiveryInstallService(
    IPipeline<LiveryInstallationContext> installPipeline,
    IPipeline<LiveryUninstallContext> uninstallPipeline)
    : ILiveryInstallService
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <inheritdoc />
    public async Task InstallLiveryAsync(LiveryInstallRequest request)
    {
        await _semaphore.WaitAsync();
        try
        {
            await installPipeline.ExecuteAsync(
                new LiveryInstallationContext(
                    request.SimulatorType,
                    request.LiveryPath,
                    request.VariantName,
                    request.TextureId,
                    request.AircraftName,
                    request.AtcId),
                CancellationToken.None);
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
            await uninstallPipeline.ExecuteAsync(
                new LiveryUninstallContext(
                    request.SimulatorType,
                    request.AircraftName,
                    request.VariantName,
                    request.TextureId,
                    request.AtcId),
                CancellationToken.None);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}