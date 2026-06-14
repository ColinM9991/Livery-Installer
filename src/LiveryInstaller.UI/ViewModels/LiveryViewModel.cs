using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Services;

namespace LiveryInstaller.UI.ViewModels;

/// <summary>
/// Represents a view model for a livery.
/// </summary>
/// <param name="livery">The livery to represent.</param>
/// <param name="liveryInstallService">The service used to install and uninstall liveries.</param>
public partial class LiveryViewModel(
    AvailableLivery livery,
    ILiveryInstallService liveryInstallService,
    IIconService iconService)
    : ObservableObject
{
    public string LiveryName => livery.LiveryName;

    public string Airline => livery.Airline;

    private string AircraftName => livery.AircraftName;

    private string VariantName => livery.VariantName;

    private string LiveryPath => livery.LiveryPath;
    
    private string IconPath => livery.IconPath;
    
    [ObservableProperty] public partial ImageSource Icon { get; set; }

    [ObservableProperty] private partial bool IsIconLoading { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(InstallLiveryCommand))]
    [NotifyCanExecuteChangedFor(nameof(UninstallLiveryCommand))]
    [NotifyPropertyChangedFor(nameof(ShouldShowProgress))]
    private partial bool IsInvokingCommand { get; set; }
    
    public Visibility ShouldShowInstallButton => IsInstalled ? Visibility.Hidden : Visibility.Visible;
    
    public Visibility ShouldShowProgress => IsInvokingCommand ? Visibility.Visible : Visibility.Hidden;
    
    public Visibility ShouldShowUninstallButton => IsInstalled ? Visibility.Visible : Visibility.Hidden;
    
    private  bool IsInstalled
    {
        get => livery.IsInstalled;
        set
        {
            if (value == livery.IsInstalled)
                return;
            
            livery.IsInstalled = value;
            OnPropertyChanged();
            
            OnPropertyChanged(nameof(ShouldShowInstallButton));
            OnPropertyChanged(nameof(ShouldShowUninstallButton));
            
            InstallLiveryCommand.NotifyCanExecuteChanged();
            UninstallLiveryCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand]
    private async Task LoadIconAsync()
    {
        if (Icon is not null || IsIconLoading)
            return;

        if (string.IsNullOrWhiteSpace(IconPath))
            return;

        IsIconLoading = true;

        Icon = await iconService.GetIconAsync(IconPath);
        
        IsIconLoading = false;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteInstallLiveryCommand))]
    private async Task InstallLiveryAsync()
    {
        IsInvokingCommand = true;
        
        await liveryInstallService.InstallLiveryAsync(new LiveryInstallRequest(AircraftName, VariantName, LiveryPath, livery.TextureId, livery.AtcId));

        IsInvokingCommand = false;
        IsInstalled = true;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteUninstallLiveryCommand))]
    private async Task UninstallLiveryAsync()
    {
        IsInvokingCommand = true;
        
        await liveryInstallService.UninstallLiveryAsync(new LiveryInstallRequest(AircraftName, VariantName, LiveryPath, livery.TextureId, livery.AtcId));

        IsInvokingCommand = false;
        IsInstalled = false;
    }

    private bool CanExecuteInstallLiveryCommand() =>
        !IsInvokingCommand && !IsInstalled && !string.IsNullOrWhiteSpace(LiveryPath);

    private bool CanExecuteUninstallLiveryCommand() => !IsInvokingCommand && IsInstalled;
}