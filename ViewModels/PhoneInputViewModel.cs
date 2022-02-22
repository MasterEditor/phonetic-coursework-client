using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyControl.Controls;
using Coursework_Client.Managers;
using Coursework_Client.XAML.Views;
using Coursework_Client.Models.API;

namespace Coursework_Client.ViewModels
{
    class PhoneInputViewModel : INotifyPropertyChanged
    {
        private UsersApiClient apiClient = new UsersApiClient();

        private bool registerDialogVisibility;
        public bool RegisterDialogVisibility
        {
            get { return registerDialogVisibility; }
            set
            {
                registerDialogVisibility = value;
                OnPropertyChanged("RegisterDialogVisibility");
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


        private async Task ProcessPhoneInput(string phoneNumber)
        {
            LoadingVisibility = true;

            phoneNumber = phoneNumber.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "");

            var isRegistered = await apiClient.IsRegistered(phoneNumber);

            LoadingVisibility = false;

            if (isRegistered)
            {
                TempStorageManager.Data["PhoneNumber"] = phoneNumber;
                PagesManager.OpenPage(new PasswordInputPage());
            }
            else
            {
                RegisterDialogVisibility = true;
            }

        }

        public async void OnMaskCompleted(object sender, EventArgs e)
        {
            var args = (MaskedPhoneNumberEventArgs)e;
            await ProcessPhoneInput(args.Number);
        }

        private RelayCommand retryCommand;
        public RelayCommand RetryCommand
        {
            get
            {
                return retryCommand ??
                    (retryCommand = new RelayCommand(obj =>
                    {
                        RegisterDialogVisibility = false;
                    }));
            }
        }
        
        private RelayCommand registerCommand;
        public RelayCommand RegisterCommand
        {
            get
            {
                return registerCommand ??
                    (registerCommand = new RelayCommand(obj =>
                    {
                        RegisterDialogVisibility = false;
                        PagesManager.OpenPage(new RegisterPage());
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
