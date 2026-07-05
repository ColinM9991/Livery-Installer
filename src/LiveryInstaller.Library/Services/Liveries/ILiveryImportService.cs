using LiveryInstaller.Library.Models.DTO;

namespace LiveryInstaller.Library.Services.Liveries;

/// <summary>
/// Represents a service responsible for loading and importing livery files.
/// </summary>
[LoggingDecorator]
public interface ILiveryImportService
{
    /// <summary>
    /// Loads a livery from a given path.
    /// </summary>
    /// <param name="liveryPath"></param>
    /// <returns></returns>
    Task<LoadedLivery> LoadLiveryAsync(string liveryPath);

    /// <summary>
    /// Imports the specified livery and icon into the catalog.
    /// </summary>
    /// <param name="livery">The specified livery.</param>
    /// <param name="iconFile">The livery icon.</param>
    /// <returns></returns>
    Task ImportLiveryAsync(LoadedLivery livery, string iconFile);

    /// <summary>
    /// Removes a user imported livery from the store.
    /// </summary>
    /// <param name="livery">The livery.</param>
    /// <returns></returns>
    Task RemoveLiveryAsync(LiveryRemoveRequest livery);
}