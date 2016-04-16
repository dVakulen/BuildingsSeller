









using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using BuildSeller.Helpers;
using Castle.Core.Logging;

namespace BuildSeller.Controllers
{



   // [HandleError(ExceptionType = typeof(InvalidOperationException), View = "Error")]
    public class BaseController : Controller
    {
        #region OnActionExecuting



        private ILogger logger = NullLogger.Instance;



        public ILogger Logger
        {
            get { return this.logger; }
            set { this.logger = value; }
        }

        public void LogInfo(string message)
        {
            this.logger.Info(message);
        }







    /*    protected override void OnException(ExceptionContext filterContext)
        {

            if (filterContext == null)
                return;


            var ex = filterContext.Exception ?? new Exception("No further information exists.");
            Logger.Error(ex.ToString());

            filterContext.ExceptionHandled = true;

            filterContext.Result = View("Error");
        }*/
        #endregion




        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }




        protected override void ExecuteCore()
        {
            int culture = 0;
            if (this.Session == null || this.Session["CurrentCulture"] == null)
            {
                int.TryParse(ConfigurationManager.AppSettings["Culture"], out culture);
                if (Session == null)
                {
                    base.ExecuteCore();
                }
                if (this.Session != null)
                    this.Session["CurrentCulture"] = culture;
            }
            else
            {
                culture = (int)this.Session["CurrentCulture"];
            }

            SessionManager.CurrentCulture = culture;
            base.ExecuteCore();
        }
    }
}
