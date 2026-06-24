using System.Windows.Media;

namespace LiveryInstaller.UI.Services.Icons;

/// <inheritdoc />
public class CachingIconService(IIconService iconService) : IIconService
{
    private const string FallbackImageName = "FallbackIcon";
    private static readonly Dictionary<string, ImageSource> IconsCache = new();
    
    /// <inheritdoc />
    public async Task<ImageSource> GetIconAsync(string iconPath)
    {
        var cachedIconPath = iconPath ?? FallbackImageName;
        
        if (IconsCache.TryGetValue(cachedIconPath, out var cachedIcon))
            return cachedIcon;

        var icon = await iconService.GetIconAsync(iconPath);
        IconsCache[cachedIconPath] = icon;
        return icon;
    }
}