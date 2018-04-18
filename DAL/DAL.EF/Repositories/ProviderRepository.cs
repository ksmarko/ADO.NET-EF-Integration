using DAL.DAL.EF.Context;
using DAL.Shared;
using DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL.DAL.EF.Repositories
{
    public class ProviderRepository : IRepository<Provider>
    {
        private EFDataContext db;

        public ProviderRepository(EFDataContext context)
        {
            this.db = context;
        }

        public void Create(Provider item)
        {
            db.Providers.Add(item);
        }

        public void Delete(int id)
        {
            Provider provider = db.Providers.Find(id);
            if (provider != null)
                db.Providers.Remove(provider);
        }

        public IEnumerable<Provider> Find(Func<Provider, bool> predicate)
        {
            return db.Providers.Where(predicate).ToList();
        }

        public Provider Get(int id)
        {
            return db.Providers.Find(id);
        }

        public IEnumerable<Provider> GetAll()
        {
            return db.Providers;
        }

        public void Update(Provider item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
