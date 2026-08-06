using LiveryInstaller.Library.Services.Configuration;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.InstallLiverySteps;

internal sealed class LiveryConfigurationStep(IAircraftConfigurationService aircraftConfigurationService)
    : IPipelineStep<LiveryInstallationContext>, ICompensatableStep<LiveryInstallationContext>
{
    public async Task ExecuteAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        await aircraftConfigurationService.AddLiveryAsync(
            context.SimulatorType,
            context.VariantName, 
            context.ExtractionPath);
    }

    public async Task UndoAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        await aircraftConfigurationService.RemoveLiveryAsync(context.SimulatorType, context.VariantName, context.AtcId);
    }
}