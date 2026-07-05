using LiveryInstaller.Library.Models.DTO;
using LiveryInstaller.Library.Models.INI;

namespace LiveryInstaller.Library.Services.Factories;

public interface ILoadedLiveryFactory
{
    LoadedLivery Create(string packagePath, AircraftConfiguration aircraftConfiguration);
}