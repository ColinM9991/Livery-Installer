namespace LiveryInstaller.UI.Models.DTO;

/// <summary>
/// Represents a variant.
/// </summary>
/// <param name="Name">Variant name.</param>
/// <param name="Liveries">Variant liveries.</param>
public record VariantDto(string Name, IList<LiveryDto> Liveries);