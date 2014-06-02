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

    public partial class RealtyDetailsPage : PhoneApplicationPage
    {
        private MainViewModel dataContext; 
        public RealtyDetailsPage()
        {
            InitializeComponent();
            dataContext = App.DataContext;
            DataContext = App.DataContext;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new Uri("/Maps.xaml", UriKind.Relative));

        }
    }
}