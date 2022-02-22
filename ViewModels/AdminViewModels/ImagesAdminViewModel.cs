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
    class ImagesAdminViewModel : INotifyPropertyChanged
    {
        private ApiClient apiClient = new ApiClient();

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

        private RelayCommand uploadImageCommand;
        public RelayCommand UploadImageCommand
        {
            get
            {
                return uploadImageCommand ??
                    (uploadImageCommand = new RelayCommand(async obj =>
                    {
                        OpenFileDialog dialog = new OpenFileDialog();
                        dialog.Filter  = "Images (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                        if (dialog.ShowDialog() == true)
                        {
                            LoadingVisibility = true;
                            var filePath = dialog.FileName;

                            if (await apiClient.UploadImageAsync(filePath))
                            {
                                LoadingVisibility = false;
                                LoadImagesCommand.Execute(null);
                            }
                            else
                            {
                                LoadingVisibility = false;
                            }
                            
                        }
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
                        string name = obj as string;

                        if (name is null) return;

                        LoadingVisibility = true;

                        var result = await apiClient.SendPostAsync(ApiStrings.RemoveImage, new { Name = name });

                        if (result.IsSuccessStatusCode)
                        {
                            LoadingVisibility = false;

                            LoadImagesCommand.Execute(null);
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
