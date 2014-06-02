
using System;
using System.Web.Http;
using BuildSeller.Infra;
using BuildSeller.Service;
using Castle.MicroKernel.Registration;

namespace BuildSeller
{

    public class WindsorConfigurator
    {

        public static void Configure()
        {

            WindsorReg.Initialize();

            WindsorRegistr.RegisterSingleton(typeof(Worker), typeof(Worker));
        }
    }
}
