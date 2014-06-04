namespace DragDropPhoneApp
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    using BuildSeller.Core.Model;

    using DragDropPhoneApp.ApiConsumer;
    using DragDropPhoneApp.Helpers;
    using DragDropPhoneApp.Resources;
    using DragDropPhoneApp.ViewModel;

    using ImagesGrid.Services;

    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

    #endregion

    public partial class MainPage : PhoneApplicationPage
    {
        #region Constants

        #endregion

        #region Static Fields

        private static bool FirstTimeLoad = true;
        #endregion

        #region Fields

        private MainViewModel dataContext;



      
        #endregion

        #region Constructors and Destructors

        public MainPage()
        {
            this.InitializeComponent();
            dataContext = App.DataContext;
            DataContext = App.DataContext;
        }

        #endregion

        #region Methods
      

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Indicator.setLoadingIndicator(this,"asd");
            if (FirstTimeLoad)
            {
                FirstTimeLoad = false;
             //   this.NavigationService.Navigate(new Uri("/Maps.xaml", UriKind.Relative));
            }

            this.dataContext.photos = DataService.GetImages().Result;
        }

        private void PhoneApplicationPage_Tap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
        }
        

        #endregion

        private void Login_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }
       
        public void Redirect()
        {
            this.NavigationService.Navigate(new Uri("/Menu.xaml", UriKind.Relative));
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.Relative));
           
        }

        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}