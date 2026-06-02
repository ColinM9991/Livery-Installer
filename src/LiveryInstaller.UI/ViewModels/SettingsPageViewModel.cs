using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models;
using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Win32;

namespace LiveryInstaller.UI.ViewModels;

public partial class SettingsPageViewModel : ObservableObject, IPage
{
    private readonly ISettingsStore _settingsStore;
    
    public SettingsPageViewModel(
        ISettingsStore settingsStore,
        IOptions<UserSettings> userSettings)
    {
        _settingsStore = settingsStore;
        
        LogLevel = userSettings.Value.LogLevel;
        LiveriesPath = userSettings.Value.LiveriesPath;
        DecryptionKey = userSettings.Value.DecryptionKey;
    }
    
    [ObservableProperty]
    public partial LogLevel LogLevel { get; set; }
    
    [ObservableProperty]
    public partial string LiveriesPath { get; set; }
    
    [ObservableProperty]
    public partial string DecryptionKey { get; set; }
    
    public static IReadOnlyCollection<LogLevel> AvailableLogLevels => Enum.GetValues<LogLevel>();

    [RelayCommand]
    private void FolderPicker()
    {
        var openFolderDialog = new OpenFolderDialog();
        var result = openFolderDialog.ShowDialog();
        
        if (result == true)
        {
            LiveriesPath = openFolderDialog.FolderName;
        }
    }
    
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task SaveSettings() => await _settingsStore.SaveSettingsAsync(new UserSettings
    {
        LogLevel = LogLevel,
        LiveriesPath = LiveriesPath,
        DecryptionKey = DecryptionKey
    });
}