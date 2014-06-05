using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace DragDropPhoneApp
{
    using BuildSeller.Core.Model;

    using DragDropPhoneApp.ApiConsumer;
    using DragDropPhoneApp.ViewModel;

    public partial class RegisterPage : PhoneApplicationPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            this.DataContext = App.DataContext;
           
        }

      

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (string.IsNullOrEmpty(App.DataContext.CurrentUser.Login)
                || string.IsNullOrEmpty(App.DataContext.CurrentUser.Password))
            {
                MessageBox.Show("Logind and password cannot be empty");
                return;
            }
            App.DataContext.CurrentUser.RegisterDateTime = DateTime.Now;
            App.DataContext.CurrentUser.PaidSeller = true;
            App.DataContext.CurrentUser.PaidUser = true;
            App.DataContext.CurrentUser.Activated = true;
            ApiService<Users>.SendPost(App.DataContext.CurrentUser, false);
            MessageBox.Show("Registration successfull");
            this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        private void Name_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (sender is TextBox)
            {
                (sender as TextBox).SelectAll();
            }
        }
    }
}