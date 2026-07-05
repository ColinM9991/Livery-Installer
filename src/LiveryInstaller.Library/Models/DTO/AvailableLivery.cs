namespace LiveryInstaller.Library.Models.DTO;

/// <summary>
/// Represents an available livery.
/// </summary>
public class AvailableLivery
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AvailableLivery"/> class.
    /// </summary>
    /// <param name="aircraft">The aircraft DTO.</param>
    /// <param name="variant">The variant DTO.</param>
    /// <param name="livery">The livery DTO.</param>
    public AvailableLivery(
        AircraftDto aircraft,
        VariantDto variant,
        LiveryDto livery)
    {
        LiveryName = livery.Name;
        Airline = livery.Airline;
        AircraftName = aircraft.Name;
        VariantName = variant.Name;
        TextureId = livery.TextureId;
        AtcId = livery.AtcId;
        IsUserImported = livery.IsUserImported;
    }
    
    public string LiveryName { get; }
    
    public string Airline { get; }

    public string AircraftName { get; }

    public string VariantName { get; }

    public string TextureId { get; }

    public string AtcId { get; }
    
    public bool IsUserImported { get; }

    public bool IsInstalled { get; set; }
    
    public string LiveryPath { get; init; }
    
    public string IconPath { get; init; }
};