
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using BuildSeller.Controllers;
namespace BuildSeller
{

public class WindsorCompositionRoot : IHttpControllerActivator
{
private readonly IWindsorContainer _container;

public WindsorCompositionRoot(IWindsorContainer container)
{
_container = container;
}

public IHttpController Create(
HttpRequestMessage request,
HttpControllerDescriptor controllerDescriptor,
Type controllerType)
{
var controller =
(IHttpController)_container.Resolve(controllerType);

request.RegisterForDispose(
new Release(
() => _container.Release(controller)));

return controller;
}

private class Release : IDisposable
{
private readonly Action _release;

public Release(Action release)
{
_release = release;
}

public void Dispose()
{
_release();
}
}
}

public class WindsorControllerFactory11 : DefaultControllerFactory
{

private readonly IWindsorContainer container;

public WindsorControllerFactory11(IWindsorContainer container)
{
this.container = container;
IEnumerable<Type> controllerTypes =
from t in Assembly.GetExecutingAssembly().GetTypes()
where typeof(IController).IsAssignableFrom(t)
select t;
foreach (Type t in controllerTypes)
{
container.Register(Component.For(t).LifeStyle.Transient);
}

}

protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
{
if (controllerType == null)
{
return null;
}

return (IController) this.container.Resolve(controllerType);
}

}
}
