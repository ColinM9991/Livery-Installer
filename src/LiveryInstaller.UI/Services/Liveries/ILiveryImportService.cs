using LiveryInstaller.UI.Models.DTO;

namespace LiveryInstaller.UI.Services.Liveries;

public interface ILiveryImportService
{
    Task<LoadedLivery> LoadLiveryAsync(string liveryPath);

    Task ImportLiveryAsync(LoadedLivery livery, string iconFile);
}