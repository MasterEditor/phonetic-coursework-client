using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Coursework_Client.Models;
using System.IO;
using System.Windows;
using Coursework_Client.Managers;
using Coursework_Client.XAML.Views;
using System.ComponentModel;
using Coursework_Client.Models.API;
using Newtonsoft.Json;
using Coursework_Client.Models.API.Receiving;
using HandyControl.Data;
using Coursework_Client.Models.API.Sending;

namespace Coursework_Client.ViewModels
{
    class NewsPageViewModel : INotifyPropertyChanged
    {
        private ApiClient apiClient = new ApiClient();

        public ObservableCollection<NewsArticle> News { get; set; }

        public NewsArticle SelectedArticle { get; set; }

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

        private int pageIndex = 1;
        public int PageIndex
        {
            get { return pageIndex; }
            set
            {
                pageIndex = value;
                OnPropertyChanged("PageIndex");
            }
        }

        private int maxPageCount = 1;
        public int MaxPageCount
        {
            get { return maxPageCount; }
            set
            {
                maxPageCount = value;
                OnPropertyChanged("MaxPageCount");
            }
        }

        private string searchString;
        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                OnPropertyChanged("SearchString");
            }
        }

       

        private SearchType searchType;
        public SearchType SearchType
        {
            get { return searchType; }
            set
            {
                searchType = value;
                OnPropertyChanged("SearchType");
            }
        }

        private DateTime? fromDate;
        public DateTime? FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        private DateTime? toDate;
        public DateTime? ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        public NewsPageViewModel()
        {
            News = new ObservableCollection<NewsArticle>();
            LoadNewsCommand.Execute(new FunctionEventArgs<int>(1));
        }


        private RelayCommand loadNewsCommand;
        public RelayCommand LoadNewsCommand
        {
            get
            {
                return loadNewsCommand ??
                    (loadNewsCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        int page;
                        if(obj is null)
                        {
                            page = 1;
                            PageIndex = 1;
                        }
                        else page = ((FunctionEventArgs<int>)obj).Info;

                        var requestModel = new GetNewsRequestModel();
                        requestModel.Page = page;

                        if (!string.IsNullOrWhiteSpace(SearchString))
                        {
                            requestModel.SearchString = SearchString;
                            requestModel.SearchType = SearchType;
                        }

                        if (FromDate.HasValue) requestModel.From = FromDate.Value;
                        if (ToDate.HasValue) requestModel.To = ToDate.Value;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetNews, requestModel);

                        if (result.IsSuccessStatusCode)
                        {
                            //GetNewsResponseModel news = JsonConvert.DeserializeObject<GetNewsResponseModel>(await result.Content.ReadAsStringAsync());

                            GetNewsResponseModel news = await Task.Run(async () => { return JsonConvert.DeserializeObject<GetNewsResponseModel>(await result.Content.ReadAsStringAsync()); });

                            MaxPageCount = news.TotalPages > 0 ? news.TotalPages : 1;

                            News.Clear();

                            foreach (var n in news.News)
                            {
                                News.Add(n);
                            }

                            LoadingVisibility = false;

                            foreach(var n in news.News)
                            {
                                var bytes = await apiClient.LoadImageAsync(n.ImagePath);
                                n.Image = bytes;
                            }

                            return;

                        }

                        LoadingVisibility = false;

                    }));
            }
        }

        private RelayCommand openNewsArticle;
        public RelayCommand OpenNewsArticle
        {
            get
            {
                return openNewsArticle ??
                    (openNewsArticle = new RelayCommand(obj =>
                    {
                        PagesManager.OpenInnerPage(new ArticlePage(Int32.Parse((string)obj)));
                    }));
            }
        }

        private RelayCommand clearCommand;
        public RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ??
                    (clearCommand = new RelayCommand(obj =>
                    {
                        SearchString = "";
                        SearchType = SearchType.Mixed;
                        FromDate = null;
                        ToDate = null;
                        LoadNewsCommand.Execute(null);
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
