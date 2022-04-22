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
        private string url = "http://169.226.244.146:8000/";
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
                responseString = response.Content.ReadAsStringAsync().Result;
                var values = new Dictionary<string, string>
                {
                    { "", "unlock" }
                };
                var data = new FormUrlEncodedContent(values);
                response = Client.PostAsync(url, data).Result;
                ResponseText.Text = responseString;
            }
            else
            {
                LockButton.Source = "Locked.png";
                var converter = new ColorTypeConverter();
                LockButtonFrame.BackgroundColor = (Color)converter.ConvertFromInvariantString("Color.Green");
                isLocked = true;
                String responseString;
                //for http requests instead of https see android assembly info
                HttpResponseMessage response = new HttpResponseMessage();
                response = Client.GetAsync(url).Result;
                responseString = response.Content.ReadAsStringAsync().Result;
                var values = new Dictionary<string, string>
                {
                    { "", "lock" }
                };
                var data = new FormUrlEncodedContent(values);
                response = Client.PostAsync(url, data).Result;
                ResponseText.Text = responseString;
            }
        }
    }

}