using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveryInstaller.UI.Models;
using LiveryInstaller.Library.Models.Configuration;
using LiveryInstaller.Library.Services.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.ViewModels;

public partial class SettingsPageViewModel(
    IWriteableConfigurationStore<UserSettings> writeableConfigurationStore,
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

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task SaveSettings() => await writeableConfigurationStore.WriteAsync(new UserSettings
    {
        LogLevel = LogLevel.GetValueOrDefault(),
        LiveriesPath = LiveriesPath,
        DecryptionKey = DecryptionKey
    });
}