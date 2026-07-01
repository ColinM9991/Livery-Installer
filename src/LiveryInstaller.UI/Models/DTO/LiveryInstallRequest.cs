using LiveryInstaller.UI.Models.Configuration;

namespace LiveryInstaller.UI.Models.DTO;

/// <summary>
/// Represents a livery install request.
/// </summary>
/// <param name="SimulatorType">The simulator the texture is being added for.</param>
/// <param name="AircraftName">Aircraft name.</param>
/// <param name="VariantName">Variant name.</param>
/// <param name="LiveryName">Livery name.</param>
/// <param name="LiveryPath">Livery path.</param>
/// <param name="TextureId">Texture ID.</param>
/// <param name="AtcId">ATC ID.</param>
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