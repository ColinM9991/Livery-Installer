using System.IO;

namespace LiveryInstaller.UI.Helpers;

public static class Paths
{
    public static string SettingsDirectory =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LiveryInstaller");
}