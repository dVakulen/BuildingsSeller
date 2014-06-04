namespace DragDropPhoneApp
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;

    using Build.DataLayer.Model;

    using DragDropPhoneApp.ApiConsumer;
    using DragDropPhoneApp.Helpers;
    using DragDropPhoneApp.ViewModel;

    using Microsoft.Phone.Controls;

    #endregion

    public partial class RealtyList : PhoneApplicationPage
    {
        #region Fields

        private BackgroundWorker bWorker = new BackgroundWorker();

        private MainViewModel dataContext;

        #endregion

        #region Constructors and Destructors

        public RealtyList()
        {
            this.InitializeComponent();
            this.dataContext = App.DataContext;
            this.DataContext = App.DataContext;
            this.bWorker.WorkerReportsProgress = false;
            this.bWorker.WorkerSupportsCancellation = false;
            this.bWorker.DoWork += this.bw_DoWork;
            this.bWorker.RunWorkerCompleted += this.bw_RunWorkerCompleted;
            this.dataContext.isInRealtyCreating = false;
        }

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.RunWorker();
        }

        private void BlogsLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is LongListSelector)
            {
                var sendr = sender as LongListSelector;

                if (sendr.SelectedItem == null)
                {
                    return;
                }

                if (!(sendr.SelectedItem is Realty))
                {
                    return;
                }

                var realt = sendr.SelectedItem as Realty;
                this.dataContext.CurrentRealty = realt;
                this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Indicator.setLoadingIndicator(this, "Loading realties");
        }

        private void RunWorker()
        {
            if (this.bWorker.IsBusy != true)
            {
                this.bWorker.RunWorkerAsync();
            }
        }

        private async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
         
            ApiService<Realty>.GetRealties();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        #endregion
       
        private void AddNew_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.dataContext.CurrentRealty = new Realty();
            this.dataContext.isInRealtyCreating = true;
            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
       
        }

        private void StackPanel_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
          //  MessageBox.Show("sdd");

        }

        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ListPicker))
            {
                return;
            }

            if ((sender as ListPicker).SelectedIndex == 1)
            {
                this.dataContext.OrderBy = true;
            }
            else
            {
                this.dataContext.OrderBy = false;
            }
        }

        private void Add_new_Click(object sender, EventArgs e)
        {
            this.dataContext.CurrentRealty = new Realty();
            this.dataContext.isInRealtyCreating = true;
            this.NavigationService.Navigate(new Uri("/RealtyDetailsPage.xaml", UriKind.Relative));
        }
    }
}