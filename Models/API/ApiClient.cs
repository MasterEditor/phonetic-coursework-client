using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Coursework_Client.Managers;
using System.IO;

namespace Coursework_Client.Models.API
{
    public class ApiClient
    {
        protected static string serverAddress = System.Configuration.ConfigurationManager.AppSettings["ServerAddress"];

        public async Task<HttpResponseMessage> SendPostAsync(string path, object obj, bool useAuthorization = false)
        {
            while (true)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(serverAddress);

                        if(useAuthorization && !string.IsNullOrEmpty(AccountManager.Token))
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccountManager.Token);
                        }

                        if(obj is null)
                        {
                            return await client.PostAsync(path, null);
                        }

                        var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                        return await client.PostAsync(path, content);
                    }
                }
                catch { }
            }
            
        }

        public async Task<byte[]> LoadImageAsync(string imageName)
        {
            for(int i = 0; i < 5; i++)
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        return await client.DownloadDataTaskAsync(serverAddress + "/Images/" + imageName);
                    }
                }
                catch { }
            }

            return null;
        }

        public async Task<bool> UploadImageAsync(string filePath)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var form = new MultipartFormDataContent())
                    {
                        using (var fs = File.OpenRead(filePath))
                        {
                            using (var streamContent = new StreamContent(fs))
                            {
                                using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
                                {
                                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                                    form.Add(fileContent, "image", Path.GetFileName(filePath));
                                    HttpResponseMessage response = await httpClient.PostAsync(serverAddress + ApiStrings.AddImage, form);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch { return false; }
        }

    }
}
