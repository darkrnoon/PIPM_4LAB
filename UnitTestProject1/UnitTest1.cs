using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PIPM_4LAB;

namespace UnitTestProject1
{
    [TestClass]
    public class ProductsTests
    {
        private ProductsEntities db;
        private Products testProduct;

        [TestInitialize]
        public void SetUp()
        {
            db = ProductsEntities.GetContext();
            testProduct = new Products { Name = "TestProduct", Price = 99, Quantity = 10, Image = "test.jpg" };
            db.Products.Add(testProduct);
            db.SaveChanges();
        }

        [TestCleanup]
        public void TearDown()
        {
            var productToDelete = db.Products.FirstOrDefault(p => p.Name == "TestProduct");
            if (productToDelete != null)
            {
                db.Products.Remove(productToDelete);
                db.SaveChanges();
            }
            var newProductToDelete = db.Products.FirstOrDefault(p => p.Name == "NewProduct");
            if (newProductToDelete != null)
            {
                db.Products.Remove(newProductToDelete);
                db.SaveChanges();
            }
            foreach (var entry in db.ChangeTracker.Entries().ToList())
            {
                entry.State = System.Data.Entity.EntityState.Detached;
            }
        }

        [TestMethod]
        public void Test_GetProducts()
        {
            var products = db.Products.ToList();
            Assert.IsTrue(products.Count > 0, "Список товаров не должен быть пустым");
        }

        [TestMethod]
        public void Test_AddProduct()
        {
            var newProduct = new Products { Name = "NewProduct", Price = 50, Quantity = 5, Image = "new.jpg" };
            db.Products.Add(newProduct);
            db.SaveChanges();

            var addedProduct = db.Products.FirstOrDefault(p => p.Name == "NewProduct");
            Assert.IsNotNull(addedProduct, "Продукт должен быть добавлен в БД");
        }

        [TestMethod]
        public void Test_UpdateProduct()
        {
            var product = db.Products.FirstOrDefault(p => p.Name == "TestProduct");
            Assert.IsNotNull(product, "Исходный товар должен существовать");

            product.Price = 199;
            db.SaveChanges();

            var updatedProduct = db.Products.FirstOrDefault(p => p.Name == "TestProduct");
            Assert.AreEqual(199, updatedProduct.Price, "Цена товара должна обновиться");
        }

        [TestMethod]
        public void Test_DeleteProduct()
        {
            var product = db.Products.FirstOrDefault(p => p.Name == "TestProduct");
            Assert.IsNotNull(product, "Товар должен существовать перед удалением");

            db.Products.Remove(product);
            db.SaveChanges();

            var deletedProduct = db.Products.FirstOrDefault(p => p.Name == "TestProduct");
            Assert.IsNull(deletedProduct, "Товар должен быть удален из БД");
        }
    }
}
