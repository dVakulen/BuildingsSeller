
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using BuildSeller.Core;
using BuildSeller.Core.Model;
using BuildSeller.Core.Service;
using BuildSeller.Helpers;
using BuildSeller.Models;
using BuildSeller.Service;
using Castle.Core.Internal;
using PagedList;

namespace BuildSeller.Controllers
{

    public class RealtyController : BaseController
    {

        private static readonly Dictionary<string, bool> isCurrentUserListRented = new Dictionary<string, bool>();
        private static readonly Dictionary<string, string> currentUserCategory = new Dictionary<string, string>();

        internal static readonly DateTime DateDefault = DateTime.MinValue;

        private readonly IBuildCategoriesService buildCategoriesService;

        private readonly IRealtyService realtyService;

        private readonly IUserService userService;

        public RealtyController(IRealtyService realtyServic, IUserService userServi,
        IBuildCategoriesService categoriesService)
        {
            this.realtyService = realtyServic;
            this.userService = userServi;
            this.buildCategoriesService = categoriesService;
        }

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page, bool? forRent, string category)
        {
            if (!isCurrentUserListRented.ContainsKey(this.User.Identity.Name))
            {
                isCurrentUserListRented.Add(this.User.Identity.Name, true);
            }
            if (!currentUserCategory.ContainsKey(this.User.Identity.Name))
            {
                currentUserCategory.Add(this.User.Identity.Name, string.Empty);
            }

            this.ViewBag.CurrentSort = sortOrder;
            this.ViewBag.CatSortParm = string.IsNullOrEmpty(sortOrder) ? "cat_desc" : string.Empty;
            this.ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            this.ViewBag.Message = this.TempData["Message"];
            IEnumerable<BuildCategories> cats = this.buildCategoriesService.GetAll();
            var CatsName = new string[cats.Count() + 1];
            CatsName[0] = "All";

            ViewBag.CurrCat = "All";
            int i = 1;
            foreach (BuildCategories cat in cats)
            {
                CatsName[i] = cat.CatName;
                i++;
            }

            this.ViewBag.Cats = new SelectList(CatsName);
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!category.IsNullOrEmpty())
            {
                ViewBag.CurrCat = category;
                currentUserCategory[this.User.Identity.Name] = category;
            }

            if (forRent != null)
            {
                isCurrentUserListRented[this.User.Identity.Name] = (bool)forRent;
            }

            this.ViewBag.CurrentFilter = searchString;
            bool rent = isCurrentUserListRented[this.User.Identity.Name];
            this.ViewBag.Rent = rent;
            IEnumerable<Realty> realties;
            if (this.TempData["Temp"] == null)
            {
                if (currentUserCategory[this.User.Identity.Name].IsEmpty() ||
                currentUserCategory[this.User.Identity.Name] == "All")
                {
                    realties = this.realtyService.GetAllIncluding(realty => realty.Owner, realty => realty.BuildCategory)
                    .Where(x => !x.IsSold && x.IsForRent == !rent);
                }
                else
                {
                    var cat = currentUserCategory[this.User.Identity.Name];
                    realties = this.realtyService.GetAllIncluding(realty => realty.Owner, realty => realty.BuildCategory)
                    .Where(x => !x.IsSold && x.IsForRent == !rent && x.BuildCategory.CatName == cat);
                }
            }
            else
            {
                realties = (IEnumerable<Realty>)this.TempData["Temp"];
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                realties = realties.Where(s => s.Named.ToUpper().Contains(searchString.ToUpper())
                || s.Description.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "cat_desc":
                    realties = realties.OrderByDescending(s => s.BuildCategory.CatName);
                    break;
                case "Date":
                    realties = realties.OrderBy(s => s.Created);
                    break;
                case "date_desc":
                    realties = realties.OrderByDescending(s => s.Created);
                    break;
                default:
                    realties = realties.OrderBy(s => s.Created);
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;
            return View(realties.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Find(string sity, string category)
        {

            IEnumerable<BuildCategories> cats = this.buildCategoriesService.GetAll();
            var CatsName = new string[cats.Count()];
            int i = 0;
            foreach (BuildCategories cat in cats)
            {
                CatsName[i] = cat.CatName;
                i++;
            }

            this.ViewBag.Cats = new SelectList(CatsName);
            int pageSize = 0;
            IEnumerable<Realty> realties;
            if (sity.IsNullOrEmpty() && category.IsNullOrEmpty())
            {
                this.TempData["Message"] = ColorfullMessages.SetDivs("Nothing to seek", MessageType.info);
                return this.RedirectToAction("Index");
            }

            if (!sity.IsNullOrEmpty() && !category.IsNullOrEmpty())
            {
                realties = this.realtyService.GetAllIncluding(realty => realty.BuildCategory, realty => realty.Owner)
                .Where(x => !x.IsSold
                && x.Address.ToUpper().Contains(sity.ToUpper())
                && x.BuildCategory.CatName.ToUpper() == category.ToUpper());
                this.TempData["Message"] = ColorfullMessages.SetDivs("Seek by sity and category", MessageType.info);
            }
            else
            {
                realties = this.realtyService.GetAllIncluding(realty => realty.BuildCategory, realty => realty.Owner)
                .Where(x => !x.IsSold
                && x.Address.ToUpper().Contains(sity.ToUpper()));
                this.TempData["Message"] = ColorfullMessages.SetDivs("Seek by sity", MessageType.info);
            }

            if (realties == null || realties.Count() == 0)
            {
                this.TempData["Message"] = ColorfullMessages.SetDivs("Found 0 matches", MessageType.info);
                return this.RedirectToAction("Index");
            }

            realties = realties.OrderBy(x => x.Created);
            pageSize = realties.Count();
            this.ViewBag.Message = this.TempData["Message"];
            return View("Index", realties.ToPagedList(1, pageSize));
        }

        public ActionResult Details(int id)
        {
            return View(this.realtyService.
            GetAllIncluding(realty => realty.Owner,
            realty => realty.Photos,
            realty => realty.BuildCategory)
            .FirstOrDefault(real => real.Id == id));
        }

        [CustomAuthorize(Roles = UsersRoles.Seller)]
        public ActionResult Create()
        {
            IEnumerable<BuildCategories> cats = this.buildCategoriesService.GetAll();
            var CatsName = new string[cats.Count()];
            int i = 0;
            foreach (BuildCategories cat in cats)
            {
                CatsName[i] = cat.CatName;
                i++;
            }

            this.ViewBag.Cats = new SelectList(CatsName);
            return this.View();
        }

        public ActionResult SearchSameRealties(int id)
        {
            Realty realt = this.realtyService.GetAllIncluding(x => x.BuildCategory).FirstOrDefault(x => x.Id == id);
            var model = new RealtySearchModel
            {
                CreatedHigh = realt.Created.AddDays(30),
                CreatedLow = realt.Created.AddDays(-30),
                IsForRent = realt.IsForRent,
                PriceHigh = decimal.Multiply(realt.Price, 1.5m),
                PriceLow = decimal.Multiply(realt.Price, 0.75m),
                SquareHigh = realt.Square * 1.5f,
                SquareLow = realt.Square * 0.75f,
                Town = realt.Address,
                Category = realt.BuildCategory.CatName
            };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult SearchSameRealties(RealtySearchModel model)
        {
            IEnumerable<Realty> realt = this.realtyService.GetAllIncluding(x => x.BuildCategory, realty => realty.Owner)
            .Where(x => x.IsForRent == model.IsForRent
            && (string.IsNullOrEmpty(model.Town) || x.Address.Contains(model.Town))
            && !x.IsSold
            && (string.IsNullOrEmpty(model.Category) || x.BuildCategory.CatName ==
            model.Category)
            && (model.PriceLow == 0 || x.Price >= model.PriceLow)
            && (model.PriceHigh == 0 || x.Price <= model.PriceHigh)
            && (model.SquareLow == 0 || x.Square >= model.SquareLow)
            && (model.SquareHigh == 0 || x.Square <= model.SquareHigh)
            && (model.CreatedLow == DateDefault || x.Created >= model.CreatedLow)
            && (model.CreatedHigh == DateDefault || x.Created <= model.CreatedHigh));
            this.TempData["Temp"] = realt;
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        [CustomAuthorize(Roles = UsersRoles.Seller)]
        public ActionResult Create(Realty realty, HttpPostedFileBase Upload, string cats)
        {
            try
            {
                if (Upload != null && Upload.ContentLength > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        Upload.InputStream.CopyTo(ms);
                        realty.Picture = ms.GetBuffer();
                    }
                }

                realty.Owner = this.userService.Get(this.User.Identity.Name);
                if (!realty.Owner.PaidSeller)
                {
                    this.TempData["Message"] = ColorfullMessages.SetDivs("You have to pay to sell", MessageType.danger);
                    return this.RedirectToAction("Index");
                }
                realty.Created = DateTime.Now;
                realty.Photos = new List<ImageAttachments>();
                realty.BuildCategory = this.buildCategoriesService.Where(c => c.CatName == cats).FirstOrDefault();
                this.realtyService.Create(realty);
                SubscriptionMessenger messenger = SubscriptionMessenger.Instance;
                messenger.SendRealtyCreated(realty);

                Logger.Info("realty " + realty.Named + " created");
                this.TempData["Message"] = ColorfullMessages.SetDivs("Realty created succesfully", MessageType.success);
                return this.RedirectToAction("Index");
            }
            catch
            {
                IEnumerable<BuildCategories> catsq = this.buildCategoriesService.GetAll();
                var CatsName = new string[catsq.Count()];
                int i = 0;
                foreach (BuildCategories cat in catsq)
                {
                    CatsName[i] = cat.CatName;
                    i++;
                }

                this.ViewBag.Cats = new SelectList(CatsName);
                return this.View();
            }
        }

        public ActionResult Edit(int id)
        {
            Realty realty =
            this.realtyService.GetAllIncluding(x => x.Photos, realty1 => realty1.Owner)
            .FirstOrDefault(x => x.Id == id);
            if (realty.Owner.Login != this.User.Identity.Name)
            {
                this.TempData["Message"] = ColorfullMessages.SetDivs("You dont have permission to edit this realty",
                MessageType.warning);

                return this.RedirectToAction("Index");
            }

            return View(realty);
        }

        [HttpPost]
        public ActionResult AddImagesToRealty(IEnumerable<HttpPostedFileBase> files, int id)
        {
            Realty realty = this.realtyService.GetAllIncluding(x => x.Photos).FirstOrDefault(x => x.Id == id);
            foreach (HttpPostedFileBase file in files)
            {
                if (file == null)
                {
                    continue;
                }

                if (file.ContentLength > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        var attach = new ImageAttachments
                        {
                            Content = ms.GetBuffer()
                        };

                        realty.Photos.Add(attach);
                    }
                }
            }

            this.realtyService.Update(realty);
            return this.Redirect(this.Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult Edit(Realty realty)
        {
            try
            {
                this.realtyService.Update(realty);
                this.TempData["Message"] = ColorfullMessages.SetDivs("Realty info updated", MessageType.success);
                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }

        public ActionResult Delete(int id)
        {
            Realty realty = this.realtyService.GetAllIncluding(realty1 => realty1.Owner).FirstOrDefault(x => x.Id == id);
            if (realty.Owner.Login != this.User.Identity.Name ||
            !RolesManager.IsUserInRole(realty.Owner, UsersRoles.Administrator))
            {
                this.TempData["Message"] = ColorfullMessages.SetDivs("You dont have permission to delete this realty",
                MessageType.warning);
                return this.RedirectToAction("Index");
            }

            Logger.Info("realty " + realty.Named + " deleted");
            this.realtyService.Delete(id);

            this.TempData["Message"] = ColorfullMessages.SetDivs("Realty deleted", MessageType.info);
            return this.RedirectToAction("Index");
        }

        public ActionResult DeletePhoto(int id, int realId)
        {
            Realty realt =
            this.realtyService.GetAllIncluding(realty => realty.Photos).FirstOrDefault(x => x.Id == realId);
            realt.Photos.Remove(realt.Photos.FirstOrDefault(x => x.Id == id));
            this.realtyService.Update(realt);
            return this.Redirect(this.Request.UrlReferrer.ToString());
        }
    }
}
