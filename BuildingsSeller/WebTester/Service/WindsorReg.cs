
using BuildSeller.Core.Model;
using BuildSeller.Core.Repository;
using BuildSeller.Core.Service;
using BuildSeller.Data;
using BuildSeller.Infra;
using Castle.Facilities.Logging;

namespace BuildSeller.Service
{

    public static class WindsorReg
    {

        static WindsorReg()
        {

        }

        public static void Initialize()
        {
            WindsorRegistr.Register(typeof(IDbContextFactory), typeof(DbContextFactory));

            WindsorRegistr.Register(typeof(IRepo<Users>), typeof(Repo<Users>));

            WindsorRegistr.Register(typeof(IUserService), typeof(UserService));

            WindsorRegistr.Register(typeof(IRepo<BuildCategories>), typeof(Repo<BuildCategories>));
            WindsorRegistr.Register(typeof(IBuildCategoriesService), typeof(BuildCategoriesService));

            WindsorRegistr.Register(typeof(IRepo<Realty>), typeof(Repo<Realty>));
            WindsorRegistr.Register(typeof(IRealtyService), typeof(RealtyService));

            WindsorRegistr.Register(typeof(IRepo<Subscribe>), typeof(Repo<Subscribe>));
            WindsorRegistr.Register(typeof(ISubscribeService), typeof(SubscribeService));
            IoC.Container.AddFacility<LoggingFacility>(f => f.LogUsing(LoggerImplementation.NLog)
            .WithConfig("NLog.config"));
        }
    }
}
