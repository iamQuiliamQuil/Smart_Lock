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
            switch (Preferences.Get("wantsText", false)) //Used to keep the Switchcell state between sessions
            {
                case true:
                    TextBool.On = true;
                    break;
                default:
                    TextBool.On = false;
                    break;
            }
        }
        
        //TextSwitch toggles the wantsText boolean kept in Preferences, called when the Switchcell TextBool is toggled
        private async void TextSwitch(object sender, EventArgs e)
        {
            bool wantsText = TextBool.On; //Checks if the switch is on or off, sets wantsText to the result
            switch (wantsText) //Switch statement based on whether the switch is on or off
            {
                case true:
                    Preferences.Set("wantsText", true);
                    PhoneRegister.IsEnabled = true; //Allows the Entrycell to receive input
                    PhoneRegister.Placeholder = "Your Phone Number Here";
                    if(Preferences.Get("phoneNumber", "").Equals("")) //Checks if the phone number hasn't been set to anything, if so it reminds the user to set their phone number
                    {
                        await DisplayAlert("SMS enabled", "SMS notifications have been enabled. Please update your phone number", "OK");

                    }
                    break;
                default:
                    Preferences.Set("wantsText", false);
                    Preferences.Remove("phoneNumber"); //Clears phone number from the app preferences, we need to be ethical
                    PhoneRegister.IsEnabled = false; //Makes it so the Entrycell can't receive input
                    PhoneRegister.Placeholder = "SMS disabled";
                    PhoneRegister.Text = "";
                    await DisplayAlert("SMS disabled", "Understood, SMS notifications disabled and phone number has been cleared from application preferences", "OK");
                    //TableSection.Remove(PhoneRegister);
                    break;
            }
            //await DisplayAlert("Switch changed", "State currently " + wantsText, "OK"); Debugging
        }

        //UpdatePhone is used to verify the format of the phone number input, if valid it updates the phoneNumber preference
        private async void UpdatePhone(object sender, EventArgs e)
        {
            string phoneNumber = PhoneRegister.Text;
            bool valid = true; //Assume it's true. If one of the conditions is fulfilled, then it isn't a true phone number
            //Verify that this is a phone number (all numbers)
            for(int i = 0; i < phoneNumber.Length; i++)
            {
                if (phoneNumber[i] < 48 || phoneNumber[i] > 57) //Checks ASCII values, only digits
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

        //SetIP is used to set the IP address in the application preferences, called when the IPADDR Entrycell receives input
        private async void SetIP(object sender, EventArgs e)
        {
            string ipAddress = IPADDR.Text;
            if(ipAddress == null) //If you were to open the keyboard and not type anything, the app would crash if not for this
            {
                return;
            }

            //Dot and colon used to separate IP Address and Port
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