using Velopack;

namespace LiveryInstaller.UI.Services;

public sealed class ApplicationUpdaterService(UpdateManager updateManager) : IApplicationUpdaterService
{
    public async Task<UpdateInfo> CheckForUpdatesAsync() => await updateManager.CheckForUpdatesAsync();

    public async Task ApplyUpdateAsync(UpdateInfo updateInfo)
    {
        await updateManager.DownloadUpdatesAsync(updateInfo);

        updateManager.ApplyUpdatesAndRestart(updateInfo);
    }
}