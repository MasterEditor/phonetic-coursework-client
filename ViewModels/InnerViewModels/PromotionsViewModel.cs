using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coursework_Client.Models;
using Coursework_Client.Models.API;
using Newtonsoft.Json;

namespace Coursework_Client.ViewModels
{
    class PromotionsViewModel : INotifyPropertyChanged
    {
        private ApiClient apiClient = new ApiClient();

        public ObservableCollection<Promotion> PromotionsView { get; set; }

        public PromotionsViewModel()
        {
            PromotionsView = new ObservableCollection<Promotion>();
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

        private RelayCommand loadPromotionsCommand;
        public RelayCommand LoadPromotionsCommand
        {
            get
            {
                return loadPromotionsCommand ??
                    (loadPromotionsCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        PromotionsView.Clear();

                        var result = await apiClient.SendPostAsync(ApiStrings.GetPromotions, null);

                        if (result.IsSuccessStatusCode)
                        {
                            var promos = JsonConvert.DeserializeObject<List<Promotion>>(await result.Content.ReadAsStringAsync());

                            foreach(var promo in promos)
                            {
                                PromotionsView.Add(promo);
                            }

                            LoadingVisibility = false;

                            foreach (var promo in promos)
                            {
                                var bytes = await apiClient.LoadImageAsync(promo.ImagePath);
                                promo.Image = bytes;
                            }

                            return;
                        }

                        LoadingVisibility = false;
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
