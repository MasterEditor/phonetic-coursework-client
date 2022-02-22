using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Models
{
    public class Card : INotifyPropertyChanged
    {
        private string number;
        public string CardNumber
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("CardNumber");
            }
        }

        private int month;
        public int CardMonth
        {
            get { return month; }
            set
            {
                month = value;
                OnPropertyChanged("CardMonth");
            }
        }

        private int year;
        public int CardYear
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("CardYear");
            }
        }

        private int cvv;
        public int CardCVV
        {
            get { return cvv; }
            set
            {
                cvv = value;
                OnPropertyChanged("CardCVV");
            }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
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
    }
}
