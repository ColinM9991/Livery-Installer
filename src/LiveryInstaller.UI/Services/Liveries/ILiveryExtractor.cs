namespace LiveryInstaller.UI.Services.Liveries;

/// <summary>
/// Represents a service that can extract liveries from a given path.
/// </summary>
[LoggingDecorator]
public interface ILiveryExtractor
{
    /// <summary>
    /// Extract livery from path.
    /// </summary>
    /// <param name="liveryPath">Path to livery archive.</param>
    /// <returns>Path to extracted livery.</returns>
    Task<string> ExtractLiveryAsync(string liveryPath);
}