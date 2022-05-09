using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartLock_Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPageDetail : ContentPage
    {
        private string url = "";
        //creating http client
        private static HttpClient Client;
        String CurrentFile;//used to keep the current image being showns name
        public GalleryPageDetail(String FileName)
        {
            InitializeComponent();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Client = new HttpClient(clientHandler);
            urlBuilder(Preferences.Get("ipAddr", "1.1.1.1"));

            CurrentFile = FileName;
            var response = Client.GetAsync(url).Result;
            //getting uuid for security
            var uuid = response.Content.ReadAsStringAsync().Result;
            //image1
            var command = commandBuilder(uuid, "getCapture_" + FileName);
            var values = new Dictionary<string, string>
                {
                    { "",command }
                };
            var data = new FormUrlEncodedContent(values);
            response = Client.PostAsync(url, data).Result;
            var stream = response.Content.ReadAsByteArrayAsync().Result;
            //ResponseText.Text = responseString;
            ImageCell.Source = ImageSource.FromStream(() =>
            {
                return new MemoryStream(stream);
            });
        }
        private void DeleteButtonClick(object sender, EventArgs e)
        {
            var response = Client.GetAsync(url).Result;
            //getting uuid for security
            var uuid = response.Content.ReadAsStringAsync().Result;
            //delete the image
            var command = commandBuilder(uuid, "deleteCapture_" + CurrentFile);
            var values = new Dictionary<string, string>
                {
                    { "",command }
                };
            var data = new FormUrlEncodedContent(values);
            response = Client.PostAsync(url, data).Result;
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
        }
        public void urlBuilder(string ip)
        {
            url = "https://" + ip + "/";
        }
        public string commandBuilder(string uuid, string command)
        {
            return "uuid~" + uuid + "Qrequest~" + command;
        }
    }
}