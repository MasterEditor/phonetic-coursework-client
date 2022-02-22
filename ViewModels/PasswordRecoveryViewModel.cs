using Coursework_Client.Managers;
using Coursework_Client.Models.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coursework_Client.Models.API.Sending;
using Coursework_Client.Models.API.Receiving;
using Coursework_Client.XAML.Views;
using Newtonsoft.Json;
using HandyControl.Controls;

namespace Coursework_Client.ViewModels
{
    class PasswordRecoveryViewModel : INotifyPropertyChanged
    {
        public PasswordRecoveryViewModel()
        {
            Number = (string)TempStorageManager.Data["PhoneNumber"];
        }

        private ApiClient apiClient = new ApiClient();
        private string SmsId;

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

        private string repeatPassword;
        public string RepeatPassword
        {
            get { return repeatPassword; }
            set
            {
                repeatPassword = value;
                OnPropertyChanged("RepeatPassword");
            }
        }

        private string confirmationCode;
        public string ConfirmationCode
        {
            get { return confirmationCode; }
            set
            {
                confirmationCode = value;
                OnPropertyChanged("ConfirmationCode");
            }
        }

        private bool confirmNumber;
        public bool ConfirmNumber
        {
            get { return confirmNumber; }
            set
            {
                confirmNumber = value;
                OnPropertyChanged("ConfirmNumber");
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

        private RelayCommand sendSMSCommand;
        public RelayCommand SendSMSCommand
        {
            get
            {
                return sendSMSCommand ??
                    (sendSMSCommand = new RelayCommand(async obj =>
                    {
                        if (!Validate()) return;

                        LoadingVisibility = true;

                        RecoverPasswordRequestModel reg_model = new RecoverPasswordRequestModel();

                        reg_model.Number = Number;

                        var result = await apiClient.SendPostAsync(ApiStrings.RecoverPassword, reg_model);

                        if (result.IsSuccessStatusCode)
                        {
                            RegisterResponseModel resp_model = JsonConvert.DeserializeObject<RegisterResponseModel>(await result.Content.ReadAsStringAsync());

                            SmsId = resp_model.Id;
                            ConfirmNumber = true;
                        }
                        else
                        {
                            ErrorResponseModel err_model = JsonConvert.DeserializeObject<ErrorResponseModel>(await result.Content.ReadAsStringAsync());
                            ShowError(err_model.Error);
                        }

                        LoadingVisibility = false;

                    }, obj =>
                    {
                        if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RepeatPassword)
                        || RepeatPassword.Length == 1 || Password.Length == 1) return false;
                        return true;
                    }));
            }
        }

        private RelayCommand confirmChangingCommand;
        public RelayCommand ConfirmChangingCommand
        {
            get
            {
                return confirmChangingCommand ??
                    (confirmChangingCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        ConfirmPasswordRecoveryRequestModel reg_model = new ConfirmPasswordRecoveryRequestModel();

                        reg_model.Id = SmsId;
                        reg_model.Code = ConfirmationCode;
                        reg_model.Password = Password;

                        var result = await apiClient.SendPostAsync(ApiStrings.ConfirmPasswordRecovery, reg_model);

                        if (result.IsSuccessStatusCode)
                        {
                            ConfirmRegistrationResponseModel resp_model = JsonConvert.DeserializeObject<ConfirmRegistrationResponseModel>(await result.Content.ReadAsStringAsync());

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
                        if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RepeatPassword)
                        || RepeatPassword.Length == 1 || Password.Length == 1 || string.IsNullOrEmpty(ConfirmationCode)) return false;
                        return true;
                    }));
            }
        }

        private RelayCommand resetCommand;
        public RelayCommand ResetCommand
        {
            get
            {
                return resetCommand ??
                    (resetCommand = new RelayCommand(obj =>
                    {
                        Password = string.Empty;
                        RepeatPassword = string.Empty;
                        ConfirmationCode = string.Empty;
                        ConfirmNumber = false;
                        SmsId = string.Empty;
                    }));
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            OpenErrorDialogCommand.Execute(null);
        }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(Number)) { ShowError("Номер не может быть пустым"); return false; }
            if (string.IsNullOrEmpty(Password)) { ShowError("Пароль не может быть пустым"); return false; }
            if (string.IsNullOrEmpty(RepeatPassword)) { ShowError("Пароль не может быть пустым"); return false; }

            if (Number.Length < 7) { ShowError("Номер слишком короткий"); return false; }
            if (Password.Length < 7) { ShowError("Пароль слишком короткий"); return false; }
            if (RepeatPassword.Length < 7) { ShowError("Пароль слишком короткий"); return false; }

            if (Password != RepeatPassword) { ShowError("Пароли должны совпадать"); return false; }

            return true;
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
