using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.Web;
using System.Net;

namespace SmartLock_Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        private string url = "https://169.226.236.162:8000/";
        public static Label pLabel;
        //creating http client
        private static HttpClient Client;
        //this par is nessasary to allow for self assigned https certificates
        bool isLocked = true;
        //IBluetoothLE ble;
        public Page1()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Client = new HttpClient(clientHandler);

            InitializeComponent();
        }
        private void ButtonClick(object sender, EventArgs e)
        {
            if (isLocked)
            {
                LockButton.Source = "Unlocked.png";
                var converter = new ColorTypeConverter();
                LockButtonFrame.BackgroundColor = (Color)converter.ConvertFromInvariantString("Color.Red");
                isLocked = false;
                String responseString;
                //for http requests instead of https see android assembly info
                HttpResponseMessage response = new HttpResponseMessage();
                response = Client.GetAsync(url).Result;
                //getting uuid for security
                var uuid = response.Content.ReadAsStringAsync().Result;
                String temp = "uuid~" + uuid + "Qrequest~getCapture_27-04-22-00-47-47.png";
                
                var values = new Dictionary<string, string>
                {
                    { "",temp }
                };
                /*
                var values = new Dictionary<string, string>
                {
                    { "","getCapture_26-04-22-23-08-24.png" }
                };
                */
                var data = new FormUrlEncodedContent(values);
                response = Client.PostAsync(url, data).Result;
                responseString = response.Content.ReadAsStringAsync().Result;
                ResponseText.Text = responseString;
                //LockButton.Source = response.Content.ReadAsStringAsync().Result;
                //ResponseText.Text = responseString;
            }
            else
            {
                LockButton.Source = "Locked.png";
                var converter = new ColorTypeConverter();
                LockButtonFrame.BackgroundColor = (Color)converter.ConvertFromInvariantString("Color.Green");
                isLocked = true;
                String responseString;
                //for http requests instead of https see android assembly info
                //HttpResponseMessage response = new HttpResponseMessage();
                //response = Client.GetAsync(url).Result;
                //responseString = response.Content.ReadAsStringAsync().Result;
                //var values = new Dictionary<string, string>
                //{
                    //{ "", "lock" }
                //};
               // var data = new FormUrlEncodedContent(values);
                //response = Client.PostAsync(url, data).Result;
                //ResponseText.Text = responseString;
            }
    }
        private async void CameraClick(object sender, EventArgs e)
        {
            await DisplayAlert("Picture taken", "The camera has taken a picture!", "OK");
        }
    }

}