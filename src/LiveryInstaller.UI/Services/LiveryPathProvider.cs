using System.IO;
using LiveryInstaller.UI.Models.Configuration;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.Services;

public class LiveryPathProvider(IOptionsMonitor<UserSettings> userSettings) : ILiveryPathProvider
{
    public string GetIconPath(string aircraftName, string variantName, string liveryName) =>
        Path.Combine(userSettings.CurrentValue.LiveriesPath, aircraftName, variantName, "icons", $"{liveryName}.jpg");

    public string GetLiveryPath(string aircraftName, string variantName, string liveryName) =>
        Path.Combine(userSettings.CurrentValue.LiveriesPath, aircraftName, variantName, $"{liveryName}.ptp");

    public bool IsLiveryPathValid(string aircraftName) => !string.IsNullOrWhiteSpace(userSettings.CurrentValue.LiveriesPath) &&
                                                          Path.Exists(Path.Combine(userSettings.CurrentValue.LiveriesPath,
                                                              aircraftName));
}