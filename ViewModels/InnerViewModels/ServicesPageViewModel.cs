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
using HandyControl.Controls;
using Newtonsoft.Json;

namespace Coursework_Client.ViewModels
{
    class ServicesPageViewModel : INotifyPropertyChanged
    {
        private ApiClient apiClient = new ApiClient();

        public ServicesPageViewModel()
        {
            //OpenMyServicesCommand.Execute(null);
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

        private ObservableCollection<Service> myServices = new ObservableCollection<Service>();
        public ObservableCollection<Service> MyServices
        {
            get { return myServices; }
            set
            {
                myServices = value;
                OnPropertyChanged("MyServices");
            }
        }

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

        private bool onSubscribeDialogVisibility;
        public bool OnSubscribeDialogVisibility
        {
            get { return onSubscribeDialogVisibility; }
            set
            {
                onSubscribeDialogVisibility = value;
                OnPropertyChanged("OnSubscribeDialogVisibility");
            }
        }

        private bool onUnsubscribeDialogVisibility;
        public bool OnUnsubscribeDialogVisibility
        {
            get { return onUnsubscribeDialogVisibility; }
            set
            {
                onUnsubscribeDialogVisibility = value;
                OnPropertyChanged("OnUnsubscribeDialogVisibility");
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

        private RelayCommand cancelSubscribingDialogCommand;
        public RelayCommand CancelSubscribingDialogCommand
        {
            get
            {
                return cancelSubscribingDialogCommand ??
                    (cancelSubscribingDialogCommand = new RelayCommand(obj =>
                    {
                        OnSubscribeDialogVisibility = false;
                        OnUnsubscribeDialogVisibility = false;                      
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

        private RelayCommand loadServicesCommand;
        public RelayCommand LoadServicesCommand
        {
            get
            {
                return loadServicesCommand ??
                    (loadServicesCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetServices, new { Header = SearchHeader == "Все категории" ? "" : SearchHeader, SearchQuery = SearchQuery }, true);

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
        
        private RelayCommand loadMyServicesCommand;
        public RelayCommand LoadMyServicesCommand
        {
            get
            {
                return loadMyServicesCommand ??
                    (loadMyServicesCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetMyServices, null, true);

                        if (result.IsSuccessStatusCode)
                        {
                            var services = JsonConvert.DeserializeObject<List<Service>>(await result.Content.ReadAsStringAsync());

                            MyServices = new ObservableCollection<Service>(services.OrderBy(s => s.Header).ToList());
                        }

                        LoadingVisibility = false;

                    }));
            }
        }

        private RelayCommand openMyServicesCommand;
        public RelayCommand OpenMyServicesCommand
        {
            get
            {
                return openMyServicesCommand ??
                    (openMyServicesCommand = new RelayCommand(obj =>
                    {
                        if(OpenedPage == 1)
                        {
                            ServicesSourceChanged(null, new IntEventArgs { Number = 0 });
                            OpenedPage = 0;
                            LoadMyServicesCommand.Execute(null);
                        }
                        
                    }));
            }
        }
        
        private RelayCommand openAllServicesCommand;
        public RelayCommand OpenAllServicesCommand
        {
            get
            {
                return openAllServicesCommand ??
                    (openAllServicesCommand = new RelayCommand(obj =>
                    {
                        if(OpenedPage == 0)
                        {
                            ServicesSourceChanged(null, new IntEventArgs { Number = 1 });
                            OpenedPage = 1;
                            LoadServicesCommand.Execute(null);
                        }
                        
                    }));
            }
        }

        private RelayCommand onSubscribeCommand;
        public RelayCommand OnSubscribeCommand
        {
            get
            {
                return onSubscribeCommand ??
                    (onSubscribeCommand = new RelayCommand(obj =>
                    {
                        Service service = (Service)obj;

                        if (service is null) return;

                        SelectedService = service;

                        OnSubscribeDialogVisibility = true;
                    }));
            }
        }

        private RelayCommand onUnsubscribeCommand;
        public RelayCommand OnUnsubscribeCommand
        {
            get
            {
                return onUnsubscribeCommand ??
                    (onUnsubscribeCommand = new RelayCommand(obj =>
                    {
                        Service service = (Service)obj;

                        if (service is null) return;

                        SelectedService = service;

                        OnUnsubscribeDialogVisibility = true;
                    }));
            }
        }

        private RelayCommand confirmSubscribingCommand;
        public RelayCommand ConfirmSubscribingCommand
        {
            get
            {
                return confirmSubscribingCommand ??
                    (confirmSubscribingCommand = new RelayCommand(async obj =>
                    {
                        OnSubscribeDialogVisibility = false;

                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.SubscribeToService, new { Id = SelectedService.Id }, true);

                        LoadingVisibility = false;

                        if (result.IsSuccessStatusCode)
                        {
                            Growl.SuccessGlobal("Услуга успешно подключена");
                            LoadServicesCommand.Execute(null);
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
        
        private RelayCommand confirmUnsubscribingCommand;
        public RelayCommand ConfirmUnsubscribingCommand
        {
            get
            {
                return confirmUnsubscribingCommand ??
                    (confirmUnsubscribingCommand = new RelayCommand(async obj =>
                    {
                        OnUnsubscribeDialogVisibility = false;

                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.UnsubscribeFromService, new { Id = SelectedService.Id }, true);

                        LoadingVisibility = false;

                        if (result.IsSuccessStatusCode)
                        {
                            Growl.SuccessGlobal("Услуга успешно отключена");
                            LoadMyServicesCommand.Execute(null);

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

        public event EventHandler ServicesSourceChanged;
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
