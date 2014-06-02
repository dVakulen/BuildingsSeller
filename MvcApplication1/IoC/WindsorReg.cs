// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindsorReg.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The windsor reg.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace BuildSeller.Service
{
    using System.Data.Entity.Infrastructure;

    using BuildSeller.Infra;

    using Castle.MicroKernel.Registration;

    using Core.Interface;
    using Core.Model;

    using DAL;

    /// <summary>
    /// The windsor reg.
    /// </summary>
    public static class WindsorReg
    {
        /// <summary>
        /// Initializes static members of the <see cref="WindsorReg"/> class.
        /// </summary>
        static WindsorReg()
        {
           
          
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public static void Initialize()
        {
             
            WindsorRegistr.Register(typeof(IDbContextFactory), typeof(DbContextFactory));
            WindsorRegistr.Register(typeof(IRepo<Users>), typeof(Repo<Users>));
            WindsorRegistr.Register(typeof(IRepo<Message>), typeof(Repo<Message>));
           // WindsorRegistr.RegisterAllFromAssemblies(AllTypes.FromAssembly(typeof(Users).Assembly).Pick().WithService.FirstInterface());
            //IoC.Container.Register(AllTypes.FromAssembly(typeof(Repo<>).Assembly).Pick().WithService.FirstInterface());

            WindsorRegistr.Register(typeof(ICrudService<Message>), typeof(CrudService<Message>));
          WindsorRegistr.Register(typeof(ICrudService<Users>), typeof(CrudService<Users>));
            //  WindsorRegistr.Register(typeof(IDbContextFactory<>), typeof(DbContextFactory));

        /*    WindsorRegistr.Register(typeof(IDbContextFactory), typeof(DbContextFactory));

            WindsorRegistr.Register(typeof(IRepo<Users>), typeof(Repo<Users>));


            WindsorRegistr.Register(typeof(IUserService), typeof(UserService));

            WindsorRegistr.Register(typeof(IRepo<BuildCategories>), typeof(Repo<BuildCategories>));
            WindsorRegistr.Register(typeof(IBuildCategoriesService), typeof(BuildCategoriesService));


            WindsorRegistr.Register(typeof(IRepo<Realty>), typeof(Repo<Realty>));
            WindsorRegistr.Register(typeof(IRealtyService), typeof(RealtyService));


            WindsorRegistr.Register(typeof(IRepo<Subscribe>), typeof(Repo<Subscribe>));
            WindsorRegistr.Register(typeof(ISubscribeService), typeof(SubscribeService));
            IoC.Container.AddFacility<LoggingFacility>(f => f.LogUsing(LoggerImplementation.NLog)
              .WithConfig("NLog.config"));*/
        }
    }
}