using LiveryInstaller.Library.Services.Configuration;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.UninstallLiverySteps;

internal sealed class LiveryConfigurationUninstallStep(
    IAircraftConfigurationService aircraftConfigurationService) : IPipelineStep<LiveryUninstallContext>
{
    private readonly IAircraftConfigurationService _aircraftConfigurationService = aircraftConfigurationService ??
        throw new ArgumentNullException(nameof(aircraftConfigurationService));

    public async Task ExecuteAsync(LiveryUninstallContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _aircraftConfigurationService.RemoveLiveryAsync(context.SimulatorType, context.VariantName, context.AtcId); 
    }
}