using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace LiveryInstaller.UI.Models.Configuration;

public class AircraftConfiguration
{
    [JsonPropertyName("aircraft")]
    [ConfigurationKeyName("aircraft")]
    public ICollection<Aircraft> Aircraft { get; set; } = new List<Aircraft>();
}