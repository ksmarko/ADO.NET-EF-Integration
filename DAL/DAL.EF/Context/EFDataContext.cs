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
            List<Provider> providers = new List<Provider>();
            providers.Add(new Provider { Id = 1, Name = "Apple", Location = "USA" });
            providers.Add(new Provider { Id = 2, Name = "BMW", Location = "Germany" });
            providers.Add(new Provider { Id = 3, Name = "Lays", Location = "Ukraine" });
            providers.Add(new Provider { Id = 4, Name = "OJJI", Location = "Russia" });
            providers.Add(new Provider { Id = 5, Name = "Baltyka", Location = "Ukraine" });

            db.Products.Add(new Product { Id = 1, Name = "Laptop", Price = 35000, CategoryId = 1, Providers = providers.Where( x=> x.Id == 1 || x.Id == 2).ToList() });
            db.Products.Add(new Product { Id = 2, Name = "iPhone 6s", Price = 16000, CategoryId = 1, Providers = providers.Where(x => x.Id == 1 || x.Id == 2).ToList() });
            db.Products.Add(new Product { Id = 3, Name = "Chips", Price = 30, CategoryId = 2, Providers = providers.Where(x => x.Id == 3).ToList() });
            db.Products.Add(new Product { Id = 4, Name = "Beer", Price = 20, CategoryId = 2, Providers = providers.Where(x => x.Id == 5).ToList() });
            db.Products.Add(new Product { Id = 5, Name = "Bicycle", Price = 40000, CategoryId = 4, Providers = providers.Where(x => x.Id == 2).ToList() });
            db.Products.Add(new Product { Id = 6, Name = "Dress", Price = 600, CategoryId = 3, Providers = providers.Where(x => x.Id == 4).ToList() });
            db.Products.Add(new Product { Id = 7, Name = "T-shirt", Price = 120, CategoryId = 3, Providers = providers.Where(x => x.Id == 4).ToList() });
            
            db.Categories.Add(new Category { Id = 1, Name = "Electronics" });
            db.Categories.Add(new Category { Id = 2, Name = "Food" });
            db.Categories.Add(new Category { Id = 3, Name = "Clothes" });
            db.Categories.Add(new Category { Id = 4, Name = "Sport" });
            
            db.SaveChanges();
        }
    }
}

