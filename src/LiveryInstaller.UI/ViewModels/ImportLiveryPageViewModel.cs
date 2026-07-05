using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models;
using LiveryInstaller.Library.Models.DTO;
using LiveryInstaller.Library.Services.Liveries;
using LiveryInstaller.UI.Services.Icons;

namespace LiveryInstaller.UI.ViewModels;

public partial class ImportLiveryPageViewModel(
    ILiveryImportService liveryImportService,
    IIconService iconService) : ObservableObject, IPage
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

    [ObservableProperty] public partial bool IsLoading { get; set; }

    public bool CanImport => !string.IsNullOrWhiteSpace(LiveryFile) && IsLoaded;

    [RelayCommand(CanExecute = nameof(CanImport))]
    private async Task ImportAsync()
    {
        await liveryImportService.ImportLiveryAsync(LoadedLivery, IconFile);

        IsLoading = false;
        LoadedLivery = null;
        Icon = null;
        LiveryFile = null;
        IconFile = null;
        IsLoaded = false;

        ImportCommand.NotifyCanExecuteChanged();
    }

    async partial void OnLiveryFileChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        try
        {
            LoadedLivery = null;
            Icon = null;
            IsLoaded = false;
            IsLoading = true;

            var livery = await Task.Run(() => liveryImportService.LoadLiveryAsync(value));

            LoadedLivery = livery;
            Icon = await iconService.GetIconAsync(IconFile);

            IsLoaded = livery is not null;
            IsLoading = false;
        }
        catch
        {
            // Swallow exception. User notified via notifications.
        }
    }

    async partial void OnIconFileChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        Icon = await Task.Run(() => iconService.GetIconAsync(value));
    }
}