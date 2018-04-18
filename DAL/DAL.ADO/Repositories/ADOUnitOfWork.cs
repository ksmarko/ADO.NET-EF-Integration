using DAL.DAL.ADO.Context;
using DAL.DAL.EF.Interfaces;
using DAL.Shared;
using DAL.Shared.Entities;
using System;

namespace DAL.DAL.ADO.Repositories
{
    public class ADOUnitOfWork : IUnitOfWork
    {
        private ADODataContext _context;

        public IRepository<Product> Products { get; private set; }
        public IRepository<Category> Categories { get; private set; }
        public IRepository<Provider> Providers { get; private set; }

        public ADOUnitOfWork(string connectionString)
        {
            _context = new ADODataContext(connectionString);

            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
            Providers = new ProviderRepository(_context);
        }

        public void Save() => _context.SaveChanges();

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
