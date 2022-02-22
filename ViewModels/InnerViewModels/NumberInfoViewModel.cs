using Coursework_Client.Managers;
using Coursework_Client.Models;
using Coursework_Client.Models.API;
using Coursework_Client.Models.API.Receiving;
using Coursework_Client.XAML.Views;
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
    class NumberInfoViewModel : INotifyPropertyChanged
    {
        private ApiClient apiClient = new ApiClient();

        public NumberInfoViewModel()
        {
        }

        public ChartValues<int> Values { get; set; } = new ChartValues<int>();

        private ObservableCollection<Operation> lastOperations;
        public ObservableCollection<Operation> LastOperations
        {
            get { return lastOperations; }
            set
            {
                lastOperations = value;
                OnPropertyChanged("LastOperations");
            }
        }

        private UserBaseInfo userBaseInfo = new UserBaseInfo();
        public UserBaseInfo UserBaseInfo
        {
            get { return userBaseInfo; }
            set
            {
                userBaseInfo = value;
                OnPropertyChanged("UserBaseInfo");
            }
        }

        private Card card = new Card();
        public Card Card
        {
            get { return card; }
            set
            {
                card = value;
                OnPropertyChanged("Card");
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

        private bool cardInputVisibility;
        public bool CardInputVisibility
        {
            get { return cardInputVisibility; }
            set
            {
                cardInputVisibility = value;
                OnPropertyChanged("CardInputVisibility");
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

        private bool cardInputLoadingVisibility;
        public bool CardInputLoadingVisibility
        {
            get { return cardInputLoadingVisibility; }
            set
            {
                cardInputLoadingVisibility = value;
                OnPropertyChanged("CardInputLoadingVisibility");
            }
        }

        //LoadInfoCommand

        private RelayCommand loadUserInfoCommand;
        public RelayCommand LoadUserInfoCommand
        {
            get
            {
                return loadUserInfoCommand ??
                    (loadUserInfoCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetUserInfo, null, true);

                        if (result.IsSuccessStatusCode)
                        {
                            var userInfo = JsonConvert.DeserializeObject<UserInfoResponseModel>(await result.Content.ReadAsStringAsync());

                            UserBaseInfo.Number = userInfo.Number;
                            UserBaseInfo.Minutes = userInfo.Minutes;
                            UserBaseInfo.Internet = userInfo.Internet;
                            UserBaseInfo.SMS = userInfo.SMS;
                            UserBaseInfo.Balance = userInfo.Balance;
                            UserBaseInfo.AllMinutes = userInfo.AllMinutes;
                            UserBaseInfo.AllInternet = userInfo.AllInternet;
                            UserBaseInfo.AllSMS = userInfo.AllSMS;

                            OnPropertyChanged("UserBaseInfo");
                        }

                        LoadingVisibility = false;

                        UpdateOperationsCommand.Execute(null);

                        UpdateConsumptionCommand.Execute(null);
                    }));
            }
        }

        private RelayCommand openCardInputCommand;
        public RelayCommand OpenCardInputCommand
        {
            get
            {
                return openCardInputCommand ??
                    (openCardInputCommand = new RelayCommand(obj =>
                    {
                        CardInputVisibility = true;
                    }));
            }
        }

        private RelayCommand closeCardInputCommand;
        public RelayCommand CloseCardInputCommand
        {
            get
            {
                return closeCardInputCommand ??
                    (closeCardInputCommand = new RelayCommand(obj =>
                    {
                        CardInputVisibility = false;
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

        private RelayCommand rechargeCommand;
        public RelayCommand RechargeCommand
        {
            get
            {
                return rechargeCommand ??
                    (rechargeCommand = new RelayCommand(async obj =>
                    {
                        CardInputLoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.Recharge, Card, true);

                        if (result.IsSuccessStatusCode)
                        {
                            CardInputLoadingVisibility = false;

                            CardInputVisibility = false;

                            LoadUserInfoCommand.Execute(null);
                        }
                        else if (result.StatusCode == HttpStatusCode.BadRequest)
                        {
                            ErrorResponseModel err_model = JsonConvert.DeserializeObject<ErrorResponseModel>(await result.Content.ReadAsStringAsync());
                            ShowError(err_model.Error);
                        }
                        else
                        {
                            ShowError("Непредвиденная ошибка");
                        }

                        CardInputLoadingVisibility = false;

                        CardInputVisibility = false;
                    }));
            }
        }

        private RelayCommand updateOperationsCommand;
        public RelayCommand UpdateOperationsCommand
        {
            get
            {
                return updateOperationsCommand ??
                    (updateOperationsCommand = new RelayCommand(async obj =>
                    {
                        var result = await apiClient.SendPostAsync(ApiStrings.GetLastOperations, null, true);

                        if (result.IsSuccessStatusCode)
                        {
                            List<Operation> operations = JsonConvert.DeserializeObject<List<Operation>>(await result.Content.ReadAsStringAsync());

                            LastOperations = new ObservableCollection<Operation>(operations.OrderByDescending(o => o.Date).ToList());

                        }
                    }));
            }
        }

        private RelayCommand updateConsumptionCommand;
        public RelayCommand UpdateConsumptionCommand
        {
            get
            {
                return updateConsumptionCommand ??
                    (updateConsumptionCommand = new RelayCommand(async obj =>
                    {
                        var result = await apiClient.SendPostAsync(ApiStrings.GetLastConsumptions, null, true);

                        if (result.IsSuccessStatusCode)
                        {
                            List<Consumption> consumptions = JsonConvert.DeserializeObject<List<Consumption>>(await result.Content.ReadAsStringAsync());

                            Values.Clear();

                            Values.AddRange(consumptions.Select(c => c.Value));
                        }
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
