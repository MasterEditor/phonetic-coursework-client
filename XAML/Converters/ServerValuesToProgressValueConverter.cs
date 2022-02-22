using Coursework_Client.Models;
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
    class ServerValuesToProgressValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || parameter is null) return null;

            UserBaseInfo info = (UserBaseInfo)value;

            string par = (string)parameter;

            if(par == "Minutes")
            {
                if (info.Minutes < 0 || info.AllMinutes < 0) return "∞";

                return $"{info.Minutes}/{info.AllMinutes}";
            }

            if (par == "Internet")
            {
                if (info.Internet  < 0 || info.AllInternet < 0) return "∞";

                return $"{info.Internet}/{info.AllInternet}";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
