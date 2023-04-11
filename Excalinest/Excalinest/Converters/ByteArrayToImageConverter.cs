using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Excalinest.Converters;
public class ByteArrayToImageConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string culture)
    {
        if (value is not byte[] imageData) return null;

        BitmapImage bitmapImage = new BitmapImage();
        using (MemoryStream memoryStream = new MemoryStream(imageData))
        {
            bitmapImage.SetSource(memoryStream.AsRandomAccessStream());
        }

        return bitmapImage;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string culture)
    {
        throw new NotImplementedException();
    }
}
