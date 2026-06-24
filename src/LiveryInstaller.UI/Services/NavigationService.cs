using LiveryInstaller.UI.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public class NavigationService(IServiceProvider serviceProvider) : INavigationService
{
    private IServiceScope _scope;
    
    /// <inheritdoc />
    public IPage GetPage<T>() where T : class, IPage
    {
        _scope?.Dispose();
        _scope = serviceProvider.CreateScope();
        
        return _scope.ServiceProvider.GetRequiredService<T>();
    }
}