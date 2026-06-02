using LiveryInstaller.UI.Models.DTO;

namespace LiveryInstaller.UI.Services;

/// <summary>
/// Represents a service that can install and uninstall liveries.
/// </summary>
public interface ILiveryInstallService
{
    /// <summary>
    /// Installs a requested livery.
    /// </summary>
    /// <param name="request">A <see cref="LiveryInstallRequest"/> containing the details of the livery to be installed.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task InstallLiveryAsync(LiveryInstallRequest request);
    
    /// <summary>
    /// Uninstalls a requested livery.
    /// </summary>
    /// <param name="request">A <see cref="LiveryInstallRequest"/> containing the details of the livery to be uninstalled.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task UninstallLiveryAsync(LiveryInstallRequest request);
}