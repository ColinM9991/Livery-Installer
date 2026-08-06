using LiveryInstaller.Library.Services.Configuration;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.InstallLiverySteps;

internal sealed class VariantInstallConfigurationStep(IVariantConfigurationService variantConfigurationService)
    : IPipelineStep<LiveryInstallationContext>
{
    private readonly IVariantConfigurationService _variantConfigurationService =
        variantConfigurationService ?? throw new ArgumentNullException(nameof(variantConfigurationService));

    public Task ExecuteAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        _variantConfigurationService.InstallVariantConfiguration(
            context.SimulatorType, 
            context.ExtractionPath,
            context.AircraftName,
            context.AtcId);
        
        return Task.CompletedTask;
    }
}