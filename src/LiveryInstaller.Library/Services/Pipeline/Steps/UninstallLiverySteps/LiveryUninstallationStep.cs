using LiveryInstaller.Library.Services.Liveries;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.UninstallLiverySteps;

internal sealed class LiveryUninstallationStep(
    ITextureInstallService textureInstallService) : IPipelineStep<LiveryUninstallContext>
{
    private readonly ITextureInstallService _textureInstallService = textureInstallService ??
                                                                     throw new ArgumentNullException(nameof(textureInstallService));
    
    public Task ExecuteAsync(LiveryUninstallContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _textureInstallService.UninstallTexture(context.SimulatorType, context.VariantName, context.TextureId);

        return Task.CompletedTask;
    }
}