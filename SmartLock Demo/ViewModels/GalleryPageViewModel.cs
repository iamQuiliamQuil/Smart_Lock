using SmartLock_Demo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Globalization;

namespace SmartLock_Demo.ViewModels
{

    public class GalleryPageViewModel
    {
        public ObservableCollection<GalleryListModel> ImageList { get; set; }


        private string url = "";
        //creating http client
        private static HttpClient Client;
        //this is for the gallery page to refresh

        public GalleryPageViewModel()
        {

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Client = new HttpClient(clientHandler);
            urlBuilder(Preferences.Get("ipAddr", "1.1.1.1"));
            try
            {
                var response = Client.GetAsync(url).Result;
                //getting uuid for security
                var uuid = response.Content.ReadAsStringAsync().Result;
                var command = commandBuilder(uuid, "getPictureNames");
                var values = new Dictionary<string, string>
                {
                    { "",command }
                };
                var data = new FormUrlEncodedContent(values);
                response = Client.PostAsync(url, data).Result;
                var fileNames = response.Content.ReadAsStringAsync().Result;

                var stringArr = fileNames.Split('~');
                Array.Reverse(stringArr);
                ImageList = new ObservableCollection<GalleryListModel>();
                foreach (string str in stringArr)
                {
                    var temp = String.Copy(str);
                   
                    var time = temp.Split('-');//yyyyMMdd
                    var dateString = "20" + time[2] + time[1] + time[0];
                    //seting culture for date time format
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime dateObject = DateTime.ParseExact(dateString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    var displayFile = string.Format("{0:D}", dateObject);
                    ImageList.Add(new GalleryListModel { DisplayName = displayFile, FileName = str });
                }
            }
            catch (Exception ex)
            {

            }

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
