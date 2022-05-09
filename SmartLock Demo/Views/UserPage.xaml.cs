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
        private string uuid = "";
        //creating http client
        private static HttpClient Client;
        //this par is nessasary to allow for self assigned https certificates
        bool isLocked = true;
        //IBluetoothLE ble;
        public Page1()
        {
            InitializeComponent();
                //creating httpClient with handler to allow ssl
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Client = new HttpClient(clientHandler);
            urlBuilder(Preferences.Get("ipAddr", "1.1.1.1"));
            try
            {
                var response = Client.GetAsync(url).Result;
                //getting uuid for security
                uuid = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Couldn't connect to lock", "OK");
            }


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
            /*
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("alertTone.mp3");
            player.Play();*/
            try { 
                if (isLocked)
                {
                    LockButton.Source = "Unlocked.png";
                    var converter = new ColorTypeConverter();
                    LockButtonFrame.BackgroundColor = (Color)converter.ConvertFromInvariantString("Color.Red");
                    isLocked = false;
                    //for http requests instead of https see android assembly info
                    var command = commandBuilder(uuid, "unlock");
                    //unlock
                    var values = new Dictionary<string, string>
                    {
                        { "",command }
                    };
                    var data = new FormUrlEncodedContent(values);
                    var response = Client.PostAsync(url, data).Result;
                }
                else
                {
                    LockButton.Source = "Locked.png";
                    var converter = new ColorTypeConverter();
                    LockButtonFrame.BackgroundColor = (Color)converter.ConvertFromInvariantString("Color.Green");
                    isLocked = true;
                    var command = commandBuilder(uuid, "lock");
                    //unlock
                    var values = new Dictionary<string, string>
                    {
                        { "",command }
                    };
                    var data = new FormUrlEncodedContent(values);
                    var response = Client.PostAsync(url, data).Result;
                }
            }
            catch(Exception ex)
            {
                DisplayAlert("Error", "Couldn't connect to lock", "OK");
            }
    }
        private void StatusButtonClick(object sender, EventArgs e)
        {
            try
            {
                //checking state of the lock
                var command = commandBuilder(uuid, "getState");
                //unlock
                var values = new Dictionary<string, string>
                    {
                        { "",command }
                    };
                var data = new FormUrlEncodedContent(values);
                var response = Client.PostAsync(url, data).Result;

                var currentState = response.Content.ReadAsStringAsync().Result;
                
                if (currentState.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    statusButton.Text = currentState;
                    LockButton.Source = "Unlocked.png";
                    var converter = new ColorTypeConverter();
                    LockButtonFrame.BackgroundColor = (Color)converter.ConvertFromInvariantString("Color.Red");
                    isLocked = false;
                }
            }
            catch(Exception ex)
            {
                DisplayAlert("Error", "Couldn't connect to lock", "OK");
            }
        }

        //Initially used for debugging and testing purposes, used to set the IP Address from the home screen (before we had the Settings page)
        private async void SetIP(object sender, EventArgs e)
        {
            bool valid = false; //Used to determine whether the prompt should be repeated, in event of invalid response
            do
            {
                string ipAddress = await DisplayPromptAsync("IP Address", "IP Address is currently " + Preferences.Get("ipAddr", "1.1.1.1") + "\nPlease enter the IP Address");
                if (ipAddress == null) //If the keyboard were to be brought up without typing anything and returning the empty input, the app would crash
                {
                    break;
                }
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
            } while (!valid); //Loops while the input isn't considered valid
        }

    }

}