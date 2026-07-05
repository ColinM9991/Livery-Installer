namespace LiveryInstaller.Library.Models.DTO;

/// <summary>
/// Aircraft DTO.
/// </summary>
/// <param name="Name">Aircraft name. </param>
/// <param name="Variants">Aircraft variants. </param>
public record AircraftDto(string Name, IReadOnlyCollection<VariantDto> Variants)
{
    public string DisplayName => $"PMDG {Name}";
}