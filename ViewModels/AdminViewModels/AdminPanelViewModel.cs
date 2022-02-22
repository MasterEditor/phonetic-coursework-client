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
    class AdminPanelViewModel : INotifyPropertyChanged
    {
        private ApiClient apiClient = new ApiClient();

        private ChartValues<int> visitorsValues;
        public ChartValues<int> VisitorsValues
        {
            get { return visitorsValues; }
            set
            {
                visitorsValues = value;
                OnPropertyChanged("VisitorsValues");
            }
        }

        private ChartValues<int> registrationsValues;
        public ChartValues<int> RegistrationsValues
        {
            get { return registrationsValues; }
            set
            {
                registrationsValues = value;
                OnPropertyChanged("RegistrationsValues");
            }
        }

        public AdminPanelViewModel()
        {
            VisitorsValues = new ChartValues<int>();
            RegistrationsValues = new ChartValues<int>();

            LoadStatisticsCommand.Execute("Visitors");
            LoadStatisticsCommand.Execute("Registrations");
        }

        private RelayCommand openNewsAdminCommand;
        public RelayCommand OpenNewsAdminCommand
        {
            get
            {
                return openNewsAdminCommand ??
                    (openNewsAdminCommand = new RelayCommand(obj =>
                    {
                        PagesManager.OpenInnerPage(new NewsAdminPage());
                    }));
            }
        }

        private RelayCommand openImagesAdminCommand;
        public RelayCommand OpenImagesAdminCommand
        {
            get
            {
                return openImagesAdminCommand ??
                    (openImagesAdminCommand = new RelayCommand(obj =>
                    {
                        PagesManager.OpenInnerPage(new ImagesAdminPage());
                    }));
            }
        }

        private RelayCommand openServicesAdminCommand;
        public RelayCommand OpenServicesAdminCommand
        {
            get
            {
                return openServicesAdminCommand ??
                    (openServicesAdminCommand = new RelayCommand(obj =>
                    {
                        PagesManager.OpenInnerPage(new ServicesAdminPage());
                    }));
            }
        }

        private RelayCommand openTariffsAdminCommand;
        public RelayCommand OpenTariffsAdminCommand
        {
            get
            {
                return openTariffsAdminCommand ??
                    (openTariffsAdminCommand = new RelayCommand(obj =>
                    {
                        PagesManager.OpenInnerPage(new TariffsAdminPage());
                    }));
            }
        }

        private RelayCommand openPromotionsAdminCommand;
        public RelayCommand OpenPromotionsAdminCommand
        {
            get
            {
                return openPromotionsAdminCommand ??
                    (openPromotionsAdminCommand = new RelayCommand(obj =>
                    {
                        PagesManager.OpenInnerPage(new PromotionsAdminPage());
                    }));
            }
        }

        private RelayCommand openUsersAdminCommand;
        public RelayCommand OpenUsersAdminCommand
        {
            get
            {
                return openUsersAdminCommand ??
                    (openUsersAdminCommand = new RelayCommand(obj =>
                    {
                        PagesManager.OpenInnerPage(new UsersAdminPage());
                    }));
            }
        }

        private RelayCommand loadStatisticsCommand;
        public RelayCommand LoadStatisticsCommand
        {
            get
            {
                return loadStatisticsCommand ??
                    (loadStatisticsCommand = new RelayCommand(async obj =>
                    {
                        string type = obj as string;
                        if (type is null) return;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetStatistics, new { Type = type }, true);

                        if (result.IsSuccessStatusCode)
                        {
                            List<Statistic> statistics = JsonConvert.DeserializeObject<List<Statistic>>(await result.Content.ReadAsStringAsync());

                            if(statistics.Count > 0)
                            {
                                if(statistics[0].Type == "Visitors")
                                {
                                    VisitorsValues.Clear();

                                    VisitorsValues.AddRange(statistics.Select(c => c.Value));
                                }
                                else
                                {
                                    RegistrationsValues.Clear();

                                    RegistrationsValues.AddRange(statistics.Select(c => c.Value));
                                }
                            }
                        }


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
