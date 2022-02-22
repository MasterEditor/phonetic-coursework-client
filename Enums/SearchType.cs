using HandyControl.Tools.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SearchType
    {
        [Description("Смешанный")]
        Mixed,
        [Description("В заголовке")]
        Header,
        [Description("В содержимом")]
        Content
    }
}
