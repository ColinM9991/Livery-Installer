using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models;
using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Services;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IPage
{
    private readonly INavigationService _navigationService;
    
    public MainWindowViewModel(
        INavigationService navigationService,
        IOptionsMonitor<UserSettings> userSettings)
    {
        _navigationService = navigationService;
        
        IsLiveryPageEnabled = !string.IsNullOrWhiteSpace(userSettings.CurrentValue.LiveriesPath);
        
        userSettings.OnChange(_ => IsLiveryPageEnabled = !string.IsNullOrWhiteSpace(userSettings.CurrentValue.LiveriesPath));
        
        CurrentPage = IsLiveryPageEnabled
            ? _navigationService.GetPage<LiveryPageViewModel>()
            : _navigationService.GetPage<SettingsPageViewModel>();
    }

    [ObservableProperty]
    public partial IPage CurrentPage { get; set; }
    
    [ObservableProperty]
    public partial bool IsLiveryPageEnabled { get; set; }
    
    [RelayCommand]
    public void NavigateToLiveriesPage() => CurrentPage = _navigationService.GetPage<LiveryPageViewModel>();
    
    [RelayCommand]
    public void NavigateToSettingsPage() => CurrentPage = _navigationService.GetPage<SettingsPageViewModel>();
}