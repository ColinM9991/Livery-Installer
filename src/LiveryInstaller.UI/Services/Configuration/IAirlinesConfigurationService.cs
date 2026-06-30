namespace LiveryInstaller.UI.Services.Configuration;

[LoggingDecorator]
public interface IAirlinesConfigurationService
{
    string GetAirlineName(string uiVariation);
}