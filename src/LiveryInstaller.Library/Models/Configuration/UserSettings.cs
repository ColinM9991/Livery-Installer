using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.Library.Models.Configuration;

public class UserSettings
{
    [JsonPropertyName("logLevel")]
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
    
    [JsonPropertyName("liveriesPath")]
    public required string LiveriesPath { get; set; }
    
    [JsonPropertyName("decryptionKey")]
    public required string DecryptionKey { get; set; }
}