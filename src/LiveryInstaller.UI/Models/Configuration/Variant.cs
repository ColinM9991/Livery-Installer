using Microsoft.Extensions.Configuration;

namespace LiveryInstaller.UI.Models.Configuration;

public class Variant
{
    [ConfigurationKeyName("name")]
    public string Name { get; set; }
    
    [ConfigurationKeyName("liveries")]
    public ICollection<Livery> Liveries { get; set; } = new List<Livery>();
}