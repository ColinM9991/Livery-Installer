using Microsoft.Extensions.Configuration;

namespace LiveryInstaller.UI.Models.Configuration;

public class Livery
{
    [ConfigurationKeyName("texture_id")]
    public string TextureId { get; set; }
    
    [ConfigurationKeyName("atc_id")]
    public string AtcId { get; set; }
    
    [ConfigurationKeyName("ui_name")]
    public string Name { get; set; }
    
    [ConfigurationKeyName("ui_description")]
    public string Description { get; set; }
    
    [ConfigurationKeyName("airline")]
    public string Airline { get; set; }
    
    [ConfigurationKeyName("sanitised_name")]
    public string SanitisedName { get; set; }
}