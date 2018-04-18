using DAL.DAL.EF.Context;
using DAL.DAL.EF.Interfaces;
using DAL.Shared;
using DAL.Shared.Entities;
using System;

namespace DAL.DAL.EF.Repositories
{
    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        private EFDataContext db;
        private ProductRepository productRepository;
        private ProviderRepository providerRepository;
        private CategoryRepository categoryRepository;

        public EFUnitOfWork(string connectionString)
        {
            db = new EFDataContext(connectionString);
        }

        public IRepository<Product> Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(db);
                return productRepository;
            }
        }

        public IRepository<Provider> Providers
        {
            get
            {
                if (providerRepository == null)
                    providerRepository = new ProviderRepository(db);
                return providerRepository;
            }
        }

        public IRepository<Category> Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new CategoryRepository(db);
                return categoryRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
