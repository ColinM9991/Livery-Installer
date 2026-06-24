namespace LiveryInstaller.UI.Services.Configuration;

public interface IReadableConfigurationStore<T>
{
    Task<T> ReadAsync();
}