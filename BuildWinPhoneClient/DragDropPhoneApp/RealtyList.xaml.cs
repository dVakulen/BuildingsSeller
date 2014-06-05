namespace DragDropPhoneApp
{
    #region Using Directives

    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;

    using Build.DataLayer.Model;

    using DragDropPhoneApp.ApiConsumer;
    using DragDropPhoneApp.Helpers;
    using DragDropPhoneApp.ViewModel;

    using Microsoft.Phone.Controls;

    using GestureEventArgs = System.Windows.Input.GestureEventArgs;

    #endregion

    public partial class RealtyList : PhoneApplicationPage
    {
        #region Fields


        #endregion

        #region Constructors and Destructors

        public RealtyList()
        {
            this.InitializeComponent();
            this.DataContext = App.DataContext;
            App.DataContext.isInRealtyCreating = false;
        }

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApiService<Realty>.GetRealties();
        }

        private void AddNew_Tap(object sender, GestureEventArgs e)
        {
            App.DataContext.CurrentRealty = new Realty();
            App.DataContext.isInRealtyCreating = true;
            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
        }

        private void Add_new_Click(object sender, EventArgs e)
        {
            App.DataContext.CurrentRealty = new Realty();
            App.DataContext.isInRealtyCreating = true;
            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
        }

        private void BlogsLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sendr = sender as LongListSelector;
            if (sendr == null)
            {
                return;
            }

            if (sendr.SelectedItem == null)
            {
                return;
            }

            var realt = sendr.SelectedItem as Realty;
            if (realt == null)
            {
                return;
            }

            App.DataContext.CurrentRealty = realt;
            sendr.SelectedItem = null;
            App.DataContext.isInRealtyCreating = false;
            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Indicator.setLoadingIndicator(this, "Loading realties");
        }

        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sendr = sender as ListPicker;
            if (sendr == null)
            {
                return;
            }

            if (sendr.SelectedIndex == 1)
            {
                App.DataContext.OrderBy = true;
            }
            else
            {
                App.DataContext.OrderBy = false;
            }
        }
        private void Ascendng_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sendr = sender as ListPicker;
            if (sendr == null)
            {
                return;
            }

            if (sendr.SelectedIndex == 1)
            {
                App.DataContext.IsAscendingSorting = true;
            }
            else
            {
                App.DataContext.IsAscendingSorting = false;
            }
        }

        #endregion
    }
}