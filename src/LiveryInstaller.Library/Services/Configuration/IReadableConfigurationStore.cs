namespace LiveryInstaller.Library.Services.Configuration;

public interface IReadableConfigurationStore<T>
{
    Task<T> ReadAsync();
}