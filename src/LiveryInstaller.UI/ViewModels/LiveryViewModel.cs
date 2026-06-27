using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Services.Icons;
using LiveryInstaller.UI.Services.Liveries;

namespace LiveryInstaller.UI.ViewModels;

/// <summary>
/// Represents a view model for a livery.
/// </summary>
/// <param name="simulatorType">The selected simulator.</param>
/// <param name="livery">The livery to represent.</param>
/// <param name="liveryInstallService">The service used to install and uninstall liveries.</param>
public partial class LiveryViewModel(
    SimulatorType simulatorType,
    AvailableLivery livery,
    ILiveryInstallService liveryInstallService,
    IIconService iconService)
    : ObservableObject
{
    private SimulatorType SelectedSimulator => simulatorType;

    public string LiveryName => livery.LiveryName;

    public string Airline => livery.Airline;

    private string AircraftName => livery.AircraftName;

    private string VariantName => livery.VariantName;

    private string LiveryPath => livery.LiveryPath;

    private string IconPath => livery.IconPath;

    public bool IsUserImported => livery.IsUserImported;

    [ObservableProperty] public partial ImageSource Icon { get; set; }

    [ObservableProperty] private partial bool IsIconLoading { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(InstallLiveryCommand))]
    [NotifyCanExecuteChangedFor(nameof(UninstallLiveryCommand))]
    [NotifyPropertyChangedFor(nameof(ShouldShowProgress))]
    private partial bool IsInvokingCommand { get; set; }

    public bool ShouldShowInstallButton => !IsInstalled;

    public bool ShouldShowProgress => IsInvokingCommand;

    public bool ShouldShowUninstallButton => IsInstalled;

    private bool IsInstalled
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

        try
        {
            await Task.Run(() => liveryInstallService.InstallLiveryAsync(new LiveryInstallRequest(SelectedSimulator,
                AircraftName,
                VariantName, livery)));

            IsInstalled = true;
        }
        finally
        {
            IsInvokingCommand = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecuteUninstallLiveryCommand))]
    private async Task UninstallLiveryAsync()
    {
        IsInvokingCommand = true;

        try
        {
            await liveryInstallService.UninstallLiveryAsync(new LiveryInstallRequest(SelectedSimulator, AircraftName,
                VariantName, livery));

            IsInvokingCommand = false;
            IsInstalled = false;
        }
        catch
        {
            // Do nothing, swallow exception. Services log and notify the user
        }
    }

    private bool CanExecuteInstallLiveryCommand() =>
        !IsInvokingCommand && !IsInstalled && !string.IsNullOrWhiteSpace(LiveryPath);

    private bool CanExecuteUninstallLiveryCommand() => !IsInvokingCommand && IsInstalled;
}