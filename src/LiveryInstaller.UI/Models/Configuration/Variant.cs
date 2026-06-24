using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace LiveryInstaller.UI.Models.Configuration;

public class Variant
{
    [JsonPropertyName("name")]
    [ConfigurationKeyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("liveries")]
    [ConfigurationKeyName("liveries")]
    public ICollection<Livery> Liveries { get; set; } = new List<Livery>();
}