using System.ComponentModel;
using SmartLock_Demo.ViewModels;
using Xamarin.Forms;

namespace SmartLock_Demo.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}