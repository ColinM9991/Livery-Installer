using LiveryInstaller.Library.Models.Configuration;
using LiveryInstaller.Library.Models.DTO;
using LiveryInstaller.Library.Services.Configuration;
using LiveryInstaller.Library.Services.Factories;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.Library.Services.Liveries;

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
        var settings = Path.Combine(extractedPath, "Settings.dat");
        var aircraftConfiguration = await aircraftConfigurationService.LoadAircraftConfigurationAsync(config);
        var aircraftSettings = await aircraftConfigurationService.LoadAircraftSettingsAsync(settings);

        return loadedLiveryFactory.Create(liveryPath, aircraftConfiguration, aircraftSettings);
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

    public async Task RemoveLiveryAsync(LiveryRemoveRequest livery)
    {
        if (fileSystem.FileExists(livery.PackagePath))
            fileSystem.FileDelete(livery.PackagePath);
        
        if (fileSystem.FileExists(livery.IconPath))
            fileSystem.FileDelete(livery.IconPath);
        
        await liveryConfigurationService.RemoveLiveryAsync(livery.AircraftName, livery.VariantName, livery.TextureId);
    }
}