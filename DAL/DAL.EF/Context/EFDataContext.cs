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

    public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<EFDataContext>
    {
        protected override void Seed(EFDataContext db)
        {
            db.SaveChanges();
        }
    }
}

