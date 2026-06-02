using LiveryInstaller.UI.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public class NavigationService(IServiceProvider serviceProvider) : INavigationService
{
    /// <inheritdoc />
    public IPage GetPage<T>() where T : class, IPage => serviceProvider.GetRequiredService<T>();
}