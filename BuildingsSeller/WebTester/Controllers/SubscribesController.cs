
using System.Linq;
using System.Web.Mvc;
using BuildSeller.Core;
using BuildSeller.Core.Model;
using BuildSeller.Core.Service;
using BuildSeller.Helpers;
using BuildSeller.Service;

namespace BuildSeller.Controllers
{

    public class SubscribesController : BaseController
    {

        private readonly IBuildCategoriesService buildCategoriesService;

        private readonly ISubscribeService subscribeService;

        private readonly IUserService userService;

        public SubscribesController(ISubscribeService subscribService, IBuildCategoriesService categoriesService,
        IUserService userServic)
        {
            this.subscribeService = subscribService;
            this.buildCategoriesService = categoriesService;
            this.userService = userServic;
        }

        public ActionResult Index()
        {
            this.ViewBag.Message = this.TempData["SubscrResult"];
            return this.View(this.buildCategoriesService.GetAll());
        }

        [CustomAuthorize(Roles = UsersRoles.User)]
        public ActionResult Subscribe(int id)
        {
            BuildCategories cat = this.buildCategoriesService.Get(id);
            if (!this.subscribeService.GetAllIncluding(c => c.Subscriber).Where(x => x.Name == cat.CatName && x.Subscriber.Login == User.Identity.Name).Any())
            {
                var subscr = this.userService.Get(this.User.Identity.Name);
                if (!subscr.PaidUser)
                {
                    this.TempData["SubscrResult"] =
                    ColorfullMessages.SetDivs("You must pay to subscribe", MessageType.warning);
                }
                else
                {
                    var subscription = new Subscribe
                    {
                        Description = cat.CatName,
                        Name = cat.CatName,
                        Subscriber = subscr
                    };

                    this.subscribeService.Create(subscription);

                    this.TempData["SubscrResult"] =
                    ColorfullMessages.SetDivs("Subscription on " + cat.CatName + " were created", MessageType.success);
                }
            }
            else
            {
                this.TempData["SubscrResult"] =
                ColorfullMessages.SetDivs("Subscription on " + cat.CatName + " already exists", MessageType.warning);
            }

            return this.RedirectToAction("Index");
        }

        public ActionResult UnSubscribe(int id)
        {
            Subscribe subToDelete = this.subscribeService.Get(id);
            this.subscribeService.Delete(id);

            this.TempData["SubscrResult"] =
            ColorfullMessages.SetDivs("Subscription on" + subToDelete.Name + " were deleted", MessageType.info);
            return this.RedirectToAction("ShowSubscriptions", "Account");
        }
    }
}
