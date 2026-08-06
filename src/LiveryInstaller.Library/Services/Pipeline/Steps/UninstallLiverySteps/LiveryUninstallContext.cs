using LiveryInstaller.Library.Models.Configuration;

namespace LiveryInstaller.Library.Services.Pipeline.Steps.UninstallLiverySteps;

public record LiveryUninstallContext(
    SimulatorType SimulatorType,
    string AircraftName,
    string VariantName,
    string TextureId,
    string AtcId);