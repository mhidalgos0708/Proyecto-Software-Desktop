using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excalinest.Strings;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Excalinest.Converters;
public class BoolToVisibilityConverter: IValueConverter
{
    public Visibility TrueValue { get; set; }
    public Visibility FalseValue { get; set; }

    public BoolToVisibilityConverter()
    {
        // set defaults
        TrueValue = Visibility.Visible;
        FalseValue = Visibility.Collapsed;
    }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool && (bool)value)
            return Visibility.Visible;

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
