using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartLock_Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewPage1 : ContentPage
    {
        private string url = "https://" + Preferences.Get("ipAddr", "1.1.1.1") + "/";
        public static Label pLabel;
        //creating http client
        private static HttpClient Client;
        //this par is nessasary to allow for self assigned https certificates

        public ObservableCollection<string> PictureList { get; set; }

        public ListViewPage1()
        {
            //creating httpClient with handler to allow ssl
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Client = new HttpClient(clientHandler);

            InitializeComponent();

            //PictureList should hold pictures. Determine how it can do this, save filenames for now
            PictureList = CreateList();

            MyListView.ItemsSource = PictureList;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Should enlarge images from the list when tapped, but there needs to be images in the list to do this
            /*var image = sender as Image;
            var viewCell = image.Parent.Parent as ViewCell;

            if(HeightRequest < 250)
            {
                image.HeightRequest = image.Height + 100;
                viewCell.ForceUpdateSize();
            }*/

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        /**
         * Function to update the PictureList in the constructor.
         * Should retrieve a set of pictures from the Raspberry Pi and store them
         * client-side to be shown to the user.
         * 
         * Need to determine how many pictures to show, and how to show more pictures if
         * the user wants to see more, as it seems like this is executed once at initialization
         **/
        private void Update(object sender, EventArgs e) {
            for (int i = 0; i < 5; i++) { //Add five more pictures to the list
                this.PictureList.Add("Item " + (this.PictureList.Count + 1));
            }
            MyListView.ItemsSource = PictureList;
        }

        //Should send a request to the Pi for details about the picture when a picture is selected
        private async void GetDetails(object sender, EventArgs e)
        {
            var list = ((ListView)sender);
            await DisplayAlert("Picture Details", list.SelectedItem.ToString(), "OK");
        }

        private async void SetDate(object sender, EventArgs e)
        {
            string[] unformatted = ((ListView)sender).SelectedItem.ToString().Split('-');
            string formatted = unformatted[0] + "/" + unformatted[1] + "/" + unformatted[2] + " "
                + unformatted[3] + ":" + unformatted[4] + ":" + unformatted[5];
            ((ListView)sender).SelectedItem = formatted;

            await DisplayAlert("Picture Details", ((ListView)sender).SelectedItem.ToString(), "OK");

        }

        private ObservableCollection<string> CreateList()
        {
            ObservableCollection<string> PictureList = new ObservableCollection<string> { };
            var response = Client.GetAsync(url).Result;
            //getting uuid for security
            var uuid = response.Content.ReadAsStringAsync().Result;
            var command = "uuid~" + uuid + "Qrequest~getPictureNames";
            var values = new Dictionary<string, string>
                {
                    { "", command }
                };

            var data = new FormUrlEncodedContent(values);
            response = Client.PostAsync(url, data).Result;
            string responseString = response.Content.ReadAsStringAsync().Result;

            string[] StringArray = (responseString).Split('~');
            for(int i = 0; i < StringArray.Length; i++)
            {
                PictureList.Add((string)StringArray[i]);
            }
            return PictureList;
        }
    }
}
