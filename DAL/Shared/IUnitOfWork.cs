using DAL.Shared;
using DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAL.EF.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<Provider> Providers { get; }
        IRepository<Category> Categories { get; }
        void Save();
    }
}
