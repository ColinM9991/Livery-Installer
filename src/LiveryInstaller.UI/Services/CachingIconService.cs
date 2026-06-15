using System.Windows.Media;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public class CachingIconService(IIconService iconService) : IIconService
{
    private static readonly Dictionary<string, ImageSource> IconsCache = new();
    
    /// <inheritdoc />
    public async Task<ImageSource> GetIconAsync(string iconPath)
    {
        if (IconsCache.TryGetValue(iconPath, out var cachedIcon))
            return cachedIcon;

        var icon = await iconService.GetIconAsync(iconPath);
        IconsCache[iconPath] = icon;
        return icon;
    }
}