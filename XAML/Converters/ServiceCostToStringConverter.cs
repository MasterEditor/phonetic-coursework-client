using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Coursework_Client.XAML.Converters
{
    class ServiceCostToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            double price = (double)value[0];
            string duration = (string)value[1];

            if (price == 0) return "FREE";

            switch (duration)
            {
                case "Infinite": return $"{price} руб";
                case "Month": return $"{price} руб/мес";
                case "Day": return $"{price} руб/день";
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
