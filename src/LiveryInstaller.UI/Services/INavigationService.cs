using LiveryInstaller.UI.Models;

namespace LiveryInstaller.UI.Services;

/// <summary>
/// Represents a service that can provide pages.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Gets a page for the presentation layer.
    /// </summary>
    /// <typeparam name="T">The type of page to get.</typeparam>
    /// <returns>The page.</returns>
    IPage GetPage<T>() where T : class, IPage;
}