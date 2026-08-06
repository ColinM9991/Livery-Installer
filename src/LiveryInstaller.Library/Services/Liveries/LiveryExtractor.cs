using CabLib;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.Library.Services.Liveries;

/// <inheritdoc />
public class LiveryExtractor(
    ILogger<LiveryExtractor> logger)
    : ILiveryExtractor
{
    public static readonly byte[] Value = Convert.FromBase64String("UE1ER19TZWN1cml0eUNvZGU=");
    /// <inheritdoc />
    public async Task<string> ExtractLiveryAsync(string liveryPath)
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
            await Task.Run(() => extractor.ExtractFile(liveryPath, tempPath));
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
        extractor.SetDecryptionKey(Value);
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