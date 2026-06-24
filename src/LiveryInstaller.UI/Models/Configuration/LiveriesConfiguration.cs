using System.Text.Json.Serialization;

namespace LiveryInstaller.UI.Models.Configuration;

public class LiveriesConfiguration
{
    [JsonPropertyName("userLiveriesConfiguration")]
    public AircraftConfiguration AircraftConfiguration { get; set; } = new AircraftConfiguration();
}