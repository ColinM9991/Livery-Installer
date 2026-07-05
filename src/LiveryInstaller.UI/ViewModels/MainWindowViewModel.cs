using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models;
using LiveryInstaller.Library.Models.Configuration;
using LiveryInstaller.UI.Services;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IPage
{
    private readonly INavigationService _navigationService;
    
    public MainWindowViewModel(
        ToastControlViewModel toastControlViewModel,
        INavigationService navigationService,
        IOptionsMonitor<UserSettings> userSettings)
    {
        _navigationService = navigationService;

        IsLiveryPageEnabled = IsLiveryPageAvailable(userSettings.CurrentValue);
        userSettings.OnChange(_ => IsLiveryPageEnabled = IsLiveryPageAvailable(userSettings.CurrentValue));
        
        CurrentPage = IsLiveryPageEnabled
            ? _navigationService.GetPage<LiveryPageViewModel>()
            : _navigationService.GetPage<SettingsPageViewModel>();
        
        ToastControlViewModel = toastControlViewModel;
    }

    public ToastControlViewModel ToastControlViewModel { get; }
    
    [ObservableProperty]
    public partial IPage CurrentPage { get; set; }
    
    [ObservableProperty]
    public partial bool IsLiveryPageEnabled { get; set; }
    
    [RelayCommand]
    public void NavigateToLiveriesPage() => CurrentPage = _navigationService.GetPage<LiveryPageViewModel>();
    
    [RelayCommand]
    public void NavigateToLiveryImportPage() => CurrentPage = _navigationService.GetPage<ImportLiveryPageViewModel>();
    
    [RelayCommand]
    public void NavigateToSettingsPage() => CurrentPage = _navigationService.GetPage<SettingsPageViewModel>();
    
    private bool IsLiveryPageAvailable(UserSettings userSettings) => !string.IsNullOrWhiteSpace(userSettings.LiveriesPath);
}