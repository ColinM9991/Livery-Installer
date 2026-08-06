using LiveryInstaller.Library.Models.Configuration;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.InstallLiverySteps;

public sealed record LiveryInstallationContext(
    SimulatorType SimulatorType,
    string LiveryPath,
    string VariantName,
    string TextureId,
    string AircraftName,
    string AtcId)
{
    internal string ExtractionPath { get; set; }
}