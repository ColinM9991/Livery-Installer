using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models;
using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Services;
using LiveryInstaller.UI.Services.Icons;
using LiveryInstaller.UI.Services.Liveries;

namespace LiveryInstaller.UI.ViewModels;

public partial class ImportLiveryPageViewModel(
    ILiveryImportService liveryImportService,
    IIconService iconService,
    IToastService toastService) : ObservableObject, IPage
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanImport))]
    [NotifyCanExecuteChangedFor(nameof(ImportCommand))]
    public partial string LiveryFile { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IconFile))]
    public partial bool UseCustomIcon { get; set; }

    [ObservableProperty] public partial string IconFile { get; set; }

    [ObservableProperty] public partial ImageSource Icon { get; set; }

    [ObservableProperty] public partial LoadedLivery LoadedLivery { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanImport))]
    [NotifyCanExecuteChangedFor(nameof(ImportCommand))]
    public partial bool IsLoaded { get; set; }

    [ObservableProperty] public partial bool WasImportSuccessful { get; set; }

    [ObservableProperty] public partial bool IsLoading { get; set; }

    public bool CanImport => !string.IsNullOrWhiteSpace(LiveryFile) && IsLoaded;

    [RelayCommand(CanExecute = nameof(CanImport))]
    private async Task ImportAsync()
    {
        try
        {
            toastService.Information($"Importing livery {LoadedLivery.Livery.Name}");

            await liveryImportService.ImportLiveryAsync(LoadedLivery, IconFile);

            toastService.Success("Livery imported successfully");

            IsLoading = false;
            LoadedLivery = null;
            Icon = null;
            LiveryFile = null;
            IconFile = null;
            IsLoaded = false;

            WasImportSuccessful = true;

            ImportCommand.NotifyCanExecuteChanged();
        }
        catch
        {
            toastService.Error("Failed to import livery. Please refer to the log");
            WasImportSuccessful = false;
        }
    }

    async partial void OnLiveryFileChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        LoadedLivery = null;
        Icon = null;
        IsLoaded = false;

        IsLoading = true;
        WasImportSuccessful = false;

        var livery = await Task.Run(() => liveryImportService.LoadLiveryAsync(value));

        LoadedLivery = livery;
        Icon = await iconService.GetIconAsync(IconFile);

        IsLoaded = livery is not null;
        IsLoading = false;
    }

    async partial void OnIconFileChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        WasImportSuccessful = false;
        Icon = await Task.Run(() => iconService.GetIconAsync(value));
    }
}