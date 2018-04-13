using DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAL.EF.Context
{
    public class EFDataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Category> Categories { get; set; }

        static EFDataContext()
        {
            Database.SetInitializer<EFDataContext>(new StoreDbInitializer());
        }

        public EFDataContext(string connectionString)
            : base(connectionString)
        {
        }
    }

    public class StoreDbInitializer : DropCreateDatabaseAlways<EFDataContext>
    {
        protected override void Seed(EFDataContext db)
        {
            db.Categories.Add(new Category { Id = 1, Name = "Electronics" });
            db.Categories.Add(new Category { Id = 2, Name = "Food" });
            db.Categories.Add(new Category { Id = 3, Name = "Clothes" });
            db.Categories.Add(new Category { Id = 4, Name = "Sport" });

            var p1 = new Product { Id = 1, Name = "Laptop", Price = 35000, CategoryId = 1};
            var p2 = new Product { Id = 2, Name = "iPhone 6s", Price = 16000, CategoryId = 1};
            var p3 = new Product { Id = 3, Name = "Chips", Price = 30, CategoryId = 2};
            var p4 = new Product { Id = 4, Name = "Beer", Price = 20, CategoryId = 2};
            var p5 = new Product { Id = 5, Name = "Bicycle", Price = 40000, CategoryId = 4};
            var p6 = new Product { Id = 6, Name = "Dress", Price = 600, CategoryId = 3};
            var p7 = new Product { Id = 7, Name = "T-shirt", Price = 120, CategoryId = 3};

            db.Products.Add(p1);
            db.Products.Add(p2);
            db.Products.Add(p3);
            db.Products.Add(p4);
            db.Products.Add(p5);
            db.Products.Add(p6);
            db.Products.Add(p7);

            var pr1 = new Provider { Id = 1, Name = "Apple", Location = "USA", Products = new List<Product> { p1, p2 } };
            var pr2 = new Provider { Id = 2, Name = "BMW", Location = "Germany", Products = new List<Product> { p5, p7 } };
            var pr3 = new Provider { Id = 3, Name = "Lays", Location = "Ukraine", Products = new List<Product> { p3} };
            var pr4 = new Provider { Id = 4, Name = "OJJI", Location = "Russia", Products = new List<Product> { p6, p7 } };
            var pr5 = new Provider { Id = 5, Name = "Baltyka", Location = "Ukraine", Products = new List<Product> { p4 } };

            db.Providers.Add(pr1);
            db.Providers.Add(pr2);
            db.Providers.Add(pr3);
            db.Providers.Add(pr4);
            db.Providers.Add(pr5);
            
            db.SaveChanges();
        }
    }
}

