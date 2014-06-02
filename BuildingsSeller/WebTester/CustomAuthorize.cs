
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BuildSeller.Core;
using BuildSeller.Core.Model;
using BuildSeller.Core.Service;
using BuildSeller.Infra;
using Castle.Core.Internal;

namespace BuildSeller
{

    public class CustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                AuthorizeCore(filterContext.HttpContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
        }

        private readonly IUserService userService = IoC.Resolve<IUserService>();

        public string Role { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.Name.IsNullOrEmpty())
            {
                return false;
            }

            Users user = this.userService.Get(httpContext.User.Identity.Name);

            if (user == null || user.Roles.IsNullOrEmpty())
            {
                return false;
            }

            if (!this.Role.IsNullOrEmpty())
            {
                return RolesManager.IsUserInRole(user, this.Role);
            }

            return RolesManager.IsUserInRole(user, this.Roles);
        }
    }
}
