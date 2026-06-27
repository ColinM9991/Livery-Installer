using CommunityToolkit.Mvvm.Messaging;
using LiveryInstaller.UI.Models.Toast;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public sealed class ToastService(IMessenger messenger) : IToastService
{
    /// <inheritdoc />
    public void Information(string message) => SubmitToast(ToastLevel.Information, message);
    
    /// <inheritdoc />
    public void Success(string message) => SubmitToast(ToastLevel.Success, message);
    
    /// <inheritdoc />
    public void Warning(string message) =>SubmitToast(ToastLevel.Warning, message);

    /// <inheritdoc />
    public void Error(string message) => SubmitToast(ToastLevel.Error, message);

    private void SubmitToast(ToastLevel level, string message)
    {
        messenger.Send(new ToastMessage(message, level));
    }
}