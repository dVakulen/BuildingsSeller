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
    using DragDropPhoneApp.ViewModel;

    public partial class RealtyList : PhoneApplicationPage
    {
        private MainViewModel dataContext;

        public RealtyList()
        {
            InitializeComponent(); 
            dataContext = App.DataContext;
            DataContext = App.DataContext;
        }
    }
}