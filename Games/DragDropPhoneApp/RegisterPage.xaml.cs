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

    public partial class RegisterPage : PhoneApplicationPage
    {
        public RegisterPage()
        {
            InitializeComponent(); 
            
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


            });
        }
    }
}