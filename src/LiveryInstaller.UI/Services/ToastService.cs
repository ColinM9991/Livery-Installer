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

    /// <summary>
    /// Raises a <see cref="ToastMessage"/> with the given level and message.
    /// </summary>
    /// <param name="level">The toast level.</param>
    /// <param name="message">The message to display.</param>
    private void SubmitToast(ToastLevel level, string message)
    {
        messenger.Send(new ToastMessage(message, level));
    }
}