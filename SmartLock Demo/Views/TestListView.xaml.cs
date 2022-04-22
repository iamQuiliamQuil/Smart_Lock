using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartLock_Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewPage1 : ContentPage
    {
        public ObservableCollection<string> PictureList { get; set; }

        public ListViewPage1()
        {
            InitializeComponent();

            //PictureList should hold pictures. Determine how it can do this, save filenames for now
            PictureList = new ObservableCollection<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5"
            };

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
                this.PictureList.Add("picture");
            }
            MyListView.ItemsSource = PictureList;
        }

        //Should send a request to the Pi for details about the picture when a picture is selected
        private async void GetDetails(object sender, EventArgs e)
        {
            var list = ((ListView)sender);
            await DisplayAlert("Picture Details", list.SelectedItem.ToString(), "OK");
        }
    }
}
