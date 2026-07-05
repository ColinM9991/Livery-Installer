using LiveryInstaller.Library;
using Velopack;

namespace LiveryInstaller.UI.Services;

[LoggingDecorator]
public interface IApplicationUpdaterService
{
    public Task<UpdateInfo> CheckForUpdatesAsync();

    public Task ApplyUpdateAsync(UpdateInfo updateInfo);
}