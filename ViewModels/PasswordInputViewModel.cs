using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyControl.Controls;
using Coursework_Client.Managers;
using Coursework_Client.XAML.Views;
using Coursework_Client.Models.API.Sending;
using Coursework_Client.Models.API;
using Coursework_Client.Models.API.Receiving;
using Newtonsoft.Json;

namespace Coursework_Client.ViewModels
{
    class PasswordInputViewModel : INotifyPropertyChanged
    {
        public PasswordInputViewModel()
        {
            Number = (string)TempStorageManager.Data["PhoneNumber"];
        }

        private ApiClient apiClient = new ApiClient();

        public string Number { get; set; }


        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
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

        private RelayCommand recoverPasswordCommand;
        public RelayCommand RecoverPasswordCommand
        {
            get
            {
                return recoverPasswordCommand ??
                    (recoverPasswordCommand = new RelayCommand(obj =>
                    {
                        PagesManager.OpenPage(new PasswordRecoveryPage());
                    }));
            }
        }

        private RelayCommand loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                    (loginCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        LoginRequestModel log_model = new LoginRequestModel();

                        log_model.Number = Number;
                        log_model.Password = Password;

                        var result = await apiClient.SendPostAsync(ApiStrings.Login, log_model);

                        if (result.IsSuccessStatusCode)
                        {
                            LoginResponseModel resp_model = JsonConvert.DeserializeObject<LoginResponseModel>(await result.Content.ReadAsStringAsync());

                            AccountManager.Token = resp_model.Token;

                            AccountManager.Role = resp_model.Role;

                            PagesManager.OpenPage(new MainContentPage());

                            Growl.SuccessGlobal("Вход выполнен успешно");
                        }
                        else
                        {
                            ErrorResponseModel err_model = JsonConvert.DeserializeObject<ErrorResponseModel>(await result.Content.ReadAsStringAsync());
                            ShowError(err_model.Error);
                        }

                        LoadingVisibility = false;

                    }, obj =>
                    {
                        if (string.IsNullOrEmpty(Password) || Password.Length < 3) return false;

                        return true;
                    }));
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

        private void ShowError(string message)
        {
            ErrorMessage = message;
            OpenErrorDialogCommand.Execute(null);
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
