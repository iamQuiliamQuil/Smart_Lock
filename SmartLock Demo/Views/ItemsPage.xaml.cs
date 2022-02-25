using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLock_Demo.Models;
using SmartLock_Demo.ViewModels;
using SmartLock_Demo.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace SmartLock_Demo.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }
        public class Lock
        { 
            public string Name 
            {
                get { return Name; } 
                set { Name = value; }
            }
            public bool LockStatus
            {
                get { return LockStatus; }
                set { LockStatus = value; }
            }
            public bool ConnectionStatus
            {
                get { return ConnectionStatus; }
                set { ConnectionStatus = value; }
            }
        }
        public ObservableCollection<Lock> LockList { get; set; }
        public void LockListViewModel()
        {
            LockList = new ObservableCollection<Lock>();
            Lock lock1 = new Lock();
            lock1.Name = "home";
            LockList.Add(lock1);
        }
    }
}