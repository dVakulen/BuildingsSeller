
using System.Linq;
using System.Web.Mvc;
using BuildSeller.Core.Model;
using BuildSeller.Service;

namespace BuildSeller.Controllers
{

    public class BuildCategoriesController : BaseController
    {

        private readonly IBuildCategoriesService buildCategoriesService;

        private readonly IRealtyService realtyService;

        public BuildCategoriesController(IBuildCategoriesService categoriesService, IRealtyService realtys)
        {
            this.buildCategoriesService = categoriesService;
            this.realtyService = realtys;
        }

        public ActionResult Index()
        {
            this.ViewBag.Message = this.TempData["Message"];
            return this.View(this.buildCategoriesService.GetAll());
        }

        public ActionResult Details(int id)
        {
            return this.View(this.buildCategoriesService.Get(id));
        }

        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Create(BuildCategories cat)
        {
            try
            {
                if (this.buildCategoriesService.IsUnique(cat))
                {
                    this.buildCategoriesService.Create(cat);

                    this.ViewBag.Message = "<div class='alert alert-success'>" + "Cat " + cat.CatName + " Created" +
                    "</div>";
                    Logger.Info("Cat +" + cat.CatName + " created");
                    return this.RedirectToAction("Index");
                }

                this.ViewBag.Message = "<div class='alert alert-warning'>" + "Cat with same name is already in db" +
                "</div>";
                return this.View();
            }
            catch
            {
                return this.View();
            }
        }

        public ActionResult Edit(int id)
        {
            return this.View(this.buildCategoriesService.Get(id));
        }

        [HttpPost]
        public ActionResult Edit(BuildCategories cat)
        {
            try
            {
                if (this.buildCategoriesService.IsUnique(cat))
                {
                    this.buildCategoriesService.Update(cat);
                    return this.RedirectToAction("Index");
                }

                this.ViewBag.Message = "<div class='alert alert-warning'>" + "Cat with same name is already in db" +
                "</div>";
                return this.View();
            }
            catch
            {
                return this.View();
            }
        }

        public ActionResult Delete(int id)
        {
            bool isRealtysHaveCat =
            this.realtyService.GetAllIncluding(x => x.BuildCategory).Where(x => x.BuildCategory.Id == id).Any();

            if (!isRealtysHaveCat)
            {
                this.buildCategoriesService.Delete(id);

                Logger.Info("Cat +" + id + " deleted");
                this.TempData["Message"] = "<div class='alert alert-success'>" + "Category deleted " + "</div>";
            }
            else
            {
                this.TempData["Message"] = "<div class='alert alert-info'>" +
                "Can not delete category because there is products in this cat " + "</div>";
            }

            return this.RedirectToAction("Index");
        }
    }
}
