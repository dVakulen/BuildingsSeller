#region Using Directives

using System.Windows;

using DragDropPhoneApp.ViewModel;

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

#endregion
namespace DragDropPhoneApp.IoC
{
  

    public class ViewModelLocator
    {
        #region Constructors and Destructors

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
        }

        #endregion

        #region Public Properties

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        #endregion

        #region Public Methods and Operators

        public static void Cleanup()
        {
            var viewModelLocator = (ViewModelLocator)Application.Current.Resources["Locator"];
            viewModelLocator.MainViewModel.Cleanup();

            Messenger.Reset();
        }

        #endregion
    }
}