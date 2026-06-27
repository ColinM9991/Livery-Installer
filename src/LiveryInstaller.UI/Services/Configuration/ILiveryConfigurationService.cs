using LiveryInstaller.UI.Models.DTO;

namespace LiveryInstaller.UI.Services.Configuration;

[LoggingDecorator]
public interface ILiveryConfigurationService
{
    Task InstallLiveryAsync(string aircraftName, string variantName, LiveryDto livery);
}