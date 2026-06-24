using System.Globalization;
using System.Windows.Data;

namespace LiveryInstaller.UI.Services;

public sealed class EscapedNewLineConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString()?.Replace("\\n", Environment.NewLine);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString();
    }
}