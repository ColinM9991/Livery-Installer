using System.Text.Json.Serialization;

namespace LiveryInstaller.Library.Models.Configuration;

public class UserConfiguration
{
    public UserConfiguration()
    {
    }

    public UserConfiguration(UserSettings settings)
    {
        Settings = settings;
    }
    
    [JsonPropertyName("userSettings")]
    public UserSettings Settings { get; set; }
}