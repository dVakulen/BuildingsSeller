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

    using DragDropPhoneApp.ViewModel;

    public partial class AllImagesPage : PhoneApplicationPage
    {
        #region Fields

        private MainViewModel dataContext;

        #endregion

        #region Constructors and Destructors

        public AllImagesPage()
        {
            this.InitializeComponent();
            this.DataContext = App.DataContext;
            this.dataContext = App.DataContext;
        }

        #endregion

        #region Methods

        private void ImagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is LongListSelector)
            {
                var sendr = sender as LongListSelector;
                if (sendr.SelectedItem == null)
                {
                    return;
                }

                if (sendr.SelectedItem is Photo)
                {
                  this.dataContext.CurrentRealty.PictureSource = (sendr.SelectedItem as Photo).Image;
                 
                    sendr.SelectedItem = null;
                    this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
                }
            }
        }

        #endregion
    }
}