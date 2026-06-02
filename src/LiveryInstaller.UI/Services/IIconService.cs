using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LiveryInstaller.UI.Services;

public interface IIconService
{
    Task<ImageSource> GetIconAsync(string iconPath);
}

public sealed class IconService : IIconService
{
    private readonly SemaphoreSlim _iconLoadSemaphore = new(Math.Max(4, Environment.ProcessorCount));

    public async Task<ImageSource> GetIconAsync(string iconPath)
    {
        await _iconLoadSemaphore.WaitAsync();

        try
        {
            return await Task.Run(() => CreateBitmapImage(iconPath));
        }
        finally
        {
            _iconLoadSemaphore.Release();
        }
    }

    private static BitmapImage CreateBitmapImage(string iconPath)
    {
        if (string.IsNullOrEmpty(iconPath))
            return null;

        var image = new BitmapImage();

        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.UriSource = new Uri(iconPath, UriKind.Absolute);
        image.DecodePixelWidth = 320;
        image.EndInit();
        image.Freeze();

        return image;
    }
}