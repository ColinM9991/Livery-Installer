using LiveryInstaller.UI.Models.Configuration;

namespace LiveryInstaller.UI.Models.DTO;

/// <summary>
/// Represents a livery.
/// </summary>
public record LiveryDto
{
    public LiveryDto()
    {
    }

    public LiveryDto(
        string textureId,
        string atcId,
        string name,
        string description,
        string airline,
        string sanitisedName)
    {
        TextureId = textureId;
        AtcId = atcId;
        Name = name;
        Description = description;
        Airline = airline;
        SanitisedName = sanitisedName;
    }

    public LiveryDto(Livery livery)
    {
        TextureId = livery.TextureId;
        AtcId = livery.AtcId;
        Name = livery.Name;
        Description = livery.Description;
        Airline = livery.Airline;
        SanitisedName = livery.SanitisedName;
        IsUserImported = livery.IsUserImported;
    }
    public string TextureId { get; set; }
    
    public string AtcId { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string Airline { get; set; }
    
    public string SanitisedName { get; set; }
    
    public bool IsUserImported { get; set; }
}