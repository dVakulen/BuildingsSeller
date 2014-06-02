
using System.Web.Mvc;
using BuildSeller.Core;
using BuildSeller.Core.Model;
using BuildSeller.Core.Service;
using BuildSeller.Helpers;

namespace BuildSeller.Controllers
{

    public class PaymentsController : BaseController
    {

        private readonly IUserService userService;

        public PaymentsController(IUserService userServi)
        {
            this.userService = userServi;
        }

        public ActionResult Index()
        {
            this.ViewBag.Message = this.TempData["Message"];
            return this.View();
        }

        public ActionResult UserPayment()
        {
            Users user = this.userService.Get(this.User.Identity.Name);
            if (user == null)
            {
                return this.RedirectToAction("Index");
            }
            if (!user.PaidUser)
            {
                if (!RolesManager.IsUserInRole(User.Identity.Name, UsersRoles.User))
                {
                    user.AddUserToRole(UsersRoles.User);
                }

                user.PaidUser = true;
                this.userService.Update(user);
                this.TempData["Message"] = ColorfullMessages.SetDivs("Payment for being user succseed ",
                MessageType.success);
            }
            else
            {
                this.TempData["Message"] = ColorfullMessages.SetDivs("You already paid for being user ",
                MessageType.warning);
            }

            return this.RedirectToAction("Index");
        }

        public ActionResult SellerPayment()
        {
            Users user = this.userService.Get(this.User.Identity.Name);
            if (user == null)
            {
                return this.RedirectToAction("Index");
            }
            if (!user.PaidSeller)
            {
                if (!RolesManager.IsUserInRole(User.Identity.Name, UsersRoles.Seller))
                {
                    user.AddUserToRole(UsersRoles.Seller);
                }

                user.PaidSeller = true;
                this.userService.Update(user);
                this.TempData["Message"] = ColorfullMessages.SetDivs("Payment for being seller succseed ",
                MessageType.success);
            }
            else
            {
                this.TempData["Message"] = ColorfullMessages.SetDivs("You already paid for being seller ",
                MessageType.warning);
            }

            return this.RedirectToAction("Index");
        }
    }
}
