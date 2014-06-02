﻿using System;
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
    using System.ComponentModel;
    using System.Diagnostics;

    using Build.DataLayer.Model;

    using BuildSeller.Core.Model;

    using DragDropPhoneApp.ApiConsumer;
    using DragDropPhoneApp.Helpers;
    using DragDropPhoneApp.ViewModel;

    public partial class RealtyList : PhoneApplicationPage
    {
        private MainViewModel dataContext; private BackgroundWorker bWorker = new BackgroundWorker();

        private async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    this.dataContext.IsLoading = true;
                //    this.dataContext.Cards = DataService.GetCards().Result;
                //    this.dataContext.photos = DataService.GetImages().Result;
                });
            ApiService<Realty>.GetRealties();
        }
        private void RunWorker()
        {
            if (this.bWorker.IsBusy != true)
            {
                this.bWorker.RunWorkerAsync();
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.RunWorker();

        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                //    this.dataContext.IsLoading = false;
                   
                });
        }
        public RealtyList()
        {
            InitializeComponent(); 
            dataContext = App.DataContext;
            DataContext = App.DataContext;
            this.bWorker.WorkerReportsProgress = false;
            this.bWorker.WorkerSupportsCancellation = false;
            this.bWorker.DoWork += this.bw_DoWork;
            this.bWorker.RunWorkerCompleted += this.bw_RunWorkerCompleted;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            Indicator.setLoadingIndicator(this, "Loading realties");
            dataContext.IsLoading = true;
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
                var realt = sender as Realty;
                dataContext.CurrentRealty = realt;
                this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }

        }
    }
}