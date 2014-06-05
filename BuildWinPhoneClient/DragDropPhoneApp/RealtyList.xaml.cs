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

        private MainViewModel dataContext;

        #endregion

        #region Constructors and Destructors

        public RealtyList()
        {
            this.InitializeComponent();
            this.dataContext = App.DataContext;
            this.DataContext = App.DataContext;
            this.dataContext.isInRealtyCreating = false;
        }

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApiService<Realty>.GetRealties();
        }

        private void AddNew_Tap(object sender, GestureEventArgs e)
        {
            this.dataContext.CurrentRealty = new Realty();
            this.dataContext.isInRealtyCreating = true;
            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
        }

        private void Add_new_Click(object sender, EventArgs e)
        {
            this.dataContext.CurrentRealty = new Realty();
            this.dataContext.isInRealtyCreating = true;
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

            this.dataContext.CurrentRealty = realt;
            sendr.SelectedItem = null;
            this.dataContext.isInRealtyCreating = false;
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
                this.dataContext.OrderBy = true;
            }
            else
            {
                this.dataContext.OrderBy = false;
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
                this.dataContext.IsAscendingSorting = true;
            }
            else
            {
                this.dataContext.IsAscendingSorting = false;
            }
        }

        #endregion
    }
}