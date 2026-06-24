using System.IO;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services.Configuration;

/// <inheritdoc />
public sealed class VariantConfigurationService(
    IFileSystem fileSystem,
    ISimulatorService simulatorService,
    ILogger<VariantConfigurationService> logger) : IVariantConfigurationService
{
    /// <inheritdoc />
    public void InstallVariantConfiguration(string sourceArchiveDirectory, string aircraftName, string atcId)
    {
        logger.LogInformation("Installing variant configuration for aircraft {AircraftName} with ATC ID {AtcId}", aircraftName, atcId);
        
        var aircraftIni = Path.Combine(sourceArchiveDirectory, "Aircraft.ini");
        var targetIni = GetIniPath(aircraftName, atcId);
        
        ArgumentNullException.ThrowIfNull(targetIni);

        if (fileSystem.FileExists(targetIni))
            fileSystem.FileDelete(targetIni);

        fileSystem.FileCopy(aircraftIni, targetIni);
    }

    /// <inheritdoc />
    public void UninstallVariantConfiguration(string aircraftName, string atcId)
    {
        logger.LogInformation("Uninstalling variant configuration for aircraft {AircraftName} with ATC ID {AtcId}", aircraftName, atcId);
        
        var targetIni = GetIniPath(aircraftName, atcId);

        if (fileSystem.FileExists(targetIni))
            fileSystem.FileDelete(targetIni);
    }

    private string GetIniPath(string aircraftName, string atcId) => Path.Combine(
        simulatorService.GetInstallationPath(),
        "PMDG",
        GetAircraftPath(aircraftName), "Aircraft", $"{atcId}.ini");

    private static string GetAircraftPath(string aircraftName) => aircraftName switch
    {
        "737" => "PMDG 737 NGXu",
        "747" => "PMDG 747 QOTS II",
        "777" => "PMDG 777X",
        _ => throw new ArgumentOutOfRangeException(nameof(aircraftName))
    };
}