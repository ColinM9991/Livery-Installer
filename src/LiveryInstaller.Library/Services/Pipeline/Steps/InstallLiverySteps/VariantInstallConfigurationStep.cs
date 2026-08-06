using LiveryInstaller.Library.Services.Configuration;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.InstallLiverySteps;

internal sealed class VariantInstallConfigurationStep(IVariantConfigurationService variantConfigurationService)
    : IPipelineStep<LiveryInstallationContext>, ICompensatableStep<LiveryInstallationContext>
{
    public Task ExecuteAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        variantConfigurationService.InstallVariantConfiguration(
            context.SimulatorType,
            context.ExtractionPath,
            context.AircraftName,
            context.AtcId);

        return Task.CompletedTask;
    }

    public Task UndoAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        variantConfigurationService.UninstallVariantConfiguration(context.SimulatorType, context.AircraftName,
            context.AtcId);

        return Task.CompletedTask;
    }
}