using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LiveryInstaller.UI.Converters;

public class CircleCircumferenceMultiValueConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 2 ||
            values[0] is not double radius ||
            values[1] is not double thickness ||
            thickness <= 0)
        {
            return Binding.DoNothing;
        }
        
        var effectiveRadius  = Math.Max(radius, thickness);

        var circumference = Math.PI * (effectiveRadius - thickness);

        var lineLength = circumference * 0.75;
        var gapLength = circumference - lineLength;

        var collection = new DoubleCollection([lineLength / thickness, gapLength / thickness]);
        collection.Freeze();
        
        return collection;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}