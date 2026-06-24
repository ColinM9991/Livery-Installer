using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Models.INI;

namespace LiveryInstaller.UI.Services.Factories;

public interface ILoadedLiveryFactory
{
    LoadedLivery Create(string packagePath, AircraftConfiguration aircraftConfiguration);
}