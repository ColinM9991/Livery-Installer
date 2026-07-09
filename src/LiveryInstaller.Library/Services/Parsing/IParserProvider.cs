namespace LiveryInstaller.Library.Services.Parsing;

public interface IParserProvider
{
    IAircraftConfigurationParser<T> Provide<T>();
}