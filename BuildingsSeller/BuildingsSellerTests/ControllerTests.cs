using BuildSeller.Controllers;
using BuildSeller.Core.Model;
using BuildSeller.Core.Service;
using BuildSeller.Models;
using BuildSeller.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BuildingsSellerTests
{
    [TestClass]
    public class ControllerTest
    {
        private Realty realt;

        [TestInitialize()]
        public void Initialize()
        {
            realt = new Realty
           {
               Address = "Addr",
               BuildCategory = new BuildCategories
               {
                   CatName = "Cat",
                   Id = 1
               },
               IsForRent = false,
               Id = 2,
               Description = "Descr",
               Created = DateTime.Now,
               IsSold = false,
               Named = "Name",
               Price = 100,
               Square = 10
           };
        }
        [TestMethod]
        public void RealtySearch_UnrealSearchString_ReturnsZeroList()
        {
            var i = new Mock<IRealtyService>();
            IList<Realty> realties = new List<Realty>();
            var model = new RealtySearchModel
            {
                CreatedHigh = realt.Created.AddDays(30),
                CreatedLow = realt.Created.AddDays(31),
                IsForRent = realt.IsForRent,
                PriceHigh = decimal.Multiply(realt.Price, 1.5m),
                PriceLow = decimal.Multiply(realt.Price, 1.75m),
                SquareHigh = realt.Square * 1.5f,
                SquareLow = realt.Square * 1.75f,
                Town = realt.Address,
                Category = realt.BuildCategory.CatName
            };
            realties.Add(realt);
            i.Setup(x => x.GetAllIncluding(c => c.BuildCategory, realty => realty.Owner)).Returns(realties.Select(x => x).AsQueryable());
            i.Setup(x => x.GetAll()).Returns(realties as IQueryable<Realty>);
            var controller = new RealtyController(i.Object, new Mock<IUserService>().Object, new Mock<IBuildCategoriesService>().Object);
            var result = controller.SearchSameRealties(model) as RedirectToRouteResult;
            var res = controller.TempData["Temp"];
            var resultRealtyList= res as IEnumerable<Realty>;
            Assert.AreEqual(0, resultRealtyList.Count());
        }
        [TestMethod]
        public void RealtySearch_LegitSearchParameters_ReturnsOneResult()
        {
            var i = new Mock<IRealtyService>();
            IList<Realty> realties = new List<Realty>();
            realties.Add(realt);
            i.Setup(x => x.GetAllIncluding(c => c.BuildCategory, realty => realty.Owner)).Returns(realties.Select(x => x).AsQueryable());
            i.Setup(x => x.GetAll()).Returns(realties);
            var controller = new RealtyController(i.Object, new Mock<IUserService>().Object, new Mock<IBuildCategoriesService>().Object);
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
            var result = controller.SearchSameRealties(model) as RedirectToRouteResult;
            var res = controller.TempData["Temp"];
            var count = (res as IEnumerable<Realty>).Count();
            Assert.AreEqual(1, count);

        }
    }
}
