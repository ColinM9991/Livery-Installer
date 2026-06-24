using LiveryInstaller.UI.Models.DTO;

namespace LiveryInstaller.UI.Services.Configuration;

public interface ILiveryConfigurationService
{
    Task<bool> IsLiveryInstalledAsync(string aircraftName, string variantName, string liveryName);
    
    Task InstallLiveryAsync(string aircraftName, string variantName, LiveryDto livery);
}