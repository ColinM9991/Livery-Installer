namespace LiveryInstaller.Library.Services.Pipeline.Steps.InstallLiverySteps;

public sealed class LiveryPostInstallationStep(IFileSystem fileSystem)
    : IPipelineStep<LiveryInstallationContext>
{
    public Task ExecuteAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        fileSystem.DirectoryDelete(context.ExtractionPath);

        return Task.CompletedTask;
    }
}