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
using System.Windows.Controls;

namespace Coursework_Client.ViewModels
{
    class ServicesAdminViewModel : INotifyPropertyChanged
    {
        public ServicesAdminViewModel()
        {
            LoadServicesCommand.Execute(null);
        }

        private ApiClient apiClient = new ApiClient();

        private ObservableCollection<Service> services = new ObservableCollection<Service>();
        public ObservableCollection<Service> Services
        {
            get { return services; }
            set
            {
                services = value;
                OnPropertyChanged("Services");
            }
        }

        private ObservableCollection<string> headers = new ObservableCollection<string>();
        public ObservableCollection<string> Headers
        {
            get { return headers; }
            set
            {
                headers = value;
                OnPropertyChanged("Headers");
            }
        }

        private Service selectedService;
        public Service SelectedService
        {
            get { return selectedService; }
            set
            {
                selectedService = value;
                OnPropertyChanged("SelectedService");
            }
        }

        private Service tempSelectedSerive;
        public Service TempSelectedService
        {
            get { return tempSelectedSerive; }
            set
            {
                tempSelectedSerive = value;
                OnPropertyChanged("TempSelectedService");
            }
        }

        private Service tempAddService;
        public Service TempAddService
        {
            get { return tempAddService; }
            set
            {
                tempAddService = value;
                OnPropertyChanged("TempAddService");
            }
        }

        private int openedPage = 1;
        public int OpenedPage
        {
            get { return openedPage; }
            set
            {
                openedPage = value;
                OnPropertyChanged("OpenedPage");
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

        private bool editServiceVisibility;
        public bool EditServiceVisibility
        {
            get { return editServiceVisibility; }
            set
            {
                editServiceVisibility = value;
                OnPropertyChanged("EditServiceVisibility");
            }
        }

        private bool addServiceVisibility;
        public bool AddServiceVisibility
        {
            get { return addServiceVisibility; }
            set
            {
                addServiceVisibility = value;
                OnPropertyChanged("AddServiceVisibility");
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

        private string searchQuery;
        public string SearchQuery
        {
            get { return searchQuery; }
            set
            {
                searchQuery = value;
                OnPropertyChanged("SearchQuery");
            }
        }

        private string searchHeader = "Все категории";
        public string SearchHeader
        {
            get { return searchHeader; }
            set
            {
                searchHeader = value;
                OnPropertyChanged("SearchHeader");
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

        private RelayCommand loadServicesCommand;
        public RelayCommand LoadServicesCommand
        {
            get
            {
                return loadServicesCommand ??
                    (loadServicesCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetAllServices, new { Header = SearchHeader == "Все категории" ? "" : SearchHeader, SearchQuery = SearchQuery }, true);

                        if (result.IsSuccessStatusCode)
                        {
                            var services = JsonConvert.DeserializeObject<List<Service>>(await result.Content.ReadAsStringAsync());

                            Services = new ObservableCollection<Service>(services.OrderBy(s => s.Header).ToList());

                            var headers = (from s in services
                                           select s.Header).Distinct();

                            var tempHeader = SearchHeader;

                            Headers = new ObservableCollection<string>(headers) { "Все категории" };

                            SearchHeader = tempHeader;
                        }

                        LoadingVisibility = false;

                    }));
            }
        }

        private RelayCommand clearCommand;
        public RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ??
                    (clearCommand = new RelayCommand(async obj =>
                    {
                        SearchHeader = "Все категории";

                        SearchQuery = "";

                        LoadServicesCommand.Execute(null);

                    }));
            }
        }

        private RelayCommand removeServiceCommand;
        public RelayCommand RemoveServiceCommand
        {
            get
            {
                return removeServiceCommand ??
                    (removeServiceCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.RemoveService, new { Id = SelectedService.Id }, true);

                        if (result.IsSuccessStatusCode)
                        {
                            LoadingVisibility = false;
                            LoadServicesCommand.Execute(null);
                            return;
                        }
                        ErrorResponseModel err_model = JsonConvert.DeserializeObject<ErrorResponseModel>(await result.Content.ReadAsStringAsync());
                        ShowError(err_model.Error);

                        LoadingVisibility = false;
                    }, obj =>
                    {
                        if (SelectedService is null) return false;
                        return true;
                    }));
            }
        }

        private RelayCommand editServiceCommand;
        public RelayCommand EditServiceCommand
        {
            get
            {
                return editServiceCommand ??
                    (editServiceCommand = new RelayCommand(obj =>
                    {
                        TempSelectedService = (Service)SelectedService.Clone();
                        EditServiceVisibility = true;
                    }, obj =>
                    {
                        if (SelectedService is null) return false;
                        return true;
                    }));
            }
        }

        private RelayCommand addServiceCommand;
        public RelayCommand AddServiceCommand
        {
            get
            {
                return addServiceCommand ??
                    (addServiceCommand = new RelayCommand(obj =>
                    {
                        TempAddService = new Service();
                        AddServiceVisibility = true;
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
                        EditServiceVisibility = false;
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
                        AddServiceVisibility = false;
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

                        if (TempSelectedService.Duration.Contains("Infinite")) TempSelectedService.Duration = "Infinite";
                        else if (TempSelectedService.Duration.Contains("Month")) TempSelectedService.Duration = "Month";
                        else if (TempSelectedService.Duration.Contains("Day")) TempSelectedService.Duration = "Day";

                        var result = await apiClient.SendPostAsync(ApiStrings.EditService, tempSelectedSerive, true);
                        if (result.IsSuccessStatusCode)
                        {
                            EditServiceVisibility = false;
                            LoadServicesCommand.Execute(null);
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

                        if (TempAddService.Duration.Contains("Infinite")) TempAddService.Duration = "Infinite";
                        else if (TempAddService.Duration.Contains("Month")) TempAddService.Duration = "Month";
                        else if (TempAddService.Duration.Contains("Day")) TempAddService.Duration = "Day";

                        var result = await apiClient.SendPostAsync(ApiStrings.AddService, TempAddService, true);
                        if (result.IsSuccessStatusCode)
                        {
                            AddServiceVisibility = false;
                            LoadServicesCommand.Execute(null);
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
