using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Coursework_Client.XAML.Converters
{
    public class ServerIntToAppTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int)
            {
                var d_num = (int)value;

                if (d_num >= 0) return d_num;

                return "∞";
            }

            var num = (double)value;

            if (num != -1) return num;

            return "∞";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
