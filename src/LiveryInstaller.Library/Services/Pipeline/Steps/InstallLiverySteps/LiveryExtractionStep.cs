using LiveryInstaller.Library.Services.Liveries;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.InstallLiverySteps;

internal sealed class LiveryExtractionStep(
    ILiveryExtractor extractor,
    IFileSystem fileSystem)
    : IPipelineStep<LiveryInstallationContext>, ICompensatableStep<LiveryInstallationContext>
{
    public async Task ExecuteAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var extractionPath = await extractor.ExtractLiveryAsync(context.LiveryPath);

        context.ExtractionPath = extractionPath;
    }

    public Task UndoAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        fileSystem.DirectoryDelete(context.ExtractionPath);
        
        return Task.CompletedTask;
    }
}