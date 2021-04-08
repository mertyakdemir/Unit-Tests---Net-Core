using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Web.Controllers;
using UnitTest.Web.Models;
using Xunit;

namespace UnitTest.Test
{
    public class ProductControllerTestWithSqlServerLocalDb : ProductControllerTest
    {
        public ProductControllerTestWithSqlServerLocalDb()
        {
            var sqlCon = @"Server=(localdb)\MSSQLLocalDB;Database=TestDB;Trusted_Connection=true; MultipleActiveResultSets=true";

            SetContextOptions(new DbContextOptionsBuilder<UnitTestContext>().UseSqlServer(sqlCon).Options);
        }

        [Fact]
        public async Task Create_ModelValidProduct_ReturnRedirectToActionWithSaveProduct()
        {
            var newProduct = new Product { Name = "Apple", Price = 700, Stock = 50, Color = "Gray" };
            using (var context = new UnitTestContext(_contextOptions))
            {
                var category = context.Category.First();

                newProduct.CategoryId = category.Id;

                //var repository = new repository<Product>(context);
                var controller = new ProductsController(context);

                var result = await controller.Create(newProduct);

                var redirect = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirect.ActionName);
            }

            using (var context = new UnitTestContext(_contextOptions))
            {
                var product = context.Products.FirstOrDefault(x => x.Name == newProduct.Name);

                Assert.Equal(newProduct.Name, product.Name);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteCategory_ExistCategoryId_DeletedAllProducts(int categoryId)
        {
            using (var context = new UnitTestContext(_contextOptions))

            {
                var category = await context.Category.FindAsync(categoryId);

                Assert.NotNull(category);

                context.Category.Remove(category);

                context.SaveChanges();
            }

            using (var context = new UnitTestContext(_contextOptions))
            {
                var products = await context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();

                Assert.Empty(products);
            }
        }
    }
}
