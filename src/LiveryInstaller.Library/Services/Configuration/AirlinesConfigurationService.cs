using LiveryInstaller.Library.Models.Configuration;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.Library.Services.Configuration;

public sealed class AirlinesConfigurationService(IOptions<AirlinesConfiguration> airlinesConfiguration)
    : IAirlinesConfigurationService
{
    public string GetAirlineName(string uiVariation) => airlinesConfiguration.Value.Airlines.Where(uiVariation.Contains)
        .OrderByDescending(airline => airline.Length)
        .FirstOrDefault();
}