using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models;
using LiveryInstaller.UI.Services;

namespace LiveryInstaller.UI.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IPage
{
    private readonly INavigationService _navigationService;
    
    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        
        CurrentPage = navigationService.GetPage<LiveryPageViewModel>();
    }

    [ObservableProperty]
    public partial IPage CurrentPage { get; set; }
    
    [RelayCommand]
    public void NavigateToLiveriesPage() => CurrentPage = _navigationService.GetPage<LiveryPageViewModel>();

    
    [RelayCommand]
    public void NavigateToSettingsPage() => CurrentPage = _navigationService.GetPage<SettingsPageViewModel>();
}