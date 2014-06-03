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
        private MainViewModel dataContext;
        public RegisterPage()
        {
            InitializeComponent();
            this.dataContext = App.DataContext;
            this.DataContext = App.DataContext;
            /*
 * 
            ApiService<Users>.SendPost(new Users
            {
                Phone = "Asd",
                Activated = false,
                Adress = "dsa",
                Comments = "Asdcdf",
                Email = "Asd",
                FirstName = "dsccd",
                LastName = "asdfcvf",
                Password = "Asdddddd",
                Patronymic = "scasca",
                RegisterDateTime = DateTime.Now,
                Login = "Addddddddd",
                Banned = true,
                Dislikes = 1,
                Likes = 2,
                PaidSeller = true,
                PaidUser = true,
                Roles = new List<Role>(),
                UsersLiked = new List<UserInteraction>(),


            }, false);*/
        }

      

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            dataContext.CurrentUser.RegisterDateTime = DateTime.Now;
            dataContext.CurrentUser.PaidSeller = true;
            dataContext.CurrentUser.PaidUser = true;
          //  dataContext.CurrentUser.Login = this.Name.Text;
        //    dataContext.CurrentUser.Password = this.Password.Text;
            dataContext.CurrentUser.Activated = true;
            ApiService<Users>.SendPost(dataContext.CurrentUser, false);
            MessageBox.Show("Registration successfull");
            this.NavigationService.Navigate(new Uri("/RealtyList.xaml", UriKind.Relative));
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