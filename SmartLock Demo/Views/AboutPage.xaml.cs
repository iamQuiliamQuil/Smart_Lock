using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Essentials;
using System.Collections.Generic;

namespace SmartLock_Demo.Views
{
    public partial class AboutPage : ContentPage
    {
        Page1 functions = new Page1();
        private string url = "";
        public static Label pLabel;
        //creating http client
        private static HttpClient Client;
        public AboutPage()
        {
            InitializeComponent();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Client = new HttpClient(clientHandler);
            urlBuilder(Preferences.Get("ipAddr", "1.1.1.1"));

        }
        public void urlBuilder(string ip)
        {
            url = "https://" + ip + "/";
        }
        public string commandBuilder(string uuid, string command)
        {
            return "uuid~" + uuid + "Qrequest~" + command;
        }

        private async void CaptureButtonClick(object sender, EventArgs e)
        {
            try
            {
                //capture
                var response = Client.GetAsync(url).Result;
                //getting uuid for security
                var uuid = response.Content.ReadAsStringAsync().Result;
                var values = new Dictionary<string, string>
            {
                { "","uuid~" + uuid + "Qrequest~capture_100x100_png" }
            };
                var data = new FormUrlEncodedContent(values);
                response = Client.PostAsync(url, data).Result;

                var FileName = response.Content.ReadAsStringAsync().Result;
                var command = commandBuilder(uuid, "getCapture_" + FileName);
                values = new Dictionary<string, string>
                {
                    { "",command }
                };
                data = new FormUrlEncodedContent(values);
                response = Client.PostAsync(url, data).Result;
                var stream = response.Content.ReadAsByteArrayAsync().Result;
                //ResponseText.Text = responseString;
                image1.Source = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(stream);
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Couldn't connect to lock", "OK");
            }

        }
    }


}