using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models;
using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Win32;

namespace LiveryInstaller.UI.ViewModels;

public partial class SettingsPageViewModel(
    ISettingsStore settingsStore,
    IOptionsMonitor<UserSettings> userSettings)
    : ObservableObject, IPage
{
    public LogLevel? LogLevel
    {
        get => field ??= userSettings.CurrentValue.LogLevel;
        set => SetProperty(ref field, value);
    }
    
    public string LiveriesPath
    {
        get => field ??= userSettings.CurrentValue.LiveriesPath;
        set => SetProperty(ref field, value); 
    }

    public string DecryptionKey
    {
        get => field ??= userSettings.CurrentValue.DecryptionKey;
        set => SetProperty(ref field, value);
    }
    
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
    private async Task SaveSettings() => await settingsStore.SaveSettingsAsync(new UserSettings
    {
        LogLevel = LogLevel.GetValueOrDefault(),
        LiveriesPath = LiveriesPath,
        DecryptionKey = DecryptionKey
    });
}