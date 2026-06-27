using System.IO;
using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Services.Configuration;
using LiveryInstaller.UI.Services.Factories;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.Services.Liveries;

public sealed class LiveryImportService(
    ILiveryExtractor liveryExtractor,
    IAircraftConfigurationService aircraftConfigurationService,
    ILoadedLiveryFactory loadedLiveryFactory,
    IFileSystem fileSystem,
    ILiveryConfigurationService liveryConfigurationService,
    IOptionsMonitor<UserSettings> userSettings) : ILiveryImportService
{
    public async Task<LoadedLivery> LoadLiveryAsync(string liveryPath)
    {
        var extractedPath = await liveryExtractor.ExtractLiveryAsync(liveryPath);
        var config = Path.Combine(extractedPath, "Config.cfg");
        var aircraftConfiguration = await aircraftConfigurationService.LoadAircraftConfigurationAsync(config);

        return loadedLiveryFactory.Create(liveryPath, aircraftConfiguration);
    }

    public async Task ImportLiveryAsync(LoadedLivery livery, string iconFile)
    {
        var variantDirectory =
            Path.Combine(userSettings.CurrentValue.LiveriesPath, livery.AircraftFamily, livery.Variant);

        var iconsPath = Path.Combine(variantDirectory, "icons");

        fileSystem.DirectoryCreate(variantDirectory);
        fileSystem.DirectoryCreate(iconsPath);
        if (!string.IsNullOrEmpty(iconFile))
        {
            var fileExtension = Path.GetExtension(iconFile);
            fileSystem.FileCopy(iconFile, Path.Combine(iconsPath, $"{livery.Livery.SanitisedName}{fileExtension}"));
        }

        fileSystem.FileCopy(livery.PackagePath, Path.Combine(variantDirectory, $"{livery.Livery.SanitisedName}.ptp"));

        await liveryConfigurationService.InstallLiveryAsync(livery.AircraftFamily, livery.Variant, livery.Livery);
    }
}