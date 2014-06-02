// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionFactory.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The Sessionfact interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using Castle.MicroKernel.Registration;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System;
using System.Reflection;
using WebTester.Core.Model;
using WebTester.Core.Properties;
using WebTester.Infra;
using Environment = NHibernate.Cfg.Environment;

namespace WebTester.Data
{
    /// <summary>
    ///     The sessionfact.
    /// </summary>
    public class Sessionfact : ISessionfact
    {
        /// <summary>
        ///     The _current.
        /// </summary>
        [ThreadStatic]
        private static Sessionfact _current;

        /// <summary>
        ///     The _transaction.
        /// </summary>
        private ITransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sessionfact"/> class.
        /// </summary>
        /// <param name="sessionFactory">
        /// The session factory.
        /// </param>
        public Sessionfact(ISessionFactory sessionFactory)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Sessionfact" /> class.
        /// </summary>
        public Sessionfact()
        {
            IoC.Container.Register(
                Component.For<ISessionFactory>().UsingFactoryMethod(CreateNhSessionFactory).LifeStyle.Singleton);
            _sessionFactory = IoC.Resolve<ISessionFactory>();
        }

        /// <summary>
        ///     Gets or sets the _session factory.
        /// </summary>
        public static ISessionFactory _sessionFactory { get; set; }

        /// <summary>
        ///     Gets or sets the current.
        /// </summary>
        public static Sessionfact Current
        {
            get { return _current; }
            set { _current = value; }
        }

        /// <summary>
        ///     Gets the session.
        /// </summary>
        public ISession Session { get; private set; }

        /// <summary>
        ///     The get current session.
        /// </summary>
        /// <returns>
        ///     The <see cref="ISession" />.
        /// </returns>
        public ISession GetCurrentSession()
        {
            if (!CurrentSessionContext.HasBind(_sessionFactory))
                CurrentSessionContext.Bind(_sessionFactory.OpenSession());
            if (_sessionFactory.GetCurrentSession().IsOpen)
                return _sessionFactory.GetCurrentSession();
            return _sessionFactory.OpenSession();
        }

        /// <summary>
        ///     The get session.
        /// </summary>
        /// <returns>
        ///     The <see cref="ISession" />.
        /// </returns>
        public ISession GetSession()
        {
            return _sessionFactory.OpenSession();
        }

        /// <summary>
        ///     The dispose current session.
        /// </summary>
        public static void DisposeCurrentSession()
        {
            ISession currentSession = CurrentSessionContext.Unbind(_sessionFactory);

            currentSession.Close();
            currentSession.Dispose();
        }

        /// <summary>
        ///     The begin transaction.
        /// </summary>
        public void BeginTransaction()
        {
            Session = _sessionFactory.OpenSession();
            _transaction = Session.BeginTransaction();
        }

        /// <summary>
        ///     The commit.
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
        }

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            if (Session != null)
                if (Session.IsOpen)
                {
                    Session.Close();
                }
        }

        /// <summary>
        ///     The get current session 111.
        /// </summary>
        /// <returns>
        ///     The <see cref="ISession" />.
        /// </returns>
        public ISession GetCurrentSession111()
        {
            return _sessionFactory.GetCurrentSession();
        }

        /// <summary>
        ///     The create nh session factory.
        /// </summary>
        /// <returns>
        ///     The <see cref="ISessionFactory" />.
        /// </returns>
        private static ISessionFactory CreateNhSessionFactory()
        {
            Configuration cfgq;
            cfgq = new Configuration();

            cfgq.SetProperty(Environment.CurrentSessionContextClass,
                "web");
            return Fluently.Configure(cfgq)
                .Database(
                    MsSqlConfiguration.MsSql2012.ConnectionString(Resources.ConnectionString))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(UsersMap))))
                .BuildSessionFactory();
        }
    }
}