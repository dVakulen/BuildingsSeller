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
    using System.Windows.Input;

    using BuildSeller.Core.Model;

    using DragDropPhoneApp.ApiConsumer;
    using DragDropPhoneApp.Helpers;
    using DragDropPhoneApp.ViewModel;

    public partial class LoginPage : PhoneApplicationPage
    {
        private MainViewModel dataContext;
        public LoginPage()
        {
            this.InitializeComponent();
            dataContext = App.DataContext;
            DataContext = App.DataContext;
        }

        private void Login_Tap(object sender,GestureEventArgs e)
        {
            (sender as TextBox).SelectAll();
           
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoginBtn_Tap(object sender, GestureEventArgs e)
        {
            
            if (this.Login.Text != string.Empty && this.Password.Text != string.Empty)
            {
                this.dataContext.IsLoading = true;
                ApiService<Users>.Login(this.Login.Text, this.Password.Text);
            }
            else
            {
                MessageBox.Show("Username and password fields cannot be empty");
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            Indicator.setLoadingIndicator(this, "Loggin in");
        }

    }
}