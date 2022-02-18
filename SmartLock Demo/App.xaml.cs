using System;
using SmartLock_Demo.Services;
using SmartLock_Demo.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartLock_Demo
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
