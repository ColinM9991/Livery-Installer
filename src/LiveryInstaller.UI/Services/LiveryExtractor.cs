using System.IO;
using CabLib;
using LiveryInstaller.UI.Models.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public class LiveryExtractor(
    ILogger<LiveryExtractor> logger,
    IOptionsMonitor<UserSettings> userSettings)
    : ILiveryExtractor
{
    /// <inheritdoc />
    public string ExtractLivery(string liveryPath)
    {
        using var extractor = CreateExtractor();
        if (!extractor.IsFileCabinet(liveryPath, out _))
        {
            logger.LogError("Livery {Livery} is not a valid file cabinet", liveryPath);
            throw new InvalidOperationException();
        }
        
        var tempDirectory = Directory.CreateTempSubdirectory();
        var tempPath = tempDirectory.FullName;

        logger.LogInformation("Extracting {Livery} to {TempPath}", liveryPath, tempDirectory);

        try
        {
            extractor.ExtractFile(liveryPath, tempPath);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to extract livery");
            throw;
        }

        return tempPath;
    }

    /// <summary>
    /// Creates a new extractor instance.
    /// </summary>
    /// <returns>The created extractor instance.</returns>
    private Extract CreateExtractor()
    {
        var extractor = new Extract();
        extractor.SetDecryptionKey(System.Text.Encoding.UTF8.GetBytes(userSettings.CurrentValue.DecryptionKey.Trim()));
        extractor.evBeforeCopyFile += info =>
        {
            logger.LogDebug("Extracting {File} from {Livery}", info.s_File, info.s_Path);

            return true;
        };
        extractor.evCabinetInfo += info =>
        {
            logger.LogDebug("Loaded cabinet file {Cabinet}", info.s_Path);
        };

        return extractor;
    }
}