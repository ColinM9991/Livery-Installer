using LiveryInstaller.Library.Services.Liveries;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.InstallLiverySteps;

internal sealed class LiveryInstallationStep(
    ITextureInstallService textureInstallService)
    : IPipelineStep<LiveryInstallationContext>, ICompensatableStep<LiveryInstallationContext>
{
    private readonly ITextureInstallService _textureInstallService =
        textureInstallService ?? throw new ArgumentNullException(nameof(textureInstallService));

    public Task ExecuteAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        _textureInstallService.InstallTexture(
            context.SimulatorType,
            context.ExtractionPath,
            context.VariantName,
            context.TextureId);

        return Task.CompletedTask;
    }

    public Task UndoAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        _textureInstallService.UninstallTexture(context.SimulatorType, context.VariantName, context.TextureId);

        return Task.CompletedTask;
    }
}