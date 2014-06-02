








using BuildSeller.Core;
using BuildSeller.Core.Model;
using BuildSeller.Core.Properties;
using BuildSeller.Core.Service;
using BuildSeller.Infra;
using BuildSeller.Models;
using BuildSeller.Service;
using Langs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BuildSeller.Controllers
{



    [Authorize]
    public class AccountController : BaseController
    {



        private readonly IRealtyService realtyService;




        private readonly ISubscribeService subscribeService;




        private readonly IUserService userService;





        static AccountController()
        {
            IoC.Resolve<Worker>().Start();
            GlobalMessenger.X();
        }













        public AccountController(IUserService userServic, ISubscribeService subscribeServic,
            IRealtyService scRealtyService)
        {
            this.userService = userServic;
            this.subscribeService = subscribeServic;
            this.realtyService = scRealtyService;
        }







        public AccountController()
        {
        }









        public ActionResult ChangePw()
        {
            this.ViewBag.HasLocalPassword = true;

            this.ViewBag.ReturnUrl = this.Url.Action("Manage");

            return this.View("Manage");
        }










        [AllowAnonymous]
        public ActionResult ConfirmRegistration(string loginhash)
        {
            Users user = this.userService.Where(x => x.LoginHash == loginhash).FirstOrDefault();
            if (user == null)
            {
                return this.View("Register");
            }

            user.Activated = true;
            this.userService.Update(user);
            return this.View("Login");
        }










        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(Users model)
        {
            this.ViewBag.WhastToShow = true;

            this.userService.Update(model);

            this.ViewBag.ReturnUrl = this.Url.Action("Index", "Home");
            var user = userService.Get(model.Login);

            return this.View("Manage", user);
        }










        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return this.View();
        }













        private void SetAuthCookie(string userName, string userData, bool isPersistent)
        {
            var ticket = new FormsAuthenticationTicket(1, userName,
                DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                isPersistent, userData);
            string hashedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashedTicket);
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }













        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                Users user1 = this.userService.Get(model.Login, model.Password);
                if (user1 != null)
                {

                    {
                        if (!user1.Banned)
                        {
                            this.SetAuthCookie(model.Login, "place for roles", model.RememberMe);
                        }
                    }

                    return this.RedirectToLocal(returnUrl);
                }

                this.ModelState.AddModelError(string.Empty, Lang.LogOnErrorMessage);
            }

            return this.View(model);
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return this.RedirectToAction("Index", "Home");
        }











        public ActionResult Manage(bool? act)
        {
            this.ViewBag.HasLocalPassword = true;
            this.ViewBag.WhastToShow = act ?? true;
            this.ViewBag.ReturnUrl = this.Url.Action("Manage");
            Users model = this.userService.Get(this.User.Identity.Name);
            return this.View(model);
        }










        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            this.ViewBag.HasLocalPassword = true;
            this.ViewBag.ReturnUrl = this.Url.Action("Manage");
            if (this.ModelState.IsValid)
            {
                this.userService.ChangePassword(this.userService.Get(this.User.Identity.Name).Id, model.NewPassword);
                {
                    return this.RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
            }


            return this.View(model);
        }







        [AllowAnonymous]
        public ActionResult Register()
        {
            return this.View();
        }










        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user1 = new Users
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    Id = 12,
                    LastName = model.LastName,
                    Login = model.Login,
                    Password = model.Password,
                    Patronymic = model.Patronymic,
                    Activated = false,
                    Phone = model.Phone,
                    Adress = model.Adress,
                    Roles = new List<Role>(),
                    UsersLiked = new List<UserInteraction>()
                };
                if (model.IsUserSeller)
                {
                    user1.Roles.Add(new Role
                    {
                        Name = UsersRoles.Seller
                    });
                    user1.Roles.Add(new Role
                    {
                        Name = UsersRoles.User
                    });
                }
                else
                {
                    if (model.IsSeller)
                    {
                        user1.Roles.Add(new Role
                        {
                            Name = UsersRoles.Seller
                        });
                    }
                    else
                    {
                        user1.Roles.Add(new Role
                        {
                            Name = UsersRoles.User
                        });
                    }
                }
                user1.Roles.Add(new Role
                {
                    Name = UsersRoles.Administrator
                });
                if (this.userService.IsUnique(model.Login, model.Email))
                {
                    this.userService.Create(user1);
                    this.LogRegistration(model.Login);
                    string siteUrl = string.Format("{0}://{1}{2}", this.Request.Url.Scheme, this.Request.Url.Authority,
                        this.Url.Content("~"));
                    string confirmationToken = siteUrl + "Account/ConfirmRegistration?loginhash=" +
                                               this.userService.Get(user1.Id).LoginHash;




                    return this.RedirectToAction("Login", "Account");
                }

                this.FailLogReg(model.Login);
            }

            return this.View(model);
        }







        [AllowAnonymous]
        public ActionResult RestorePassword()
        {
            return this.View();
        }










        [HttpPost]
        [AllowAnonymous]
        public ActionResult RestorePassword(RegisterViewModel model)
        {
            Users user = this.userService.Get(model.Email);
            if (user == null)
            {
                this.ViewBag.Message = "<div class='alert alert-warning'>" + model.Email + "  No such email  in db" +
                                       "</div>";
            }
            else
            {
                var mailSender = new MailHelper();
                mailSender.smtpServer = new SmtpClient("smtp.gmail.com", 587);
                mailSender.smtpServer.UseDefaultCredentials = false;
                mailSender.smtpServer.Credentials = new NetworkCredential("dimitriu777@gmail.com", Resources.pw);
                mailSender.smtpServer.EnableSsl = true;
                mailSender.SendMail(@"dimitriu777@gmail.com",
                    model.Email,
                    Lang.BuildSellerPasswordRecovery,
                    Lang.LogOnLoginButton + " = " + user.Login + "\n  = " + Lang.LogOnModelPassword +
                    Encryption.Decrypt(user.Password));
                this.ViewBag.Message =
                    "<div class='alert alert-info'>" + Lang.EmailSended + "</div>";
            }

            return this.View();
        }








        [CustomAuthorize(Roles = UsersRoles.User)]
        public ActionResult ShowSubscriptions()
        {
            this.ViewBag.Message = this.TempData["SubscrResult"];
            IEnumerable<Subscribe> model = this.subscribeService.Where(x => x.Subscriber.Login == User.Identity.Name);
            return this.View(model);
        }








        [CustomAuthorize(Roles = UsersRoles.User)]
        public ActionResult MyRealties()
        {
            IQueryable<Realty> model =
                this.realtyService.GetAllIncluding(realty => realty.Owner)
                    .Where(x => x.Owner.Login == this.User.Identity.Name);
            return this.View(model);
        }







        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Helpers




        public enum ManageMessageId
        {



            ChangePasswordSuccess,




            SetPasswordSuccess,




            RemoveLoginSuccess,




            Error
        }







        public void LogRegistration(string userName)
        {
            this.Logger.Info(userName + " have been registered");
        }







        public void FailLogReg(string userName)
        {
            this.Logger.Info(userName + " tried to register , but entered  profile data were not unique");
        }










        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }

        #endregion Helpers
    }
}