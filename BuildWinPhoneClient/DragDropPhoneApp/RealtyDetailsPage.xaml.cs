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
    using System.Windows.Media.Imaging;

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
            if (this.dataContext.CurrentRealty.Picture == null || this.dataContext.CurrentRealty.Picture.Length ==0)
            {
                BitmapImage img = new BitmapImage();
                img.SetSource(
                    Application.GetResourceStream(
                        new Uri(@"Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative)).Stream);
                this.ImageRealt.Source = img;
            }
            if (dataContext.isInRealtyCreating)
            {
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

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

            if (this.dataContext.isInRealtyCreating && dataContext.CurrentRealty.Address != string.Empty && dataContext.CurrentRealty.Named != string.Empty)
            {
                dataContext.CurrentRealty.Created = DateTime.Now;
                ApiService<Realty>.SendPost(dataContext.CurrentRealty);
                MessageBox.Show("Realty created successfully");
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.dataContext.isInRealtyCreating = false;
            this.NavigationService.Navigate(new Uri("/RealtyList.xaml", UriKind.Relative));
        }
    }
}