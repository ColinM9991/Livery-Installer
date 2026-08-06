using LiveryInstaller.Library.Services.Configuration;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.InstallLiverySteps;

internal sealed class LiveryConfigurationStep(IAircraftConfigurationService aircraftConfigurationService) : IPipelineStep<LiveryInstallationContext>
{
    private readonly IAircraftConfigurationService _aircraftConfigurationService =
        aircraftConfigurationService ?? throw new ArgumentNullException(nameof(aircraftConfigurationService));

    public async Task ExecuteAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        await _aircraftConfigurationService.AddLiveryAsync(
            context.SimulatorType,
            context.VariantName, 
            context.ExtractionPath);
    }
}