using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Coursework_Client.Models;
using Coursework_Client.Models.API;
using Coursework_Client.Models.API.Receiving;
using HandyControl.Controls;
using HandyControl.Data;
using Newtonsoft.Json;

namespace Coursework_Client.ViewModels
{
    class TariffsPageViewModel : INotifyPropertyChanged
    {
        private ApiClient apiClient = new ApiClient();

        public TariffsPageViewModel()
        {

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

        private bool infoVisibility;
        public bool InfoVisibility
        {
            get { return infoVisibility; }
            set
            {
                infoVisibility = value;
                OnPropertyChanged("InfoVisibility");
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

        private bool changeTariffDialogVisibility;
        public bool ChangeTariffDialogVisibility
        {
            get { return changeTariffDialogVisibility; }
            set
            {
                changeTariffDialogVisibility = value;
                OnPropertyChanged("ChangeTariffDialogVisibility");
            }
        }

        private Tariff myTariff;
        public Tariff MyTariff
        {
            get { return myTariff; }
            set
            {
                myTariff = value;
                OnPropertyChanged("MyTariff");
            }
        }

        private Tariff selectedTariff;
        public Tariff SelectedTariff
        {
            get { return selectedTariff; }
            set
            {
                selectedTariff = value;
                OnPropertyChanged("SelectedTariff");
            }
        }

        private bool myTariffLoadingVisibility;
        public bool MyTariffLoadingVisibility
        {
            get { return myTariffLoadingVisibility; }
            set
            {
                myTariffLoadingVisibility = value;
                OnPropertyChanged("MyTariffLoadingVisibility");
            }
        }

        private RelayCommand updateMyTariffCommand;
        public RelayCommand UpdateMyTariffCommand
        {
            get
            {
                return updateMyTariffCommand ??
                    (updateMyTariffCommand = new RelayCommand(async obj =>
                    {
                        MyTariffLoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetMyTariff, null, true);

                        if (result.IsSuccessStatusCode)
                        {
                            MyTariff = JsonConvert.DeserializeObject<Tariff>(await result.Content.ReadAsStringAsync());
                        }

                        MyTariffLoadingVisibility = false;
                    }));
            }
        }

        private RelayCommand loadTariffsCommand;
        public RelayCommand LoadTariffsCommand
        {
            get
            {
                return loadTariffsCommand ??
                    (loadTariffsCommand = new RelayCommand(async obj =>
                    {
                        // clear list

                        TariffsCleaningRequiered(null, null);

                        for (int i = 0; i < 4; i++)
                        {
                            var result = await apiClient.SendPostAsync(ApiStrings.GetTariffs, new { Type = (TariffType)i }, true);

                            if (result.IsSuccessStatusCode)
                            {
                                var tariffs = JsonConvert.DeserializeObject<List<Tariff>>(await result.Content.ReadAsStringAsync());

                                foreach (var t in tariffs)
                                {
                                    TariffAdditionRequiered(null, new OnAddNewTariffEventArgs() { Tariff = t, Type = (TariffType)i });
                                }
                            }
                        }


                    }
                    ));
                }
        }

        private RelayCommand getTariffInfo;
        public RelayCommand GetTariffInfo
        {
            get
            {
                return getTariffInfo ??
                    (getTariffInfo = new RelayCommand(obj =>
                    {
                        if (obj is null) return;

                        var tariff = (Tariff)obj;

                        InfoVisibility = true;

                        SelectedTariff = tariff;
                    }));
            }
        }

        private RelayCommand updateAllTariffsCommand;
        public RelayCommand UpdateAllTariffsCommand
        {
            get
            {
                return updateAllTariffsCommand ??
                    (updateAllTariffsCommand = new RelayCommand(obj =>
                    {
                        UpdateMyTariffCommand.Execute(null);
                        LoadTariffsCommand.Execute(null);
                    }));
            }
        }
        
        private RelayCommand closeInfoDialogCommand;
        public RelayCommand CloseInfoDialogCommand
        {
            get
            {
                return closeInfoDialogCommand ??
                    (closeInfoDialogCommand = new RelayCommand(obj =>
                    {
                        InfoVisibility = false;
                    }));
            }
        }

        private RelayCommand openChangeTariffDialogCommand;
        public RelayCommand OpenChangeTariffDialogCommand
        {
            get
            {
                return openChangeTariffDialogCommand ??
                    (openChangeTariffDialogCommand = new RelayCommand(obj =>
                    {
                        if (obj is null) return;

                        var tariff = (Tariff)obj;

                        SelectedTariff = tariff;

                        ChangeTariffDialogVisibility = true;

                    }));
            }
        }

        private RelayCommand closeChangeTariffDialogCommand;
        public RelayCommand CloseChangeTariffDialogCommand
        {
            get
            {
                return closeChangeTariffDialogCommand ??
                    (closeChangeTariffDialogCommand = new RelayCommand(obj =>
                    {
                        ChangeTariffDialogVisibility = false;
                    }));
            }
        }

        private RelayCommand confirmTariffChangingCommand;
        public RelayCommand ConfirmTariffChangingCommand
        {
            get
            {
                return confirmTariffChangingCommand ??
                    (confirmTariffChangingCommand = new RelayCommand(async obj =>
                    {
                        ChangeTariffDialogVisibility = false;

                        var result = await apiClient.SendPostAsync(ApiStrings.ChangeMyTariff, new { Id = SelectedTariff.Id }, true);

                        if (result.IsSuccessStatusCode)
                        {
                            Growl.SuccessGlobal("Тариф успешно подключен");

                            TariffsCleaningRequiered.Invoke(null, null);

                            UpdateAllTariffsCommand.Execute(null);
                        }
                        else
                        {
                            ErrorResponseModel error = JsonConvert.DeserializeObject<ErrorResponseModel>(await result.Content.ReadAsStringAsync());

                            ErrorMessage = error.Error;

                            ErrorVisibility = true;
                        }

                    }));
            }
        }

        public event EventHandler TariffAdditionRequiered;
        public event EventHandler TariffsCleaningRequiered;

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
