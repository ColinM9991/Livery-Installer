using System.IO;
using CabLib;
using LiveryInstaller.UI.Helpers;
using LiveryInstaller.UI.Models.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.Services.Liveries;

/// <inheritdoc />
public class LiveryExtractor(
    ILogger<LiveryExtractor> logger,
    IOptionsMonitor<UserSettings> userSettings)
    : ILiveryExtractor
{
    /// <inheritdoc />
    public Task<string> ExtractLiveryAsync(string liveryPath)
    {
        TaskCompletionSource<string> tcs = new();
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
            tcs.SetResult(tempPath);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to extract livery");
            tcs.SetException(e);
            throw;
        }

        return tcs.Task;
    }

    /// <summary>
    /// Creates a new extractor instance.
    /// </summary>
    /// <returns>The created extractor instance.</returns>
    private Extract CreateExtractor()
    {
        var extractor = new Extract();
        extractor.SetDecryptionKey(Paths.Value);
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