using System.IO;

namespace LiveryInstaller.UI.Helpers;

public static class Paths
{
    public static string SettingsDirectory =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LiveryInstaller");

    public static readonly byte[] Value = Convert.FromBase64String("UE1ER19TZWN1cml0eUNvZGU=");
}