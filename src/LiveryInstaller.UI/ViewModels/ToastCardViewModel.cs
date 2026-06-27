using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models.Toast;

namespace LiveryInstaller.UI.ViewModels;

public partial class ToastCardViewModel : ObservableObject
{
    private readonly Action<ToastCardViewModel> _close;
    private readonly DispatcherTimer _timer;

    public ToastCardViewModel(
        string description,
        ToastLevel level,
        Action<ToastCardViewModel> close)
    {
        Description = description;
        Level = level;
        _close = close;
        
        _timer = new DispatcherTimer(TimeSpan.FromSeconds(5), DispatcherPriority.Normal, (_, _) => _close(this), Dispatcher.CurrentDispatcher);
        _timer.Start();
    }

    public ToastCardViewModel(
        string title,
        string description,
        ToastLevel level,
        Action<ToastCardViewModel> close)
        : this(title, level, close)
    {
        Description = description;
    }

    public string Title { get; }

    public string Description { get; }

    public ToastLevel Level { get; }
    
    public bool IsTitleVisible => !string.IsNullOrWhiteSpace(Title);

    [RelayCommand]
    public void Close()
    {
        _timer?.Stop();
        _close(this);
    }
}