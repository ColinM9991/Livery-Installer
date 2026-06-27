using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LiveryInstaller.UI.Models.Toast;

namespace LiveryInstaller.UI.ViewModels;

public class ToastControlViewModel : ObservableRecipient, IRecipient<ToastMessage>
{
    private const int MaxToasts = 5;
    private const int OldestToastIndex = 0;

    public ToastControlViewModel(IMessenger messenger) : base(messenger)
    {
        IsActive = true;
    }

    public ObservableCollection<ToastCardViewModel> ToastNotifications { get; } =
        new(new List<ToastCardViewModel>(MaxToasts));

    public void Receive(ToastMessage message)
    {
        var dispatcher = Application.Current.Dispatcher;
        if (dispatcher.CheckAccess())
            AddToast(message);
        else
            dispatcher.InvokeAsync(() => AddToast(message));
    }

    private void AddToast(ToastMessage message)
    {
        if (ToastNotifications.Count == MaxToasts)
        {
            ToastNotifications.RemoveAt(OldestToastIndex);
        }

        ToastNotifications.Add(new ToastCardViewModel(
            message.Description,
            message.Level,
            vm => ToastNotifications.Remove(vm)));
    }
}