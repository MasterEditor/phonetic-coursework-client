using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Models
{
    public class Service : INotifyPropertyChanged, ICloneable
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string header;
        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                OnPropertyChanged("Header");
            }
        }

        private string details;
        public string Details
        {
            get { return details; }
            set
            {
                details = value;
                OnPropertyChanged("Details");
            }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        private string duration = "Infinite";
        public string Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged("Duration");
            }
        }

        private int minutes;
        public int Minutes
        {
            get { return minutes; }
            set
            {
                minutes = value;
                OnPropertyChanged("Minutes");
            }
        }

        private int internet;
        public int Internet
        {
            get { return internet; }
            set
            {
                internet = value;
                OnPropertyChanged("Internet");
            }
        }

        private int sms;
        public int SMS
        {
            get { return sms; }
            set
            {
                sms = value;
                OnPropertyChanged("SMS");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public object Clone()
        {
            return new Service()
            {
                Id = Id,
                Name = Name is null ? "" : string.Copy(Name),
                Header = Header is null ? "" :string.Copy(Header),
                Price = Price,
                Duration = Duration is null ? "" : string.Copy(Duration),
                Minutes = Minutes,
                Internet = Internet,
                SMS = SMS,
                Details = Details is null ? "" : string.Copy(Details)
            };
        }
    }
}
