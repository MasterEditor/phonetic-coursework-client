using Coursework_Client.Managers;
using Coursework_Client.Models;
using Coursework_Client.Models.API;
using Coursework_Client.Models.API.Receiving;
using Coursework_Client.Models.API.Sending;
using Coursework_Client.XAML.Views;
using HandyControl.Data;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.ViewModels
{
    class UsersAdminViewModel : INotifyPropertyChanged
    {

        private ApiClient apiClient = new ApiClient();

        public UsersAdminViewModel()
        {
            LoadUsersCommand.Execute(null);
        }

        private ObservableCollection<UserBaseInfo> users = new ObservableCollection<UserBaseInfo>();
        public ObservableCollection<UserBaseInfo> Users
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }

        private bool loadingVisibility;
        public bool LoadingVisibility
        {
            get { return loadingVisibility; }
            set
            {
                loadingVisibility = value;
                OnPropertyChanged("LoadingVisibility");
            }
        }

        private bool errorVisibility;
        public bool ErrorVisibility
        {
            get { return errorVisibility; }
            set
            {
                errorVisibility = value;
                OnPropertyChanged("ErrorVisibility");
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private RelayCommand closeErrorDialogCommand;
        public RelayCommand CloseErrorDialogCommand
        {
            get
            {
                return closeErrorDialogCommand ??
                    (closeErrorDialogCommand = new RelayCommand(obj =>
                    {
                        ErrorVisibility = false;
                        ErrorMessage = null;
                    }));
            }
        }

        private RelayCommand openErrorDialogCommand;
        public RelayCommand OpenErrorDialogCommand
        {
            get
            {
                return openErrorDialogCommand ??
                    (openErrorDialogCommand = new RelayCommand(obj =>
                    {
                        ErrorVisibility = true;
                    }));
            }
        }

        private RelayCommand loadUsersCommand;
        public RelayCommand LoadUsersCommand
        {
            get
            {
                return loadUsersCommand ??
                    (loadUsersCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetAllUsers, null, true);

                        if (result.IsSuccessStatusCode)
                        {
                            List<UserBaseInfo> users = JsonConvert.DeserializeObject<List<UserBaseInfo>>(await result.Content.ReadAsStringAsync());

                            Users = new ObservableCollection<UserBaseInfo>(users);


                        }

                        LoadingVisibility = false;
                    }));
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            OpenErrorDialogCommand.Execute(null);
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
