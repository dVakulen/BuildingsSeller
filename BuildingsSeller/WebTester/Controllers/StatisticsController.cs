
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BuildSeller.Core.Model;
using BuildSeller.Helpers;
using BuildSeller.Models;
using BuildSeller.Service;

namespace BuildSeller.Controllers
{

    public class StatisticsController : BaseController
    {

        private readonly IBuildCategoriesService categoriesService;

        private readonly IRealtyService realtyService;

        public StatisticsController(IBuildCategoriesService bsService, IRealtyService service)
        {
            this.realtyService = service;
            this.categoriesService = bsService;
        }

        public ActionResult Index()
        {
            this.ViewBag.Message = this.TempData["Message"];
            IEnumerable<BuildCategories> cats = this.categoriesService.GetAll();
            var CatsName = new string[cats.Count()];
            int i = 0;
            foreach (BuildCategories cat in cats)
            {
                CatsName[i] = cat.CatName;
                i++;
            }

            this.ViewBag.Cats = new SelectList(CatsName);
            var model = new StatModel();
            model.Until = this.realtyService.GetAll().OrderBy(x => x.Created).FirstOrDefault().Created;
            model.ToTime = this.realtyService.GetAll().OrderByDescending(x => x.Created).FirstOrDefault().Created;
            return View(model);
        }

        public ActionResult GetStatistic(StatModel model, string cats)
        {
            int stat =
            this.realtyService.GetAllIncluding(x => x.BuildCategory)
            .Count(
            x =>
            x.Created >= model.Until && x.Created <= model.ToTime && x.IsSold &&
            x.BuildCategory.CatName == cats);
            this.TempData["Message"] =
            ColorfullMessages.SetDivs(
            "Sold count for " + cats + " of period " + model.Until + " to " + model.ToTime + " : " + stat,
            MessageType.info);
            return this.RedirectToAction("Index");
        }
    }
}
