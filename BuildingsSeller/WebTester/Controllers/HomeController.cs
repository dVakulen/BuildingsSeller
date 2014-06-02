
using BuildSeller.Controllers;
using BuildSeller.Core;
using BuildSeller.Core.Model;
using BuildSeller.Core.Service;
using BuildSeller.Helpers;
using BuildSeller.Service;
using Castle.Core.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebTester.Controllers
{

    public class HomeController : BaseController
    {

        public delegate void StatusUpdateHandler(object sender, CategoryEventArgs e);

        private readonly IBuildCategoriesService BuildCategoriesService;

        private readonly IRealtyService RealtyService;

        private readonly IUserService us;

        private ILogger _logger = NullLogger.Instance;

        private ISubscribeService subscribeService;

        public HomeController(IUserService us1, IBuildCategoriesService s, IRealtyService sd, ISubscribeService sddd)
        {
            RolesManager.userService = us1;
            this.us = us1;
            this.BuildCategoriesService = s;
            this.RealtyService = sd;
            this.subscribeService = sddd;
        }

        public HomeController()
        {
        }

        public ActionResult About()
        {
            return this.View();
        }

        public ActionResult Contact()
        {
            this.ViewBag.Message = "WebTester contacts";
            return this.View();
        }

        public IEnumerable<string> GetStatistic()
        {
            var statistics = new List<string>();
            IEnumerable<BuildCategories> cats = this.BuildCategoriesService.GetAllIncluding(categories => categories.Realties);
            foreach (BuildCategories cat in cats)
            {
                int statCount = cat.Realties.Where(x => x.IsSold).Count();
                statistics.Add(cat.CatName + " Total solds: " + statCount);
            }

            return statistics;
        }

        public ActionResult ErrorPage()
        {
            var err = this.RouteData.Values["Err"];
            if (err != null)
                ViewBag.Message = ColorfullMessages.SetDivs(err.ToString(), MessageType.danger);
            return this.View("Error");
        }

        public ActionResult Index()
        {
            this.ViewBag.Statistic = this.GetStatistic();
            IQueryable<Realty> realties = this.RealtyService.GetAllIncluding(realty => realty.Owner,
            realty => realty.BuildCategory)
            .OrderByDescending(x => x.Created)
            .Where(x => !x.IsSold)
            .Take(5);
            return this.View(realties);
        }
    }
}
