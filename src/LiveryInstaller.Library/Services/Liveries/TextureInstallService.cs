using LiveryInstaller.Library.Models.Configuration;

namespace LiveryInstaller.Library.Services.Liveries;

/// <inheritdoc />
public sealed class TextureInstallService(
    IFileSystem fileSystem,
    ISimulatorService simulatorService) : ITextureInstallService
{
    /// <inheritdoc />
    public void InstallTexture(SimulatorType simulatorType, string sourceArchiveDirectory, string variantName,
        string textureId)
    {
        var textureDestination = simulatorService.GetLiveryPath(simulatorType, variantName, textureId);
        fileSystem.DirectoryCopy(
            Path.Combine(sourceArchiveDirectory, $"Texture.{textureId}"),
            textureDestination);
    }

    /// <inheritdoc />
    public void UninstallTexture(SimulatorType simulatorType, string variantName, string textureId)
    {
        var liveryPath = simulatorService.GetLiveryPath(simulatorType, variantName, textureId);

        if (fileSystem.DirectoryExists(liveryPath))
            fileSystem.DirectoryDelete(liveryPath);
    }
}