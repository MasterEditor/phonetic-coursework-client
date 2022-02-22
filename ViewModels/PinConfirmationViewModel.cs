using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyControl.Controls;
using Coursework_Client.Managers;
using Coursework_Client.XAML.Views;


namespace Coursework_Client.ViewModels
{
    class PinConfirmationViewModel : INotifyPropertyChanged
    {
        public Func<string> GetPinValue;

        // BINDING PROPERTIES


        private RelayCommand pinCompletedCommand;
        public RelayCommand PinCompletedCommand
        {
            get
            {
                return pinCompletedCommand ??
                    (pinCompletedCommand = new RelayCommand(obj =>
                    {
                        var password = GetPinValue.Invoke();
                        if (password == "1111") PagesManager.OpenPage(new MainContentPage());
                    }));
            }
        }
        

        private RelayCommand goBackCommand;
        public RelayCommand GoBackCommand
        {
            get
            {
                return goBackCommand ??
                    (goBackCommand = new RelayCommand(obj =>
                    {
                        PagesManager.GoBack();
                    }));
            }
        }

        // INotifyPropertyChanged Realisation

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
