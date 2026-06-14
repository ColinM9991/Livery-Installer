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

    public bool IsAircraftPathValid(string aircraftName) =>
        !string.IsNullOrWhiteSpace(userSettings.CurrentValue.LiveriesPath) &&
        Directory.Exists(Path.Combine(userSettings.CurrentValue.LiveriesPath,
            aircraftName));

    public bool IsVariantPathValid(string aircraftName, string variantName) =>
        !string.IsNullOrWhiteSpace(userSettings.CurrentValue.LiveriesPath) &&
        Directory.Exists(Path.Combine(userSettings.CurrentValue.LiveriesPath,
            aircraftName, variantName));

    public bool IsLiveryPathValid(string aircraftName, string variantName, string liveryName) =>
        !string.IsNullOrWhiteSpace(userSettings.CurrentValue.LiveriesPath) &&
        File.Exists(Path.Combine(userSettings.CurrentValue.LiveriesPath,
            aircraftName, variantName, $"{liveryName}.ptp"));
}