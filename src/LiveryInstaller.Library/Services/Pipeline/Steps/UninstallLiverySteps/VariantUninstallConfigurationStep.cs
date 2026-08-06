using LiveryInstaller.Library.Services.Configuration;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.UninstallLiverySteps;

internal sealed class VariantUninstallConfigurationStep(
    IVariantConfigurationService variantConfigurationService) : IPipelineStep<LiveryUninstallContext>
{
    private readonly IVariantConfigurationService _variantConfigurationService = variantConfigurationService ??
        throw new ArgumentNullException(nameof(variantConfigurationService));

    public Task ExecuteAsync(LiveryUninstallContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _variantConfigurationService.UninstallVariantConfiguration(context.SimulatorType, context.AircraftName,
            context.AtcId);

        return Task.CompletedTask;
    }
}