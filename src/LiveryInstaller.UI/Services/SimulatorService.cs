using System.IO;
using LiveryInstaller.UI.Models.Configuration;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public sealed class SimulatorService(
    IFileSystem fileSystem,
    IOptions<SimulatorConfiguration> simulatorConfiguration) : ISimulatorService
{
    private string InstallationPath => simulatorConfiguration.Value.InstallationPath;

    private string AircraftPath => Path.Combine(InstallationPath, "SimObjects", "Airplanes");

    /// <inheritdoc />
    public bool IsAircraftInstalled(string variantName) =>
        fileSystem.DirectoryExists(GetVariantPath(variantName));

    /// <inheritdoc />
    public bool IsLiveryInstalled(string variantName, string textureId) =>
        fileSystem.DirectoryExists(GetLiveryPath(variantName, textureId));

    /// <inheritdoc />
    public string GetLiveryPath(string variantName, string textureId) =>
        Path.Combine(AircraftPath, variantName, $"texture.{textureId}");

    /// <inheritdoc />
    public string GetVariantConfigurationPath(string variantName)
        => Path.Combine(GetVariantPath(variantName), "aircraft.cfg");

    /// <inheritdoc />
    public string GetInstallationPath() => InstallationPath;

    private string GetVariantPath(string variantName) => Path.Combine(AircraftPath, variantName);
}