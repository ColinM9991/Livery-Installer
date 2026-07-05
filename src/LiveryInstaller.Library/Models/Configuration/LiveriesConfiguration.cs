using System.Text.Json.Serialization;

namespace LiveryInstaller.Library.Models.Configuration;

public class LiveriesConfiguration
{
    [JsonPropertyName("userLiveriesConfiguration")]
    public AircraftConfiguration AircraftConfiguration { get; set; } = new AircraftConfiguration();
}