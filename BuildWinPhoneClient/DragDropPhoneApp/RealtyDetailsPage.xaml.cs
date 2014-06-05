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
        public RealtyDetailsPage()
        {
            InitializeComponent();
            DataContext = App.DataContext;
            if (App.DataContext.CurrentRealty.Picture == null || App.DataContext.CurrentRealty.Picture.Length ==0)
            {
                BitmapImage img = new BitmapImage();
                img.SetSource(
                    Application.GetResourceStream(
                        new Uri(@"Assets/FlipCycleTileMedium.png", UriKind.Relative)).Stream);
                this.ImageRealt.Source = img;
            }
            else
            {
              this.ImageRealt.Source = App.DataContext.CurrentRealty.PictureSource;
            
            }
            if (!App.DataContext.isInRealtyCreating)
            {
                this.ApplicationBar.Mode = ApplicationBarMode.Minimized;
                this.ApplicationBar.IsVisible = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new Uri("/Maps.xaml", UriKind.Relative));

        }
        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!App.DataContext.isInRealtyCreating) return;

            this.NavigationService.Navigate(new Uri("/AllImagesPage.xaml", UriKind.Relative));

        }

        private void Submit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.DataContext.CurrentRealty.Address != string.Empty && App.DataContext.CurrentRealty.Named != string.Empty)
            {
                App.DataContext.CurrentRealty.Created = DateTime.Now;
                ApiService<Realty>.SendPost(App.DataContext.CurrentRealty);
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

            if (!(App.DataContext.isInRealtyCreating && App.DataContext.CurrentRealty.Address != string.Empty && App.DataContext.CurrentRealty.Named != string.Empty))
            {
                MessageBox.Show("You must fill all fields");
              return;
            }
            if (App.DataContext.CurrentRealty.Picture == null || App.DataContext.CurrentRealty.Picture.Length == 0)
            {
                MessageBox.Show("You must fill all fields");
                return;
            }
            if (App.DataContext.CurrentRealty.Description == string.Empty || App.DataContext.CurrentRealty.Square == 0 )
            {
                MessageBox.Show("You must fill all fields");
                return;
            }
            App.DataContext.CurrentRealty.Created = DateTime.Now;
            ApiService<Realty>.SendPost(App.DataContext.CurrentRealty);
            MessageBox.Show("Realty created successfully");
             App.DataContext.CurrentRealty = new Realty();
            Cancel_Click(null, null);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            App.DataContext.isInRealtyCreating = false;
            this.NavigationService.Navigate(new Uri("/RealtyList.xaml", UriKind.Relative));
        }
    }
}