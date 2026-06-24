using System.Text.Json.Serialization;
using LiveryInstaller.UI.Models.DTO;
using Microsoft.Extensions.Configuration;

namespace LiveryInstaller.UI.Models.Configuration;

public class Livery
{
    public Livery()
    {
    }

    public Livery(LiveryDto liveryDto)
    {
        TextureId = liveryDto.TextureId;
        AtcId = liveryDto.AtcId;
        Name = liveryDto.Name;
        Description = liveryDto.Description;
        Airline = liveryDto.Airline;
        SanitisedName = liveryDto.SanitisedName;
        IsUserImported = true;
    }
    
    [JsonPropertyName("texture_id")]
    [ConfigurationKeyName("texture_id")]
    public string TextureId { get; set; }
    
    [JsonPropertyName("atc_id")]
    [ConfigurationKeyName("atc_id")]
    public string AtcId { get; set; }
    
    [JsonPropertyName("ui_name")]
    [ConfigurationKeyName("ui_name")]
    public string Name { get; set; }
    
    [JsonPropertyName("ui_description")]
    [ConfigurationKeyName("ui_description")]
    public string Description { get; set; }
    
    [JsonPropertyName("airline")]
    [ConfigurationKeyName("airline")]
    public string Airline { get; set; }
    
    [JsonPropertyName("sanitised_name")]
    [ConfigurationKeyName("sanitised_name")]
    public string SanitisedName { get; set; }
    
    [JsonPropertyName("is_user_imported")]
    [ConfigurationKeyName("is_user_imported")]
    public bool IsUserImported { get; set; }
}