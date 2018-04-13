using DAL.DAL.EF.Interfaces;
using DAL.Shared;
using DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAL.ADO.Repositories
{
    public class ProviderRepository : IRepository<Provider>
    {
        private ADOUnitOfWork db;

        public ProviderRepository(IUnitOfWork uow)
        {
            if (uow == null)
                throw new ArgumentNullException("uow");

            db = uow as ADOUnitOfWork;
            if (db == null)
                throw new NotSupportedException("Ohh my, change that UnitOfWorkFactory, will you?");
        }

        public void Create(Provider item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Provider> Find(Func<Provider, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Provider Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Provider> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Provider item)
        {
            throw new NotImplementedException();
        }
    }
}
