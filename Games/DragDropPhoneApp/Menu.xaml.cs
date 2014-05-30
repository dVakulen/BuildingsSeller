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
    using Windows.Devices.Geolocation;

    public partial class Menu : PhoneApplicationPage
    {
        public Menu()
        {
            InitializeComponent();
          //  Geolocator geolocator = new Geolocator();
           // geolocator.DesiredAccuracy = Windows.Devices.Geolocation.PositionAccuracy.High;
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Game.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Winners_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/WinnersRatingPage.xaml", UriKind.Relative));
        }
    }
}