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
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Build.DataLayer.Interfaces;
    using Build.DataLayer.Model;
    using Build.DataLayer.Repository;

    using BuildSeller.Core.Model;

    using DragDropPhoneApp.ApiConsumer;
    using DragDropPhoneApp.Helpers;
    using DragDropPhoneApp.ViewModel;

    public partial class LoginPage : PhoneApplicationPage
    {
        private IRepository<CurrentUser> userRepository = App.UserRepository;

        public LoginPage()
        {
            this.InitializeComponent();
            DataContext = App.DataContext;
        }

        private void Login_Tap(object sender, GestureEventArgs e)
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
                App.DataContext.IsLoading = true;
                ApiService<Users>.Login(this.Login.Text, this.Password.Text);
                userRepository.Insert(new CurrentUser
                                          {
                                              Login = this.Login.Text,
                                              Password = this.Password.Text,
                                              LoginTime = DateTime.Now
                                          });
                userRepository.SubmitChanges();
            }
            else
            {
                MessageBox.Show("Username and password fields cannot be empty");
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            Indicator.setLoadingIndicator(this, "Loggin in");
            var user = this.userRepository.GetAll().OrderByDescending(b => b.LoginTime).FirstOrDefault();

            if (user == null)
                return;
            this.Login.Text = user.Login;
            this.Password.Text = user.Password;
            Task.Factory.StartNew(
                () =>
                {
                    var users = userRepository.GetAll().OrderByDescending(b => b.LoginTime).Skip(1);
                    userRepository.DeleteAll(users);
                });
        }

    }
}