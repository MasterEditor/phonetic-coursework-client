using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coursework_Client.Models;

namespace Coursework_Client
{
    class OnAddNewTariffEventArgs : EventArgs
    {
        public Tariff Tariff { get; set; }

        public TariffType Type { get; set; }
    }
}
