
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BuildSeller.Core;
using BuildSeller.Core.Model;
using BuildSeller.Core.Service;
using BuildSeller.Helpers;
using Castle.Core.Internal;
using PagedList;

namespace BuildSeller.Controllers
{

    public class UserManagerController : BaseController
    {

        private readonly IUserService userService;

        public UserManagerController(IUserService us1)
        {
            this.userService = us1;
        }

        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult Create(Users user)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            IList<Users> us = this.userService.Where(x => x.Login == user.Login || x.Email == user.Email);
            if (us == null)
            {
                return this.View();
            }

            this.userService.Create(user);

            return this.RedirectToAction("ManageUsers");
        }

        public ActionResult Delete(int id)
        {
            this.userService.Delete(id);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddComment(int id, string comment)
        {
            Users user = this.userService.Get(id);
            user.Comments = user.Comments + Environment.NewLine + this.User.Identity.Name + " : " + comment;
            this.userService.Update(user);

            return this.Redirect(this.Request.UrlReferrer.ToString());
        }

        public ActionResult Like(int id)
        {
            Users user = this.userService.Get(id);

            Users userThatLikes = this.userService.Get(this.User.Identity.Name);

            if (!userThatLikes.UsersLiked.Where(x => x.Name == user.Login && x.IsLiked).Any())
            {
                user.Likes = ++user.Likes;
                var inter = new UserInteraction
                {
                    Name = user.Login,
                    IsLiked = true,
                    IsInteracted = true
                };
                if (userThatLikes.UsersLiked.Where(x => x.Name == user.Login && x.IsInteracted).Any())
                {
                    user.Dislikes = --user.Dislikes;
                    inter = userThatLikes.UsersLiked.Where(x => x.Name == user.Login && x.IsInteracted).FirstOrDefault();
                    inter.IsLiked = true;
                }
                else
                {
                    userThatLikes.UsersLiked.Add(inter);
                }

                this.userService.Update(user);
                this.userService.Update(userThatLikes);
            }

            return this.Redirect(this.Request.UrlReferrer.ToString());
        }

        public ActionResult DisLike(int id)
        {
            Users user = this.userService.Get(id);

            Users userThatLikes = this.userService.Get(this.User.Identity.Name);

            if (!userThatLikes.UsersLiked.Where(x => x.Name == user.Login && !x.IsLiked).Any())
            {
                user.Dislikes = ++user.Dislikes;
                var inter = new UserInteraction
                {
                    Name = user.Login,
                    IsLiked = false,
                    IsInteracted = true
                };
                if (userThatLikes.UsersLiked.Where(x => x.Name == user.Login && x.IsInteracted).Any())
                {
                    user.Likes = --user.Likes;
                    inter = userThatLikes.UsersLiked.Where(x => x.Name == user.Login && x.IsInteracted).FirstOrDefault();
                    inter.IsLiked = false;
                }
                else
                {
                    userThatLikes.UsersLiked.Add(inter);
                }

                this.userService.Update(user);
                this.userService.Update(userThatLikes);
            }

            return this.Redirect(this.Request.UrlReferrer.ToString());
        }

        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult Ban(int id)
        {
            Users us = this.userService.Get(id);
            if (us.Login != this.User.Identity.Name)
            {
                us.Banned = !us.Banned;
                this.userService.Update(us);
                string banned = us.Banned ? "banned" : "unbanned";
                this.TempData["Message"] = ColorfullMessages.SetDivs("User " + us.Login + " has succesfully " + banned,
                MessageType.info);
            }
            else
            {
                this.TempData["Message"] = ColorfullMessages.SetDivs("You cannt ban yourself", MessageType.warning);
            }

            return this.RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult DeleteRoleForUser(string usersName, string roleName)
        {
            if (!((roleName == UsersRoles.Administrator) && this.User.Identity.Name == usersName))
            {
                bool res = this.RoleDeleteFromUser(usersName, roleName);
                if (res)
                {
                    this.ViewBag.Title = "Role deletion sucseed";

                    this.ViewBag.Message =
                    ColorfullMessages.SetDivs("Role deletion for " + usersName + " from " + roleName + " sucseed",
                    MessageType.info);
                }
                else
                {
                    this.ViewBag.Message =
                    ColorfullMessages.SetDivs("Role deletion for " + usersName + " from " + roleName + " failed",
                    MessageType.danger);
                }
            }

            string[] thisUserRoles = RolesManager.GetRolesForUser(this.userService.Get(usersName));
            string[] rolesList = RolesManager.GetAllRoles();
            this.ViewBag.Roles = new SelectList(rolesList.Except(thisUserRoles));
            this.ViewBag.UsersNames = this.GetUserOrdered(usersName);

            this.ViewBag.RolesForThisUser = thisUserRoles;
            return this.View("ManageUsers");
        }

        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult DeleteRoleForUser1()
        {
            var list = new SelectList(RolesManager.GetAllRoles());
            this.ViewBag.Roles = list;

            this.ViewBag.UsersNames = this.GetUserNames();
            return this.View("DeleteRoleForUser");
        }

        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult Edit(int id)
        {
            Users user = this.userService.Get(id);
            user.LoginHash = user.Login;
            return this.View(user);
        }

        public ActionResult Details(int id)
        {
            Users user = this.userService.Get(id);
            return this.View(user);
        }

        [HttpPost]
        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult Edit(Users user)
        {
            user.Roles = this.userService.Get(user.Id).Roles;
            try
            {
                this.userService.Update(user.Id, user);
                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }

        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult GetRoles(string userName)
        {
            SelectList userNames = this.GetUserOrdered(userName);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                this.ViewBag.RolesForThisUser = RolesManager.GetRolesForUser(this.userService.Get(userName));
                SelectListItem selected = userNames.First(x => x.Text == userName);
                selected.Selected = true;

                this.ViewBag.Message = ColorfullMessages.SetDivs("Roles for " + userName, MessageType.info);
            }
            else
            {
                this.ViewBag.Message = ColorfullMessages.SetDivs("No such user ", MessageType.danger);
            }

            var list = new SelectList(RolesManager.GetAllRoles());
            this.ViewBag.Roles = list;
            this.ViewBag.UsersNames = userNames;
            return this.View("ManageUsers");
        }

        public SelectList GetUserNames()
        {
            IEnumerable<Users> users = this.userService.GetAll();
            IList<string> x = new List<string>();

            foreach (Users user in users)
            {
                x.Add(user.Login);
            }

            var names = new SelectList(x);
            return names;
        }

        public SelectList GetUserOrdered(string name)
        {
            IEnumerable<Users> users = this.userService.GetAll();
            IList<string> x = new List<string>();
            x.Add(name);
            foreach (Users user in users)
            {
                if (user.Login != name)
                {
                    x.Add(user.Login);
                }
            }

            var names = new SelectList(x);
            return names;
        }

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            this.ViewBag.CurrentSort = sortOrder;
            this.ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : string.Empty;
            this.ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            this.ViewBag.Message = this.TempData["Message"];
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewBag.CurrentFilter = searchString;

            IEnumerable<Users> users = this.userService.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                || s.Login.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    users = users.OrderBy(s => s.RegisterDateTime);
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.RegisterDateTime);
                    break;
                default:
                    users = users.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = page ?? 1;
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult ManageUsers()
        {
            string[] rolesList = RolesManager.GetAllRoles();
            SelectList userNames = this.GetUserNames();
            this.ViewBag.UsersNames = userNames;
            string[] thisUserRoles = RolesManager.GetRolesForUser(this.userService.Get(userNames.FirstOrDefault().Text));
            this.ViewBag.Roles = new SelectList(rolesList.Except(thisUserRoles));
            this.ViewBag.UsersNames = this.GetUserOrdered(userNames.FirstOrDefault().Text);
            this.ViewBag.RolesForThisUser = thisUserRoles;
            return this.View();
        }

        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult ManageConcreteUser(string userName)
        {
            Users user = this.userService.Get(userName);
            string[] thisUserRoles = RolesManager.GetRolesForUser(user);
            string[] list = RolesManager.GetAllRoles();
            this.ViewBag.Roles = new SelectList(list.Except(thisUserRoles));
            this.ViewBag.UsersNames = this.GetUserNames();
            return this.View("ManageUsers");
        }

        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult RoleAddToUser()
        {
            var list = new SelectList(Roles.GetAllRoles());
            this.ViewBag.Roles = list;
            this.ViewBag.UsersNames = this.GetUserNames();
            return this.View("ManageUsers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = UsersRoles.Administrator)]
        public ActionResult RoleAddToUser(int? id, string btnSubmit, string roleName, string usersName)
        {
            Users user = this.userService.Get(usersName);
            string[] thisUserRoles = RolesManager.GetRolesForUser(user);
            if (btnSubmit != "Get")
            {
                bool res;
                if (usersName.IsNullOrEmpty() || roleName.IsNullOrEmpty())
                {
                    res = false;
                }
                else
                {
                    res = this.RoleAddToUser(roleName, usersName);
                }

                if (res)
                {
                    thisUserRoles = RolesManager.GetRolesForUser(user);

                    this.ViewBag.Message =
                    ColorfullMessages.SetDivs("Role " + roleName + " added succesfully to " + usersName,
                    MessageType.success);
                }
                else
                {
                    this.ViewBag.Message =
                    ColorfullMessages.SetDivs("User " + usersName + " already in role " + roleName,
                    MessageType.warning);
                }

                string[] rolesList = RolesManager.GetAllRoles();
                thisUserRoles = RolesManager.GetRolesForUser(this.userService.Get(usersName));
                this.ViewBag.Roles = new SelectList(rolesList.Except(thisUserRoles));
                this.ViewBag.UsersNames = this.GetUserOrdered(usersName);

                this.ViewBag.RolesForThisUser = thisUserRoles;
                return this.View("ManageUsers");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(usersName))
                {
                    this.ViewBag.RolesForThisUser = thisUserRoles;

                    this.ViewBag.Message = ColorfullMessages.SetDivs("Roles for " + usersName, MessageType.info);
                }
                else
                {
                    this.ViewBag.Message = ColorfullMessages.SetDivs("No such user", MessageType.warning);
                }

                string[] rolesList = RolesManager.GetAllRoles();
                this.ViewBag.Roles = new SelectList(rolesList.Except(thisUserRoles));
                this.ViewBag.UsersNames = this.GetUserOrdered(usersName);
                return this.View("ManageUsers");
            }
        }

        public bool RoleAddToUser(string roleName, string usersName)
        {
            bool sucsess;
            sucsess = false;
            Users user = this.userService.Get(usersName);

            if (!RolesManager.IsUserInRole(user, roleName))
            {
                user.AddUserToRole(roleName);
                sucsess = true;
            }

            if (sucsess)
            {
                this.userService.Update(user);
            }

            return sucsess;
        }

        public bool RoleDeleteFromUser(string usersName, string roleName)
        {
            bool sucsess = false;
            if (roleName.IsNullOrEmpty() || usersName.IsNullOrEmpty())
            {
                return false;
            }

            Users user = this.userService.Get(usersName);

            if (RolesManager.IsUserInRole(user, roleName))
            {
                user.RemoveUserFromRole(roleName);
                sucsess = true;
            }

            if (sucsess)
            {
                this.userService.Update(user);
            }

            return sucsess;
        }
    }
}
