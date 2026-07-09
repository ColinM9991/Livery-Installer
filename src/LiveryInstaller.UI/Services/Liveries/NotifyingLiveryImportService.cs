using LiveryInstaller.Library.Models.DTO;
using LiveryInstaller.Library.Services.Liveries;

namespace LiveryInstaller.UI.Services.Liveries;

public sealed class NotifyingLiveryImportService(
    ILiveryImportService liveryImportService,
    IToastService toastService) : ILiveryImportService
{
    public async Task<LoadedLivery> LoadLiveryAsync(string liveryPath)
    {
        try
        {
            return await liveryImportService.LoadLiveryAsync(liveryPath);
        }
        catch
        {
            toastService.Error("An error occurred when loading the livery. Please refer to the log");
            throw;
        }
    }

    public async Task ImportLiveryAsync(LoadedLivery livery, string iconFile)
    {
        try
        {
            toastService.Information($"Importing {livery.Livery.Name}");

            await liveryImportService.ImportLiveryAsync(livery, iconFile);

            toastService.Success("Livery imported successfully");
        }
        catch (InvalidOperationException ex)
        {
            toastService.Error(ex.Message);
            throw;
        }
        catch
        {
            toastService.Error($"Failed to import livery: {livery.Livery.Name}. Please refer to the log");
            throw;
        }
    }

    public async Task RemoveLiveryAsync(LiveryRemoveRequest livery)
    {
        try
        {
            toastService.Information($"Removing user livery {livery.LiveryName}");

            await liveryImportService.RemoveLiveryAsync(livery);
            
            toastService.Success("Livery removed successfully");
        }
        catch
        {
            toastService.Error($"Failed to remove livery: {livery.LiveryName}. Please refer to the log");
            throw;
        }
    }
}