using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coursework_Client.Models;
using System.Threading.Tasks;
using System.ComponentModel;
using Coursework_Client.Managers;
using Coursework_Client.Models.API;
using Newtonsoft.Json;

namespace Coursework_Client.ViewModels
{
    class ArticlePageViewModel : INotifyPropertyChanged
    {
        public ArticlePageViewModel(int id)
        {
            Id = id;
            LoadDataCommand.Execute(null);
        }

        private ApiClient apiClient = new ApiClient();
        private int Id;

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

        private NewsArticle article;
        public NewsArticle Article
        {
            get { return article; }
            set
            {
                article = value;
                OnPropertyChanged("Article");
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

        private RelayCommand loadDataCommand;
        public RelayCommand LoadDataCommand
        {
            get
            {
                return loadDataCommand ??
                    (loadDataCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetOneNews, new { Id = Id });

                        if (result.IsSuccessStatusCode)
                        {
                            Article = JsonConvert.DeserializeObject<NewsArticle>(await result.Content.ReadAsStringAsync());

                            var bytes = await apiClient.LoadImageAsync(Article.ImagePath);
                            Article.Image = bytes;
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
