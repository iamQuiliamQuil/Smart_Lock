using System;
using System.Collections.Generic;
using SmartLock_Demo.ViewModels;
using SmartLock_Demo.Views;
using Xamarin.Forms;

namespace SmartLock_Demo
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
