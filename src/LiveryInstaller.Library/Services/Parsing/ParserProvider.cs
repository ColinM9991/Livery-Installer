using Microsoft.Extensions.DependencyInjection;

namespace LiveryInstaller.Library.Services.Parsing;

public sealed class ParserProvider(IServiceProvider serviceProvider) : IParserProvider
{
    public IAircraftConfigurationParser<T> Provide<T>() =>
        serviceProvider.GetRequiredService<IAircraftConfigurationParser<T>>();
}