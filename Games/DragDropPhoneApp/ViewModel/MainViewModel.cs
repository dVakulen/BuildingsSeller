using GalaSoft.MvvmLight;

namespace DragDropPhoneApp.ViewModel
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using Build.DataLayer.Model;

    using BuildSeller.Core.Model;

    using DragDropPhoneApp.Helpers;

    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
       
        public bool IsAuthorized { get; set; }
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                this.NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }  public List<Realty> realtys;
        public List<Realty> Realtys
        {
            get
            {
                return this.realtys;
            }

            set
            {
                this.realtys = value;

                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                    {
                       // this.CardsCount = value.Count;
                        this.NotifyPropertyChanged("GroupedRealtiesForRent");
                        this.NotifyPropertyChanged("GroupedRealtiesForSell");
                    });
            }
        }
        public List<AlphaKeyGroup<Realty>> GroupedRealtiesForRent
        {
            get
            {
                var cards = this.Realtys.Where(v => v.IsForRent); 
                return AlphaKeyGroup<Realty>.CreateGroups(cards, s => s.Named, true);
            }
        }
        public List<AlphaKeyGroup<Realty>> GroupedRealtiesForSell
        {
            get
            {
                var cards = this.Realtys.Where(v=>!v.IsForRent);
                return AlphaKeyGroup<Realty>.CreateGroups(cards, s => s.Named, true);
            }
        }
        public MainViewModel()
        {
            Realtys = new List<Realty>();

            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }
    }
}