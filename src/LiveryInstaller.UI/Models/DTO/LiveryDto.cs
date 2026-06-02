namespace LiveryInstaller.UI.Models.DTO;

/// <summary>
/// Represents a livery.
/// </summary>
/// <param name="TextureId">Texture ID.</param>
/// <param name="AtcId">ATC ID.</param>
/// <param name="Name">Livery name.</param>
/// <param name="Description">Livery description.</param>
/// <param name="Airline">Livery airline.</param>
/// <param name="SanitisedName">Sanitised livery name.</param>
public record LiveryDto(
    string TextureId,
    string AtcId,
    string Name,
    string Description,
    string Airline,
    string SanitisedName);