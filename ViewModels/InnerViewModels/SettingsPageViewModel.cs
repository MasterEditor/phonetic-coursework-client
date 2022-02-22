using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coursework_Client.Models;
using Coursework_Client.Models.API;
using Coursework_Client.Models.API.Receiving;
using Newtonsoft.Json;
using Coursework_Client.Managers;
using Coursework_Client.XAML.Views;
using HandyControl.Controls;

namespace Coursework_Client.ViewModels
{
    class SettingsPageViewModel : INotifyPropertyChanged
    {
        private ApiClient apiClient = new ApiClient();

        private string currentPassword;
        public string CurrentPassword
        {
            get { return currentPassword; }
            set
            {
                currentPassword = value;
                OnPropertyChanged("CurrentPassword");
            }
        }

        private string newPassword;
        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }

        private string newPasswordRepeat;
        public string NewPasswordRepeat
        {
            get { return newPasswordRepeat; }
            set
            {
                newPasswordRepeat = value;
                OnPropertyChanged("NewPasswordRepeat");
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

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
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

        private RelayCommand changePasswordCommand;
        public RelayCommand ChangePasswordCommand
        {
            get
            {
                return changePasswordCommand ??
                    (changePasswordCommand = new RelayCommand(async obj =>
                    {
                        if (!ValidatePasswords()) return;

                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.ChangePassword, new { CurrentPassword = CurrentPassword, PasswordToSet = NewPassword }, true);

                        LoadingVisibility = false;

                        if (!result.IsSuccessStatusCode)
                        {
                            ErrorResponseModel error = JsonConvert.DeserializeObject<ErrorResponseModel>(await result.Content.ReadAsStringAsync());

                            ErrorMessage = error.Error;

                            ErrorVisibility = true;


                        }
                        else { Growl.SuccessGlobal("Пароль успешно изменен"); }

                        
                    }));
            }
        }

        private RelayCommand sendFeedbackCommand;
        public RelayCommand SendFeedbackCommand
        {
            get
            {
                return sendFeedbackCommand ??
                    (sendFeedbackCommand = new RelayCommand(async obj =>
                    {
                        if (string.IsNullOrWhiteSpace(message)) return;

                        var result = await apiClient.SendPostAsync(ApiStrings.SendFeedback, new { Message = message }, true);

                        if (result.IsSuccessStatusCode)
                        {
                            Growl.SuccessGlobal("Сообщение успешно отправлено");
                        }
                        else
                        {
                            Growl.ErrorGlobal("При отправке сообщения произошла ошибка");
                        }
                    }));
            }
        }


        private RelayCommand logoutCommand;
        public RelayCommand LogoutCommand
        {
            get
            {
                return logoutCommand ??
                    (logoutCommand = new RelayCommand(obj =>
                    {
                        PagesManager.OpenPage(new PhoneInputPage());
                    }));
            }
        }


        private bool ValidatePasswords()
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                ShowError("Текущий пароль не может быть пустым");
                return false;
            }
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                ShowError("Новый пароль не может быть пустым");
                return false;
            }
            if (string.IsNullOrWhiteSpace(NewPasswordRepeat))
            {
                ShowError("Новый пароль не может быть пустым");
                return false;
            }
            if(NewPassword != NewPasswordRepeat)
            {
                ShowError("Пароли не совпадают");
                return false;
            }
            if(NewPassword.Length < 6)
            {
                ShowError("Новый пароль слишком слабый");
                return false;
            }

            return true;
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
