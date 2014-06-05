
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace DragDropPhoneApp.ViewModel
{
    using Build.DataLayer.Model;
    using Build.DataLayer.Repository;

    public class ViewModelLocator
    {

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<Repository<CurrentUser>>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();

            }
        }
        public  Repository<CurrentUser> UserRepository
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Repository<CurrentUser>>();

            }
        }
    }
}