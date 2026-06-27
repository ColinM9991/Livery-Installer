using LiveryInstaller.UI.Models.DTO;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services.Liveries;

public sealed class LoggingLiveryImportService(
    ILiveryImportService liveryImportService,
    ILogger<LoggingLiveryImportService> logger) : ILiveryImportService
{
    public async Task<LoadedLivery> LoadLiveryAsync(string liveryPath)
    {
        try
        {
            logger.LogInformation("Loading livery from path: {LiveryPath}", liveryPath);

            var livery = await liveryImportService.LoadLiveryAsync(liveryPath);

            logger.LogInformation("Finished loading livery from path: {LiveryPath}", liveryPath);

            return livery;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to load livery from path: {LiveryPath}", liveryPath);
            throw;
        }
    }

    public async Task ImportLiveryAsync(LoadedLivery livery, string iconFile)
    {
        try
        {
            logger.LogInformation("Importing livery: {LiveryName}", livery.Livery.Name);

            await liveryImportService.ImportLiveryAsync(livery, iconFile);

            logger.LogInformation("Finished importing livery: {LiveryName}", livery.Livery.Name);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to import livery: {LiveryName}", livery.Livery.Name);
            throw;
        }
    }
}