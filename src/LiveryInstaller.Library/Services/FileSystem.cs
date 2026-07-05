using Microsoft.Extensions.Logging;

namespace LiveryInstaller.Library.Services;

public sealed class FileSystem(ILogger<FileSystem> logger) : IFileSystem
{
    public bool DirectoryExists(string path) => Directory.Exists(path);

    public void DirectoryDelete(string path) => Directory.Delete(path, recursive: true);

    public void DirectoryCopy(string source, string destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        logger.LogInformation("Copying directory {Source} to {Destination}", source, destination);

        if (Directory.Exists(destination))
        {
            logger.LogWarning("Directory {Destination} already exists, deleting it", destination);
            DirectoryDelete(destination);
        }

        DirectoryCreate(destination);

        foreach (var file in Directory.GetFiles(source))
        {
            var targetFile = Path.Combine(destination, Path.GetFileName(file));
            FileCopy(file, targetFile);
        }
    }

    public bool FileExists(string path) => File.Exists(path);

    public void FileDelete(string path) => File.Delete(path);

    public void FileCopy(string source, string destination) => File.Copy(source, destination, overwrite: true);

    public void DirectoryCreate(string path) => Directory.CreateDirectory(path);
}