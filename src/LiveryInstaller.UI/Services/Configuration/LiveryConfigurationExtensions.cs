using LiveryInstaller.UI.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveryInstaller.UI.Services.Configuration;

public static class LiveryConfigurationExtensions
{
    public static IServiceCollection ConfigureLiveryConfiguration(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<AircraftConfiguration>(opts =>
        {
            var liveryConfiguration = configuration.GetSection("liveriesConfiguration").Get<AircraftConfiguration>() ?? new AircraftConfiguration();
            var userLiveryConfiguration = configuration.GetSection("userLiveriesConfiguration").Get<AircraftConfiguration>() ?? new AircraftConfiguration();

            foreach (var aircraft in userLiveryConfiguration.Aircraft)
            {
                var liveryConfigurationAircraft = liveryConfiguration.Aircraft.FirstOrDefault(x => x.Name == aircraft.Name);
                if (liveryConfigurationAircraft == null)
                {
                    liveryConfiguration.Aircraft.Add(aircraft);
                    liveryConfigurationAircraft = aircraft;
                }
                
                foreach (var variant in aircraft.Variants)
                {
                    var liveryAircraftVariant = liveryConfigurationAircraft.Variants.FirstOrDefault(x => x.Name == variant.Name);
                    if (liveryAircraftVariant == null)
                    {
                        liveryConfigurationAircraft.Variants.Add(variant);
                        liveryAircraftVariant = variant;
                    }
                    
                    foreach (var livery in variant.Liveries)
                    {
                        var liveryAircraftVariantLivery = liveryAircraftVariant.Liveries.FirstOrDefault(x => x.TextureId == livery.TextureId);
                        if (liveryAircraftVariantLivery == null)
                        {
                            liveryAircraftVariant.Liveries.Add(livery);
                        }
                    }
                }
            }
            
            opts.Aircraft = liveryConfiguration.Aircraft;
        });

        return services;
    }
}