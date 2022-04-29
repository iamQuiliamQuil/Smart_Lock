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
            urlBuilder(Preferences.Get("ipAddr", "1.1.1.1"));

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
            }
            else
            {
                //switch images
                LockButton.Source = "Locked.png";
                var converter = new ColorTypeConverter();
                LockButtonFrame.BackgroundColor = (Color)converter.ConvertFromInvariantString("Color.Green");
                isLocked = true;
                //get uuid it is unique each time pi is restarted
                var response = Client.GetAsync(url).Result;
                var uuid = response.Content.ReadAsStringAsync().Result;
                //create command
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