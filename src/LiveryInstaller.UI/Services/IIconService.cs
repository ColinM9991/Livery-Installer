using System.Windows.Media;

namespace LiveryInstaller.UI.Services;

public interface IIconService
{
    Task<ImageSource> GetIconAsync(string iconPath);
}