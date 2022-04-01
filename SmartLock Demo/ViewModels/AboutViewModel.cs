using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartLock_Demo.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "Camera";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }
    }
}