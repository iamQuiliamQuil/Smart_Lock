using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartLock_Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        bool isLocked = true;
        public Page1()
        {
            InitializeComponent();
        } 
    private void ButtonClick(object sender, EventArgs e)
    {
            if(isLocked)
            {
                LockButton.Source = "padlock.png";
                isLocked = false;
            }
            else
            {
                LockButton.Source = "smartlock_logo.png";
                isLocked = true;
            }
    }
    }
   
}