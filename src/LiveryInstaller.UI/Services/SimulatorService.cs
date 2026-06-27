using System.IO;
using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Models.DTO;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public sealed class SimulatorService(
    IFileSystem fileSystem,
    IOptions<SimulatorConfiguration> simulatorConfiguration) : ISimulatorService
{
    private static string AircraftPath => Path.Combine("SimObjects", "Airplanes");

    /// <inheritdoc />
    public IReadOnlyList<InstalledSimulator> GetInstalledSimulators() =>
        simulatorConfiguration.Value.InstallationPaths.Select(x => new InstalledSimulator(x.Key))
            .ToList()
            .AsReadOnly();

    /// <inheritdoc />
    public bool IsAircraftInstalled(SimulatorType simulatorType, string variantName) =>
        fileSystem.DirectoryExists(GetVariantPath(simulatorType, variantName));

    /// <inheritdoc />
    public bool IsLiveryInstalled(SimulatorType simulatorType, string variantName, string textureId) =>
        fileSystem.DirectoryExists(GetLiveryPath(simulatorType, variantName, textureId));

    /// <inheritdoc />
    public string GetLiveryPath(SimulatorType simulatorType, string variantName, string textureId) =>
        Path.Combine(GetVariantPath(simulatorType, variantName), $"texture.{textureId}");

    /// <inheritdoc />
    public string GetVariantConfigurationPath(SimulatorType simulatorType, string variantName)
        => Path.Combine(GetVariantPath(simulatorType, variantName), "aircraft.cfg");

    /// <inheritdoc />
    public string GetInstallationPath(SimulatorType simulatorType)
    {
        return !simulatorConfiguration.Value.InstallationPaths.TryGetValue(simulatorType, out var installationPath)
            ? throw new ArgumentException($"Simulator type {simulatorType} is not supported.")
            : installationPath;
    }

    private string GetVariantPath(SimulatorType simulatorType, string variantName) =>
        Path.Combine(GetInstallationPath(simulatorType), AircraftPath, variantName);
}