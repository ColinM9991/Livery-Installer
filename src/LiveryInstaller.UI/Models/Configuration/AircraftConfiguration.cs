using Microsoft.Extensions.Configuration;

namespace LiveryInstaller.UI.Models.Configuration;

public class AircraftConfiguration
{
    [ConfigurationKeyName("aircraft")]
    public ICollection<Aircraft> Aircraft { get; set; } = new List<Aircraft>();
}