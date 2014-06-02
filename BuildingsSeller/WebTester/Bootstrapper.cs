
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using BuildSeller.Infra;

namespace BuildSeller
{

    public class Bootstrapper
    {

        public static void Bootstrap()
        {

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory11(IoC.Container));
        }
    }
}
