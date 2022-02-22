using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Coursework_Client.Models;

namespace Coursework_Client.XAML.Converters
{
    class ServerValuesToCircleProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || parameter is null) return null;

            UserBaseInfo info = (UserBaseInfo)value;

            string par = (string)parameter;

            if (par == "Minutes")
            {
                if (info.Minutes < 0 || info.AllMinutes < 0) return 100;

                return info.Minutes / (info.AllMinutes == 0 ? 1 : info.AllMinutes) * 100;
            }

            if (par == "Internet")
            {
                if (info.Internet < 0 || info.AllInternet < 0) return 100;

                return info.Internet / (info.AllInternet == 0 ? 1 : info.AllInternet) * 100;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
