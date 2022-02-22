using Coursework_Client.Managers;
using Coursework_Client.Models;
using Coursework_Client.Models.API;
using Coursework_Client.Models.API.Receiving;
using Coursework_Client.Models.API.Sending;
using Coursework_Client.XAML.Views;
using HandyControl.Data;
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
    class TariffsAdminViewModel : INotifyPropertyChanged
    {
        public TariffsAdminViewModel()
        {
            LoadTariffsCommand.Execute(null);
        }

        private ApiClient apiClient = new ApiClient();

        private ObservableCollection<Tariff> tariffs = new ObservableCollection<Tariff>();
        public ObservableCollection<Tariff> Tariffs
        {
            get { return tariffs; }
            set
            {
                tariffs = value;
                OnPropertyChanged("Tariffs");
            }
        }

        private Tariff tempSelectedTariff;
        public Tariff TempSelectedTariff
        {
            get { return tempSelectedTariff; }
            set
            {
                tempSelectedTariff = value;
                OnPropertyChanged("TempSelectedTariff");
            }
        }

        private Tariff tempAddTariff;
        public Tariff TempAddTariff
        {
            get { return tempAddTariff; }
            set
            {
                tempAddTariff = value;
                OnPropertyChanged("TempAddTariff");
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

        private bool editTariffVisibility;
        public bool EditTariffVisibility
        {
            get { return editTariffVisibility; }
            set
            {
                editTariffVisibility = value;
                OnPropertyChanged("EditTariffVisibility");
            }
        }

        private bool addTariffVisibility;
        public bool AddTariffVisibility
        {
            get { return addTariffVisibility; }
            set
            {
                addTariffVisibility = value;
                OnPropertyChanged("AddTariffVisibility");
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

        private RelayCommand loadTariffsCommand;
        public RelayCommand LoadTariffsCommand
        {
            get
            {
                return loadTariffsCommand ??
                    (loadTariffsCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetAllTariffs, null, true);

                        if (result.IsSuccessStatusCode)
                        {
                            List<Tariff> tariffs = JsonConvert.DeserializeObject<List<Tariff>>(await result.Content.ReadAsStringAsync());

                            tariffs = tariffs.OrderBy(t => t.Type).ToList();

                            Tariffs = new ObservableCollection<Tariff>(tariffs);
                        }

                        LoadingVisibility = false;

                    }));
            }
        }

        private RelayCommand removeTariffCommand;
        public RelayCommand RemoveTariffCommand
        {
            get
            {
                return removeTariffCommand ??
                    (removeTariffCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.RemoveTariff, new { Id = SelectedTariff.Id }, true);

                        if (result.IsSuccessStatusCode)
                        {
                            LoadingVisibility = false;
                            LoadTariffsCommand.Execute(null);
                            return;
                        }
                        ErrorResponseModel err_model = JsonConvert.DeserializeObject<ErrorResponseModel>(await result.Content.ReadAsStringAsync());
                        ShowError(err_model.Error);

                        LoadingVisibility = false;
                    }, obj =>
                    {
                        if (SelectedTariff is null) return false;
                        return true;
                    }));
            }
        }

        private RelayCommand editTariffCommand;
        public RelayCommand EditTariffCommand
        {
            get
            {
                return editTariffCommand ??
                    (editTariffCommand = new RelayCommand(obj =>
                    {
                        TempSelectedTariff = (Tariff)SelectedTariff.Clone();
                        EditTariffVisibility = true;
                    }, obj =>
                    {
                        if (SelectedTariff is null) return false;
                        return true;
                    }));
            }
        }

        private RelayCommand addTariffCommand;
        public RelayCommand AddTariffCommand
        {
            get
            {
                return addTariffCommand ??
                    (addTariffCommand = new RelayCommand(obj =>
                    {
                        TempAddTariff = new Tariff();
                        AddTariffVisibility = true;
                    }));
            }
        }

        private RelayCommand closeEditingDialogCommand;
        public RelayCommand CloseEditingDialogCommand
        {
            get
            {
                return closeEditingDialogCommand ??
                    (closeEditingDialogCommand = new RelayCommand(obj =>
                    {
                        EditTariffVisibility = false;
                    }));
            }
        }

        private RelayCommand closeAddingDialogCommand;
        public RelayCommand CloseAddingDialogCommand
        {
            get
            {
                return closeAddingDialogCommand ??
                    (closeAddingDialogCommand = new RelayCommand(obj =>
                    {
                        AddTariffVisibility = false;
                    }));
            }
        }

        private RelayCommand acceptEditingCommand;
        public RelayCommand AcceptEditingCommand
        {
            get
            {
                return acceptEditingCommand ??
                    (acceptEditingCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;
                        var result = await apiClient.SendPostAsync(ApiStrings.EditTariff, TempSelectedTariff, true);
                        if (result.IsSuccessStatusCode)
                        {
                            EditTariffVisibility = false;
                            LoadTariffsCommand.Execute(null);
                        }
                        else
                        {
                            ErrorResponseModel err_model = JsonConvert.DeserializeObject<ErrorResponseModel>(await result.Content.ReadAsStringAsync());
                            ShowError(err_model.Error);
                        }
                        LoadingVisibility = false;
                    }));
            }
        }

        private RelayCommand acceptAddingCommand;
        public RelayCommand AcceptAddingCommand
        {
            get
            {
                return acceptAddingCommand ??
                    (acceptAddingCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;
                        var result = await apiClient.SendPostAsync(ApiStrings.AddTariff, TempAddTariff, true);
                        if (result.IsSuccessStatusCode)
                        {
                            AddTariffVisibility = false;
                            LoadTariffsCommand.Execute(null);
                        }
                        else
                        {
                            ErrorResponseModel err_model = JsonConvert.DeserializeObject<ErrorResponseModel>(await result.Content.ReadAsStringAsync());
                            ShowError(err_model.Error);
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
