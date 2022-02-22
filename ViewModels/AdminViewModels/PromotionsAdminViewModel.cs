using Coursework_Client.Managers;
using Coursework_Client.Models;
using Coursework_Client.Models.API;
using Coursework_Client.Models.API.Receiving;
using Coursework_Client.Models.API.Sending;
using Coursework_Client.XAML.Views;
using HandyControl.Data;
using Microsoft.Win32;
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
    class PromotionsAdminViewModel : INotifyPropertyChanged
    {
        private ApiClient apiClient = new ApiClient();

        private ObservableCollection<Promotion> images = new ObservableCollection<Promotion>();
        public ObservableCollection<Promotion> Images
        {
            get { return images; }
            set
            {
                images = value;
                OnPropertyChanged("Images");
            }
        }

        private ObservableCollection<ImageModel> localImages = new ObservableCollection<ImageModel>();
        public ObservableCollection<ImageModel> LocalImages
        {
            get { return localImages; }
            set
            {
                localImages = value;
                OnPropertyChanged("LocalImages");
            }
        }




        private Promotion selectedPromotion;
        public Promotion SelectedPromotion
        {
            get { return selectedPromotion; }
            set
            {
                selectedPromotion = value;
                OnPropertyChanged("SelectedPromotion");
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

        private RelayCommand loadImagesCommand;
        public RelayCommand LoadImagesCommand
        {
            get
            {
                return loadImagesCommand ??
                    (loadImagesCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetPromotions, null, false);

                        if (result.IsSuccessStatusCode)
                        {
                            var response = JsonConvert.DeserializeObject<List<Promotion>>(await result.Content.ReadAsStringAsync());

                            LoadingVisibility = false;

                            Images.Clear();

                            foreach (var img in response)
                            {
                                Promotion model = new Promotion() { ImagePath = img.ImagePath, Id = img.Id };
                                var bytes = await apiClient.LoadImageAsync(img.ImagePath);
                                Images.Add(new Promotion { ImagePath = img.ImagePath, Image = bytes, Id = img.Id });
                            }

                            return;
                        }

                        LoadingVisibility = false;
                    }));
            }
        }

        private RelayCommand loadLocalImagesCommand;
        public RelayCommand LoadLocalImagesCommand
        {
            get
            {
                return loadLocalImagesCommand ??
                    (loadLocalImagesCommand = new RelayCommand(async obj =>
                    {
                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.GetAllImages, null, false);

                        if (result.IsSuccessStatusCode)
                        {
                            var response = JsonConvert.DeserializeObject<string[]>(await result.Content.ReadAsStringAsync());

                            LoadingVisibility = false;

                            LocalImages.Clear();

                            foreach (var img in response)
                            {
                                ImageModel model = new ImageModel() { Path = img };
                                var bytes = await apiClient.LoadImageAsync(img);
                                LocalImages.Add(new ImageModel { Path = img, Bytes = bytes });
                            }

                            return;
                        }

                        LoadingVisibility = false;
                    }));
            }
        }

        private RelayCommand uploadImageCommand;
        public RelayCommand UploadImageCommand
        {
            get
            {
                return uploadImageCommand ??
                    (uploadImageCommand = new RelayCommand(async obj =>
                    {
                        LoadLocalImagesCommand.Execute(null);
                        ImageListVisibility = true;
                        //OpenFileDialog dialog = new OpenFileDialog();
                        //dialog.Filter = "PNG|*.png|JPG|*.jpg";
                        //if (dialog.ShowDialog() == true)
                        //{
                        //    LoadingVisibility = true;
                        //    var filePath = dialog.FileName;

                        //    if (await apiClient.UploadImageAsync(filePath))
                        //    {
                        //        LoadingVisibility = false;
                        //        LoadImagesCommand.Execute(null);
                        //    }
                        //    else
                        //    {
                        //        LoadingVisibility = false;
                        //    }

                        //}
                    }));
            }
        }

        private RelayCommand removeImageCommand;
        public RelayCommand RemoveImageCommand
        {
            get
            {
                return removeImageCommand ??
                    (removeImageCommand = new RelayCommand(async obj =>
                    {
                        int id = (int)obj;

                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.RemovePromotion, new { Id = id});

                        if (result.IsSuccessStatusCode)
                        {
                            LoadingVisibility = false;

                            LoadImagesCommand.Execute(null);
                        }

                        LoadingVisibility = false;
                    }));
            }
        }

        private RelayCommand confirmAddingImageCommand;
        public RelayCommand ConfirmAddingImageCommand
        {
            get
            {
                return confirmAddingImageCommand ??
                    (confirmAddingImageCommand = new RelayCommand(async obj =>
                    {
                        var path = obj as string;
                        if (path is null) return;
                        var result = await apiClient.SendPostAsync(ApiStrings.AddPromotion, new Promotion { ImagePath = path });
                        if (result.IsSuccessStatusCode)
                        {
                            LoadImagesCommand.Execute(null);
                        }
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
