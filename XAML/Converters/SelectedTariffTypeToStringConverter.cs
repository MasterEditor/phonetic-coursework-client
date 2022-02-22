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
    class SelectedTariffTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TariffType type = (TariffType)value;

            string converted;

            switch (type)
            {
                case TariffType.ForSmartphone:
                    converted = "Данный тариф отлично подходит для смартфона";
                    break;
                case TariffType.ForInternet:
                    converted = "Данный тариф отлично подходит для серфинга в интернете";
                    break;
                case TariffType.ForCalls:
                    converted = "Данный тариф отлично подходит для звонков";
                    break;
                case TariffType.Special:
                    converted = "Данный тариф является специальный тарифом";
                    break;
                default:
                    converted = "О данном тарифе нет информации";
                    break;
            }

            return converted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

//анный тариф отлично подходит для серфинга в интернете