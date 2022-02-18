using System;
using System.Collections.Generic;
using System.ComponentModel;
using SmartLock_Demo.Models;
using SmartLock_Demo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartLock_Demo.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}