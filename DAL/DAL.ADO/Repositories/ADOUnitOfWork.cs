using DAL.DAL.ADO.Context;
using DAL.DAL.EF.Interfaces;
using DAL.Shared;
using DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAL.ADO.Repositories
{
    public class ADOUnitOfWork : IUnitOfWork
    {
        private ProductRepository productRepository;
        private ProviderRepository providerRepository;
        private CategoryRepository categoryRepository;

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public ADOUnitOfWork(IDbConnection connection)
        {
            _connection = connection;
            _transaction = connection.BeginTransaction();
        }

        public IRepository<Product> Products
        {
            get
            {
                using (var uow = ADODataContext.Create(_connection.ConnectionString))
                {
                    if (productRepository == null)
                        productRepository = new ProductRepository(uow);
                    return productRepository;
                }
            }
        }

        public IRepository<Provider> Providers
        {
            get
            {
                using (var uow = ADODataContext.Create(_connection.ConnectionString))
                {
                    if (providerRepository == null)
                        providerRepository = new ProviderRepository(uow);
                    return providerRepository;
                }
            }
        }

        public IRepository<Category> Categories
        {
            get
            {
                using (var uow = ADODataContext.Create(_connection.ConnectionString))
                {
                    if (categoryRepository == null)
                        categoryRepository = new CategoryRepository(uow);
                    return categoryRepository;
                }
            }
        }

        public void Save()
        {
            if (_transaction == null)
                throw new InvalidOperationException
                 ("Transaction have already been committed. Check your transaction handling.");

            _transaction.Commit();
            _transaction = null;
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}
