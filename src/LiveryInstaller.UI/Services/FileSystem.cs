using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services;

public sealed partial class FileSystem(ILogger<FileSystem> logger) : IFileSystem
{
    [LibraryImport("kernel32.dll", EntryPoint = "CreateDirectoryW", SetLastError = true,
        StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CreateDirectoryW(string lpPathName, IntPtr lpSecurityAttributes);

    [LibraryImport("kernel32.dll", EntryPoint = "RemoveDirectoryW", SetLastError = true,
        StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool RemoveDirectoryW(string lpPathName);

    [LibraryImport("kernel32.dll", EntryPoint = "CopyFileW", SetLastError = true,
        StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CopyFileW(string lpExistingFileName, string lpNewFileName,
        [MarshalAs(UnmanagedType.Bool)] bool bFailIfExists);

    [LibraryImport("kernel32.dll", EntryPoint = "DeleteFileW", SetLastError = true,
        StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool DeleteFile(string lpFileName);

    public bool DirectoryExists(string path) => Directory.Exists(path);

    public void DirectoryDelete(string path) => DirectoryDeleteRecursive(path);

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

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public void FileDelete(string path) => Invoke(() => File.Delete(path), () => DeleteFile(ToExtendedPath(path)));

    public void FileCopy(string source, string destination) => Invoke(
        () => File.Copy(source, destination, overwrite: true),
        () => CopyFileW(ToExtendedPath(source), ToExtendedPath(destination), false));

    private void DirectoryCreate(string path) => Invoke(() => Directory.CreateDirectory(path),
        () => CreateDirectoryW(ToExtendedPath(path), IntPtr.Zero));

    private void DirectoryDeleteRecursive(string path) => Invoke(
        () => Directory.Delete(path, recursive: true),
        () =>
        {
            var extended = ToExtendedPath(path);

            // Recurse into subdirectories first
            foreach (var dir in Directory.GetDirectories(path))
            {
                DirectoryDeleteRecursive(dir);
            }

            // Delete files in this directory
            foreach (var file in Directory.GetFiles(path))
            {
                FileDelete(file);
            }

            return RemoveDirectoryW(extended);
        });

    private void Invoke(Action func, Func<bool> fallback)
    {
        try
        {
            func();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred when performing file system operation. Falling back to native Win32");
            try
            {
                if (!fallback())
                {
                    ThrowLastWin32Error();
                }
            }
            catch (Exception innerEx)
            {
                logger.LogError(innerEx, "Error occurred when performing Win32 file system operation.");
                throw;
            }
        }
    }

    private static string ToExtendedPath(string path)
    {
        if (path.StartsWith(@"\\?\")) return path;
        return path.StartsWith(@"\\")
            ? @"\\?\UNC\" + path[2..]
            : @"\\?\" + path;
    }

    private static void ThrowLastWin32Error()
    {
        var error = Marshal.GetLastWin32Error();
        throw new IOException($"Operation failed (Win32 error {error})",
            new Win32Exception(error));
    }
}