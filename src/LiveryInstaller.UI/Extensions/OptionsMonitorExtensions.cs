using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.Extensions;

public static class OptionsMonitorExtensions
{
    public static IDisposable OnChangeWithInitial<T>(
        this IOptionsMonitor<T> monitor,
        Action<T> listener)
    {
        listener(monitor.CurrentValue);
        return monitor.OnChange(listener);
    }
}