using LiveryInstaller.UI.Models.DTO;

namespace LiveryInstaller.UI.Services.Liveries;

public sealed class NotifyingLiveryInstallService(
    ILiveryInstallService liveryInstallService,
    IToastService toastService) : ILiveryInstallService
{
    public async Task InstallLiveryAsync(LiveryInstallRequest request)
    {
        try
        {
            toastService.Information($"Installing livery {request.LiveryName}");

            await liveryInstallService.InstallLiveryAsync(request);

            toastService.Success("Livery installed successfully");
        }
        catch
        {
            toastService.Error("Failed to install livery. Please refer to the log");
            throw;
        }
    }

    public async Task UninstallLiveryAsync(LiveryInstallRequest request)
    {
        try
        {
            toastService.Information($"Uninstalling livery {request.LiveryName}");
            
            await liveryInstallService.UninstallLiveryAsync(request);

            toastService.Success("Livery uninstalled successfully");
        }
        catch
        {
            toastService.Error("Failed to uninstall livery. Please refer to the log");
            throw;
        }
    }
}