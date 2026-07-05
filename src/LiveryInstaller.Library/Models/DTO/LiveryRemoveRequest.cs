namespace LiveryInstaller.Library.Models.DTO;

public record LiveryRemoveRequest(
    string AircraftName,
    string VariantName,
    string LiveryName,
    string TextureId,
    string PackagePath,
    string IconPath);