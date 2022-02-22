using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Models
{
    public class Tariff : ICloneable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Minutes { get; set; }

        public int Internet { get; set; }
        
        public int SMS { get; set; }

        public double Price { get; set; }

        public TariffType Type { get; set; }

        public object Clone()
        {
            return new Tariff { Id = Id, Name = Name is null ? "" : string.Copy(Name), Minutes = Minutes, Internet = Internet, SMS = SMS, Price = Price, Type = Type };
        }
    }

    public enum TariffType
    {
        ForSmartphone,
        ForInternet,
        ForCalls,
        Special
    }
}
