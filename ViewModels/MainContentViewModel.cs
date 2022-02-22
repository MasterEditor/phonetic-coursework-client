using Coursework_Client.Managers;
using Coursework_Client.XAML.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.ViewModels
{
    class MainContentViewModel : INotifyPropertyChanged
    {
        public MainContentViewModel()
        {
            PagesManager.OpenInnerPage(new NumberInfoPage());
        }

        private bool isAdmin = AccountManager.Role == "Admin" ? true : false;
        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }

        // Events

        public event EventHandler InnerPageChanged; 

        // Binding Properties

        private RelayCommand selectInnerPageCommand;
        public RelayCommand SelectInnerPageCommand
        {
            get
            {
                return selectInnerPageCommand ??
                    (selectInnerPageCommand = new RelayCommand(obj =>
                    {
                        string parameter = (string)obj;

                        InnerPageChanged?.Invoke(null, new InnerPageChangedEventArgs { PageName = parameter });
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
