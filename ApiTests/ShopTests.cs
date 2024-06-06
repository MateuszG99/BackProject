using Domain.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseContext;
using Presentation.Controllers;

namespace ApiTests
{
    [TestClass]
    public class ShopTests
    {
        static ShopsController GetTestController()
        {
            var testDb = new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options);

            testDb.Owners.Add(new Owner
            {
                Firstname = "Jan",
                Surname = "Kowalski"
            });

            testDb.Categories.Add(new Category
            {
                CategoryName = "Obuwniczy"
            });

            testDb.Shops.Add(new Shop
            {
                ShopName = "ButShop",
                ShopAddress = "Koronkowa 34",
                OwnerId = 1,
                CategoryId = 1
            });

            testDb.SaveChanges();

            return new ShopsController(testDb);
        }

        [TestMethod]
        public void GetAll_Ok()
        {
            var testObject = GetTestController();
            var result = testObject.GetAll() as OkObjectResult;
            Assert.AreEqual(1, (result.Value as List<Shop>).Count);
        }

        [TestMethod]
        public void GetById_Ok()
        {
            var testObject = GetTestController();
            var result = testObject.GetById(1) as OkObjectResult;

            var resultValue = result.Value as Shop;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual("ButShop", resultValue.ShopName);
            Assert.IsNotNull(resultValue.Owner);
            Assert.AreEqual("Jan", resultValue.Owner.Firstname);
            Assert.IsNotNull(resultValue.Category);
            Assert.AreEqual("Obuwniczy", resultValue.Category.CategoryName);
        }

        [TestMethod]
        public void GetById_Nok()
        {
            var testObject = GetTestController();
            var result = testObject.GetById(2) as NotFoundResult;
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void Create_Ok()
        {
            var testObject = GetTestController();
            testObject.Create(new Shop
            {
                ShopName = "Nowy",
                ShopAddress = "Adres",
                OwnerId = 1,
                CategoryId = 1
            });

            var result = testObject.GetById(2) as OkObjectResult;
            var resultValue = result.Value as Shop;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual("Nowy", resultValue.ShopName);
        }

        [TestMethod]
        public void Update_Ok() 
        {
            var testObject = GetTestController();
            testObject.Update(1, new Shop
            {
                ShopName = "Nowy",
                ShopAddress = "Adres",
                OwnerId = 1,
                CategoryId = 1
            });

            var result = testObject.GetById(1) as OkObjectResult;
            var resultValue = result.Value as Shop;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual("Nowy", resultValue.ShopName);

            var idResult = testObject.GetById(2) as NotFoundResult;
            Assert.AreEqual(404, idResult.StatusCode);
        }

        [TestMethod]
        public void Delete_Ok() 
        { 
            var testObject = GetTestController();
            testObject.Delete(1);
            var result = testObject.GetById(1) as NotFoundResult;
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}