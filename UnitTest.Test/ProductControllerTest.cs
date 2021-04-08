using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Web.Models;

namespace UnitTest.Test
{
    public class ProductControllerTest
    {
        protected DbContextOptions<UnitTestContext> _contextOptions { get; private set; }

        public void SetContextOptions(DbContextOptions<UnitTestContext> contextOptions)
        {
            _contextOptions = contextOptions;
            Seed();
        }

        public void Seed()
        {
            using (UnitTestContext context = new UnitTestContext(_contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Category.Add(new Category { Name = "Category 1" });
                context.Category.Add(new Category { Name = "Category 2" });
                context.SaveChanges();

                context.Products.Add(new Product() { CategoryId = 1, Name = "Phone", Price = 100, Stock = 100, Color = "White" });
                context.Products.Add(new Product() { CategoryId = 1, Name = "Computer", Price = 200, Stock = 200, Color = "Black" });

                context.SaveChanges();
            }
        }
    }
}
