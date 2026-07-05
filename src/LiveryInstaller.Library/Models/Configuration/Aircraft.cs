using Microsoft.Extensions.Configuration;

namespace LiveryInstaller.Library.Models.Configuration;

public class Aircraft
{
    [ConfigurationKeyName("name")]
    public string Name { get; set; }
    
    [ConfigurationKeyName("variants")]
    public ICollection<Variant> Variants { get; set; } = new List<Variant>();
}