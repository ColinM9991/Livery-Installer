using System.Windows.Media;

namespace LiveryInstaller.UI.Services.Icons;

public interface IIconService
{
    Task<ImageSource> GetIconAsync(string iconPath);
}