namespace LiveryInstaller.Library.Services;

public interface IFileSystem
{
    void DirectoryCreate(string path);
    
    bool DirectoryExists(string path);

    void DirectoryDelete(string path);

    void DirectoryCopy(string source, string destination);

    bool FileExists(string path);

    void FileDelete(string path);

    void FileCopy(string source, string destination);
}