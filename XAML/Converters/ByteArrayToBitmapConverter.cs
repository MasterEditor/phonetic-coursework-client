using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Coursework_Client.XAML.Converters
{
    public class ByteArrayToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] byteArr = (byte[])value;
            BitmapImage image = new BitmapImage();

            var stream = new MemoryStream(byteArr);
            stream.Seek(0, SeekOrigin.Begin);

            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
