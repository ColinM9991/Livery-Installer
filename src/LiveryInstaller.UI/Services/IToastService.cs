namespace LiveryInstaller.UI.Services;

/// <summary>
/// Represents a service that can raise toast notifications.
/// </summary>
public interface IToastService
{
    /// <summary>
    /// Raises an error toast notification.
    /// </summary>
    /// <param name="message">The toast notification.</param>
    void Information(string message);
    
    /// <summary>
    /// Raises an error toast notification.
    /// </summary>
    /// <param name="message">The toast notification.</param>
    void Success(string message);
    
    /// <summary>
    /// Raises an error toast notification.
    /// </summary>
    /// <param name="message">The toast notification.</param>
    void Warning(string message);
    
    /// <summary>
    /// Raises an error toast notification.
    /// </summary>
    /// <param name="message">The toast notification.</param>
    void Error(string message);
}