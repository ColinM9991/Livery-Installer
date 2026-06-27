using System.IO;
using LiveryInstaller.UI.Models.Configuration;

namespace LiveryInstaller.UI.Services.Configuration;

/// <inheritdoc />
public sealed class VariantConfigurationService(
    IFileSystem fileSystem,
    ISimulatorService simulatorService) : IVariantConfigurationService
{
    /// <inheritdoc />
    public void InstallVariantConfiguration(SimulatorType simulatorType, string sourceArchiveDirectory, string aircraftName, string atcId)
    {
        var aircraftIni = Path.Combine(sourceArchiveDirectory, "Aircraft.ini");
        var targetIni = GetIniPath(simulatorType, aircraftName, atcId);
        
        ArgumentNullException.ThrowIfNull(targetIni);

        if (fileSystem.FileExists(targetIni))
            fileSystem.FileDelete(targetIni);

        fileSystem.FileCopy(aircraftIni, targetIni);
    }

    /// <inheritdoc />
    public void UninstallVariantConfiguration(SimulatorType simulatorType, string aircraftName, string atcId)
    {
        var targetIni = GetIniPath(simulatorType, aircraftName, atcId);

        if (fileSystem.FileExists(targetIni))
            fileSystem.FileDelete(targetIni);
    }

    private string GetIniPath(SimulatorType simulatorType, string aircraftName, string atcId) => Path.Combine(
        simulatorService.GetInstallationPath(simulatorType),
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