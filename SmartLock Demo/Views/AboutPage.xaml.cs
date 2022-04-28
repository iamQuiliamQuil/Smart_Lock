using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace SmartLock_Demo.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private async void CameraCapture(object sender, EventArgs e)
        {
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
    }

}