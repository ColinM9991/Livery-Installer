using System.IO;
using LiveryInstaller.UI.Models.Configuration;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.Services;

public class LiveryPathProvider(IOptions<UserSettings> userSettings) : ILiveryPathProvider
{
    public string GetIconPath(string aircraftName, string variantName, string liveryName) =>
        Path.Combine(userSettings.Value.LiveriesPath, aircraftName, variantName, "icons", $"{liveryName}.jpg");

    public string GetLiveryPath(string aircraftName, string variantName, string liveryName) =>
        Path.Combine(userSettings.Value.LiveriesPath, aircraftName, variantName, $"{liveryName}.ptp");
}