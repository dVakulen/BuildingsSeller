namespace DragDropPhoneApp.ViewModel
{
    #region Using Directives

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using Build.DataLayer.Model;

    using BuildSeller.Core.Model;

    using DragDropPhoneApp.Helpers;

    using GalaSoft.MvvmLight;

    #endregion

    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields

        public Users CurrentUser;

        public List<Realty> realtys;

        private bool isLoading;

        #endregion

        #region Constructors and Destructors

        public MainViewModel()
        {
            this.Realtys = new List<Realty>();
            this.CurrentUser = new Users();
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public Realty CurrentRealty { get; set; }

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
                var cards = this.Realtys.Where(v => !v.IsForRent);
                return AlphaKeyGroup<Realty>.CreateGroups(cards, s => s.Named, true);
            }
        }

        public bool IsAuthorized { get; set; }

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

        #endregion

        #region Public Methods and Operators

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}