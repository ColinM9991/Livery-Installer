namespace LiveryInstaller.UI.Services;

/// <summary>
/// Represents a service that can manage aircraft configurations.
/// </summary>
public interface IAircraftConfigurationService
{
    /// <summary>
    /// Adds livery to variant configuration
    /// </summary>
    /// <param name="variantName">The name of the variant to which the livery will be added.</param>
    /// <param name="archiveDirectory">The directory path of the livery archive.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task AddLiveryAsync(string variantName, string archiveDirectory);

    /// <summary>
    /// Removes livery from variant configuration
    /// </summary>
    /// <param name="variantName">The name of the variant from which to remove the livery.</param>
    /// <param name="atcId">The ATC ID of the livery to remove.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task RemoveLiveryAsync(string variantName, string atcId);
}