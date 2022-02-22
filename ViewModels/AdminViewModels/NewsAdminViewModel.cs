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

namespace Coursework_Client.ViewModels
{
    class NewsAdminViewModel : INotifyPropertyChanged
    {
        public NewsAdminViewModel()
        {
            News = new ObservableCollection<NewsArticle>();
            LoadNewsCommand.Execute(new FunctionEventArgs<int>(1));
        }

        private ObservableCollection<ImageModel> images = new ObservableCollection<ImageModel>();
        public ObservableCollection<ImageModel> Images
        {
            get { return images; }
            set
            {
                images = value;
                OnPropertyChanged("Images");
            }
        }

        private ApiClient apiClient = new ApiClient();

        public ObservableCollection<NewsArticle> News { get; set; }

        public NewsArticle SelectedArticle { get; set; }

        private NewsArticle tempSelectedArticle;
        public NewsArticle TempSelectedArticle
        {
            get { return tempSelectedArticle; }
            set
            {
                tempSelectedArticle = value;
                OnPropertyChanged("TempSelectedArticle");
            }
        }

        private NewsArticle tempAddArticle;
        public NewsArticle TempAddArticle
        {
            get { return tempAddArticle; }
            set
            {
                tempAddArticle = value;
                OnPropertyChanged("TempAddArticle");
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

        private bool editArticleVisibility;
        public bool EditArticleVisibility
        {
            get { return editArticleVisibility; }
            set
            {
                editArticleVisibility = value;
                OnPropertyChanged("EditArticleVisibility");
            }
        }

        private bool addArticleVisibility;
        public bool AddArticleVisibility
        {
            get { return addArticleVisibility; }
            set
            {
                addArticleVisibility = value;
                OnPropertyChanged("AddArticleVisibility");
            }
        }

        private bool imageListVisibility;
        public bool ImageListVisibility
        {
            get { return imageListVisibility; }
            set
            {
                imageListVisibility = value;
                OnPropertyChanged("ImageListVisibility");
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
                        if (obj is null)
                        {
                            page = 1;
                            PageIndex = 1;
                        }
                        else if(obj is int)
                        {
                            page = PageIndex;
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
                            GetNewsResponseModel news = JsonConvert.DeserializeObject<GetNewsResponseModel>(await result.Content.ReadAsStringAsync());

                            MaxPageCount = news.TotalPages > 0 ? news.TotalPages : 1;

                            News.Clear();

                            foreach (var n in news.News)
                            {
                                News.Add(n);
                            }

                            LoadingVisibility = false;

                            return;

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

        private RelayCommand removeArticleCommand;
        public RelayCommand RemoveArticleCommand
        {
            get
            {
                return removeArticleCommand ??
                    (removeArticleCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.RemoveArticle, new { Id = SelectedArticle.Id }, true);

                        if (result.IsSuccessStatusCode)
                        {
                            LoadingVisibility = false;
                            LoadNewsCommand.Execute(PageIndex);
                            return;
                        }
                        ErrorResponseModel err_model = JsonConvert.DeserializeObject<ErrorResponseModel>(await result.Content.ReadAsStringAsync());
                        ShowError(err_model.Error);

                        LoadingVisibility = false;
                    }, obj =>
                    {
                        if (SelectedArticle is null) return false;
                        return true;
                    }));
            }
        }

        private RelayCommand editArticleCommand;
        public RelayCommand EditArticleCommand
        {
            get
            {
                return editArticleCommand ??
                    (editArticleCommand = new RelayCommand(async obj =>
                    {
                        TempSelectedArticle = (NewsArticle)SelectedArticle.Clone();
                        EditArticleVisibility = true;
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetOneNews, new {SelectedArticle.Id });

                        if (result.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<NewsArticle>(await result.Content.ReadAsStringAsync());

                            TempSelectedArticle.Content = temp.Content;
                        }

                        LoadingVisibility = false;
                    }, obj =>
                    {
                        if (SelectedArticle is null) return false;
                        return true;
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
                        EditArticleVisibility = false;
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
                        AddArticleVisibility = false;
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
                        var result = await apiClient.SendPostAsync(ApiStrings.EditArticle, TempSelectedArticle, true);
                        if (result.IsSuccessStatusCode)
                        {
                            EditArticleVisibility = false;
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
                        var result = await apiClient.SendPostAsync(ApiStrings.AddArticle, TempAddArticle, true);
                        if (result.IsSuccessStatusCode)
                        {
                            AddArticleVisibility = false;
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

        private RelayCommand addArticleCommand;
        public RelayCommand AddArticleCommand
        {
            get
            {
                return addArticleCommand ??
                    (addArticleCommand = new RelayCommand(async obj =>
                    {
                        TempAddArticle = new NewsArticle();
                        AddArticleVisibility = true;
                    }));
            }
        }

        private RelayCommand loadImagesCommand;
        public RelayCommand LoadImagesCommand
        {
            get
            {
                return loadImagesCommand ??
                    (loadImagesCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetAllImages, null, false);

                        if (result.IsSuccessStatusCode)
                        {
                            var response = JsonConvert.DeserializeObject<string[]>(await result.Content.ReadAsStringAsync());

                            LoadingVisibility = false;

                            Images.Clear();

                            foreach (var img in response)
                            {
                                ImageModel model = new ImageModel() { Path = img };
                                var bytes = await apiClient.LoadImageAsync(img);
                                Images.Add(new ImageModel { Path = img, Bytes = bytes });
                            }

                            return;
                        }

                        LoadingVisibility = false;
                    }));
            }
        }

        private RelayCommand addImageCommand;
        public RelayCommand AddImageCommand
        {
            get
            {
                return addImageCommand ??
                    (addImageCommand = new RelayCommand(obj =>
                    {
                        LoadImagesCommand.Execute(null);
                        ImageListVisibility = true;
                    }));
            }
        }

        private RelayCommand confirmAddingImageCommand;
        public RelayCommand ConfirmAddingImageCommand
        {
            get
            {
                return confirmAddingImageCommand ??
                    (confirmAddingImageCommand = new RelayCommand(obj =>
                    {
                        var path = obj as string;
                        if (path is null) return;
                        if(TempSelectedArticle != null) TempSelectedArticle.ImagePath = path;
                        if (TempAddArticle != null) TempAddArticle.ImagePath = path;
                        ImageListVisibility = false;
                    }));
            }
        }

        private RelayCommand closeImageDialogCommand;
        public RelayCommand CloseImageDialogCommand
        {
            get
            {
                return closeImageDialogCommand ??
                    (closeImageDialogCommand = new RelayCommand(obj =>
                    {
                        ImageListVisibility = false;
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
