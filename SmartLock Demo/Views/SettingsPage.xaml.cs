using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartLock_Demo.Views
{
    public partial class SettingsPage : ContentPage
    {
        private EntryCell registerCell;

        public SettingsPage()
        {
            InitializeComponent();
            PhoneRegister.IsEnabled = false;
            registerCell = PhoneRegister;
        }

        private async void TextSwitch(object sender, EventArgs e)
        {
            bool wantsText = TextBool.On;
            switch (wantsText)
            {
                case true:
                    Preferences.Set("wantsText", true);
                    PhoneRegister.IsEnabled = true;
                    PhoneRegister.Placeholder = "Your Phone Number Here";
                    await DisplayAlert("SMS enabled", "SMS notifications have been enabled. Please update your phone number", "OK");
                    //TableSection.Add(registerCell);
                    break;
                default:
                    Preferences.Set("wantsText", false);
                    Preferences.Remove("phoneNumber");
                    PhoneRegister.IsEnabled = false;
                    PhoneRegister.Placeholder = "SMS disabled";
                    PhoneRegister.Text = "";
                    await DisplayAlert("SMS disabled", "Understood, SMS notifications disabled and phone number has been cleared from application preferences", "OK");
                    //TableSection.Remove(PhoneRegister);
                    break;
            }
            //await DisplayAlert("Switch changed", "State currently " + wantsText, "OK"); Debugging
        }
        private async void UpdatePhone(object sender, EventArgs e)
        {
            string phoneNumber = PhoneRegister.Text;
            bool valid = true;
            //Verify that this is a phone number (all numbers)
            for(int i = 0; i < phoneNumber.Length; i++)
            {
                if (phoneNumber[i] < 48 || phoneNumber[i] > 57)
                {
                    valid = false;
                }
            }
            
            if(!valid)
            {
                await DisplayAlert("Incorrect Format", "Incorrect phone number format, please input with no" +
                    "hypens, dashes, or spaces. Include dialing code as well", "OK");
            } else
            {
                Preferences.Set("phoneNumer", phoneNumber);
                await DisplayAlert("Phone Number Set!", "Phone number successfully set to: " + phoneNumber, "OK");
            }
        }
        private async void SetIP(object sender, EventArgs e)
        {
            string ipAddress = IPADDR.Text;
            if(ipAddress == null)
            {
                return;
            }

            char ch = '.';
            char col = ':';
            int dots = ipAddress.Count(f => (f == ch)); //Counts how many periods in input
            int portCount = ipAddress.Count(f => (f == col)); //Colons in the input
            if (dots == 3 && portCount == 1) //Checks if the IP Address is in the correct format
            {
                Preferences.Set("ipAddr", ipAddress);
                await DisplayAlert("IP Address set", "IP Address and Port successfully set to: " + ipAddress, "OK");
            }
            else
            {
                await DisplayAlert("Incorrect format", "IP Address consist of four numbers separated by periods and then the port, separated by a colon", "Retry");
            }
        }
    }
}