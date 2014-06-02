
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BuildSeller.Core.Model;
using BuildSeller.Service;

namespace BuildSeller.Controllers
{

    public class GlobalMessengerController : Controller
    {

        private readonly IBuildCategoriesService buildCategoriesService;

        private readonly ISubscribeService subscribeService;

        public GlobalMessengerController(IBuildCategoriesService categoriesService, ISubscribeService subscribService)
        {
            this.buildCategoriesService = categoriesService;
            this.subscribeService = subscribService;
        }

        public ActionResult Index()
        {
            this.ViewBag.Message = this.TempData["Message"];
            return this.View(this.buildCategoriesService.GetAll());
        }

        public ActionResult Details(int id)
        {
            return this.View(this.GetSubscribersForCat(id));
        }

        public ActionResult CreateMessage(int id)
        {
            return View(id);
        }

        public List<Users> GetSubscribersForCat(int id)
        {
            string catName = this.buildCategoriesService.Get(id).CatName;
            IQueryable<Subscribe> subscriptions =
            this.subscribeService.GetAllIncluding(x => x.Subscriber).Where(x => x.Name == catName);
            var users = new List<Users>();
            foreach (Subscribe subscr in subscriptions)
            {
                users.Add(subscr.Subscriber);
            }

            return users;
        }

        [HttpPost]
        public ActionResult SendMessage(int id, string message, string title)
        {
            GlobalMessenger.Send(message, title, string.Empty, this.GetSubscribersForCat(id));
            this.TempData["Message"] = "<div class='alert alert-success'>" + "Message sended succesfully" + "</div>";
            return this.RedirectToAction("Index");
        }
    }
}
