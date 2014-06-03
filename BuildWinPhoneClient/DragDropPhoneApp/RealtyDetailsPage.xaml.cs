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
    using Build.DataLayer.Model;

    using DragDropPhoneApp.ApiConsumer;
    using DragDropPhoneApp.ViewModel;

    public partial class RealtyDetailsPage : PhoneApplicationPage
    {
        private MainViewModel dataContext; 
        public RealtyDetailsPage()
        {
            InitializeComponent();
            dataContext = App.DataContext;
            DataContext = App.DataContext;
            if (dataContext.isInRealtyCreating)
            {
                this.Submit.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new Uri("/Maps.xaml", UriKind.Relative));

        }

        private void Submit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (dataContext.CurrentRealty.Address != string.Empty && dataContext.CurrentRealty.Named != string.Empty)
            {
                dataContext.CurrentRealty.Created = DateTime.Now;
                ApiService<Realty>.SendPost(dataContext.CurrentRealty);
            }
        }
    }
}