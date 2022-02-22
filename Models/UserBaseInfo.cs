using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Models
{
    public class UserBaseInfo : INotifyPropertyChanged
    {
        private string number;
        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        private double balance;
        public double Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                OnPropertyChanged("Balance");
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

        private int allMinutes;
        public int AllMinutes
        {
            get { return allMinutes; }
            set
            {
                allMinutes = value;
                OnPropertyChanged("AllMinutes");
            }
        }

        private int allInternet;
        public int AllInternet
        {
            get { return allInternet; }
            set
            {
                allInternet = value;
                OnPropertyChanged("AllInternet");
            }
        }

        private int allSMS;
        public int AllSMS
        {
            get { return allSMS; }
            set
            {
                allSMS = value;
                OnPropertyChanged("UserBaseInfo");
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
