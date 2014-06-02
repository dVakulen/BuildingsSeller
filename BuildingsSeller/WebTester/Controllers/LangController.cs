
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BuildSeller.Helpers;

namespace BuildSeller.Controllers
{

    public class LangController : Controller
    {

        private readonly IDictionary<string, string> langs = new Dictionary<string, string>
{
{ "en", "english" },
{ "ru", "русский" },
{ "auto", "default" },
};

        public ActionResult Index()
        {
            HttpCookie c = this.Request.Cookies["lang"];
            string k = c == null ? "auto" : c.Value;
            this.ViewBag.lang = this.langs[k];
            return this.View();
        }

        public ActionResult Langs()
        {
            return this.View(this.langs);
        }

        [HttpPost]
        public ActionResult Change(string l)
        {
            int period = 1;
            if (l == "auto")
            {
                period = -1;
            }

            var aCookie = new HttpCookie("lang") { Value = l, Expires = DateTime.Now.AddYears(1) };
            this.Response.Cookies.Add(aCookie);
            return this.Content(string.Empty);
        }

        public ActionResult ChangeCurrentCulture(int id)
        {
            SessionManager.CurrentCulture = id;
            this.Session["CurrentCulture"] = id;
            return this.Redirect(this.Request.UrlReferrer.ToString());
        }
    }
}
