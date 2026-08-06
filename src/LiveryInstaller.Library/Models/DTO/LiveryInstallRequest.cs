using LiveryInstaller.Library.Models.Configuration;

namespace LiveryInstaller.Library.Models.DTO;

/// <summary>
/// Represents a livery install request.
/// </summary>
/// <param name="SimulatorType">The simulator the texture is being added for.</param>
/// <param name="AircraftName">Aircraft name.</param>
/// <param name="VariantName">Variant name.</param>
/// <param name="Livery">The <see cref="AvailableLivery"/>.</param>
public record LiveryInstallRequest(
    SimulatorType SimulatorType,
    string AircraftName,
    string VariantName,
    AvailableLivery Livery)
{
    public string LiveryName => Livery.LiveryName;

    public string LiveryPath => Livery.LiveryPath;
    
    public string TextureId => Livery.TextureId;
    
    public string AtcId => Livery.AtcId;   
}