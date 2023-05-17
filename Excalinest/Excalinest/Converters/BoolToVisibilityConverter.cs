using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excalinest.Strings;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Excalinest.Converters;
public class BoolToVisibilityConverter
{
    public Visibility TrueValue { get; set; }
    public Visibility FalseValue { get; set; }

    public BoolToVisibilityConverter()
    {
        // set defaults
        TrueValue = Visibility.Visible;
        FalseValue = Visibility.Collapsed;
    }

    public object? Convert(object value, Type targetType, object parameter, string culture)
    {
        if (!(value is bool))
            return null;
        return GlobalVariables.AdminAutenticado ? TrueValue : FalseValue;
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
