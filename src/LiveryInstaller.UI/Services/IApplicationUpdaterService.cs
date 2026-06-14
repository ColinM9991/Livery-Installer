using Velopack;

namespace LiveryInstaller.UI.Services;

public interface IApplicationUpdaterService
{
    public Task<UpdateInfo> CheckForUpdatesAsync();

    public Task ApplyUpdateAsync(UpdateInfo updateInfo);
}