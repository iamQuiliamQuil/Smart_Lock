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
using System.IO;
using Xamarin.Essentials;

namespace SmartLock_Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        private string url = "";
        public static Label pLabel;
        //creating http client
        private static HttpClient Client;
        //this par is nessasary to allow for self assigned https certificates
        bool isLocked = true;
        //IBluetoothLE ble;
        public Page1()
        {
            //creating httpClient with handler to allow ssl
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Client = new HttpClient(clientHandler);

            InitializeComponent();
        }
        //used to add ip from phone
        public void urlBuilder(string ip)
        {
            url = "https://" + ip + "/";
        }
        public string commandBuilder(string uuid, string command)
        {
            return "uuid~" + uuid + "Qrequest~" + command;
        }
        //lock/unlock button
        private void ButtonClick(object sender, EventArgs e)
        {
            if (isLocked)
            {
                LockButton.Source = "Unlocked.png";
                var converter = new ColorTypeConverter();
                LockButtonFrame.BackgroundColor = (Color)converter.ConvertFromInvariantString("Color.Red");
                isLocked = false;
                //for http requests instead of https see android assembly info
                var response = Client.GetAsync(url).Result;
                //getting uuid for security
                var uuid = response.Content.ReadAsStringAsync().Result;
                var command = commandBuilder(uuid, "unlock");
                //unlock
                var values = new Dictionary<string, string>
                {
                    { "",command }
                };
                var data = new FormUrlEncodedContent(values);
                response = Client.PostAsync(url, data).Result;
                /*
                //capture
                var values = new Dictionary<string, string>
                {
                    { "","uuid~" + uuid + "Qrequest~capture_100x100_png" }
                };
                var data = new FormUrlEncodedContent(values);
                response = Client.PostAsync(url, data).Result;
                responseString = response.Content.ReadAsStringAsync().Result;
                //get capture
                
                var values2 = new Dictionary<string, string>
                {
                    { "","uuid~" + uuid + "Qrequest~getCapture_" + responseString }
                };
                
                var data2 = new FormUrlEncodedContent(values2);
                response = Client.PostAsync(url, data2).Result;
                var stream = response.Content.ReadAsByteArrayAsync().Result;
                //ResponseText.Text = responseString;
                LockButton.Source = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(stream);
                });
                ResponseText.Text = responseString;
                */
            }
            else
            {
                LockButton.Source = "Locked.png";
                var converter = new ColorTypeConverter();
                LockButtonFrame.BackgroundColor = (Color)converter.ConvertFromInvariantString("Color.Green");
                isLocked = true;
                var response = Client.GetAsync(url).Result;
                //getting uuid for security
                var uuid = response.Content.ReadAsStringAsync().Result;
                var command = commandBuilder(uuid, "lock");
                //unlock
                var values = new Dictionary<string, string>
                {
                    { "",command }
                };
                var data = new FormUrlEncodedContent(values);
                response = Client.PostAsync(url, data).Result;
            }
    }
        private async void SetIP(object sender, EventArgs e)
        {
            bool valid = false; //Used to determine whether the prompt should be repeated, in event of invalid response
            do
            {
                string ipAddress = await DisplayPromptAsync("IP Address", "IP Address is currently " + Preferences.Get("ipAddr", "1.1.1.1") + " Please enter the IP Address");
                char ch = '.';

                int dots = ipAddress.Count(f => (f == ch)); //Counts how many periods in response

                if (dots == 3) //Checks if the IP Address is in the correct format
                {
                    valid = true;
                    await DisplayAlert("Successfully set IP", "The lock's IP Address has been successfully configured! (" + ipAddress + ")", "OK!");
                    Preferences.Set("ipAddr", ipAddress);
                    urlBuilder(ipAddress);
                }
                else
                {
                    await DisplayAlert("Incorrect format", "IP Address consist of four numbers separated by periods", "Retry");
                }
            } while (!valid);
        }

    }

}