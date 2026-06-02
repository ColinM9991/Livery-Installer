namespace LiveryInstaller.UI.Services;

/// <summary>
/// Represents a service that can extract liveries from a given path.
/// </summary>
public interface ILiveryExtractor
{
    /// <summary>
    /// Extract livery from path.
    /// </summary>
    /// <param name="liveryPath">Path to livery archive.</param>
    /// <returns>Path to extracted livery.</returns>
    string ExtractLivery(string liveryPath);
}