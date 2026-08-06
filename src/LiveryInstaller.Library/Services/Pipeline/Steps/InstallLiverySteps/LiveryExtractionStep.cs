using LiveryInstaller.Library.Services.Liveries;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.InstallLiverySteps;

internal sealed class LiveryExtractionStep(ILiveryExtractor extractor) : IPipelineStep<LiveryInstallationContext>
{
    private readonly ILiveryExtractor _extractor = extractor ?? throw new ArgumentNullException(nameof(extractor));
    
    public async Task ExecuteAsync(LiveryInstallationContext context, CancellationToken cancellationToken)
    {
        var extractionPath = await _extractor.ExtractLiveryAsync(context.LiveryPath);
        
        context.ExtractionPath = extractionPath;
    }
}