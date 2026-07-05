namespace LiveryInstaller.Library.Services.Configuration;

[LoggingDecorator]
public interface IAirlinesConfigurationService
{
    string GetAirlineName(string uiVariation);
}