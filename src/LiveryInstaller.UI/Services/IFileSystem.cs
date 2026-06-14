namespace LiveryInstaller.UI.Services;

public interface IFileSystem
{

    bool DirectoryExists(string path);

    void DirectoryDelete(string path);

    void DirectoryCopy(string source, string destination);

    bool FileExists(string path);

    void FileDelete(string path);

    void FileCopy(string source, string destination);
}