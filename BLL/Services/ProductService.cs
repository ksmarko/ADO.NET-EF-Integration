using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.DAL.EF.Interfaces;
using DAL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductService : IService
    {
        IUnitOfWork Database { get; set; }

        public ProductService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            return Mapper.Map<IEnumerable<Product>, List<ProductDTO>>(Database.Products.GetAll());
        }

        public IEnumerable<ProductDTO> GetProviderProducts(ProviderDTO provider)
        {
            return GetAll().Where(x => x.Providers.Contains(provider));
        }

        public IEnumerable<ProviderDTO> GetCategoryProviders(CategoryDTO category)
        {
            var products = GetAll().Where(x => x.Category == category);
            List<ProviderDTO> providers = new List<ProviderDTO>();

            foreach (var el in products)
                foreach (var el2 in el.Providers)
                    providers.Add(el2);

            return providers;
        }

        public IEnumerable<ProductDTO> Find(Func<ProductDTO, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IEnumerable<ProviderDTO> Find(Func<ProviderDTO, bool> predicate)
        {
            return Mapper.Map<IEnumerable<Provider>, List<ProviderDTO>>(Database.Providers.GetAll()).Where(predicate);
        }

        public IEnumerable<CategoryDTO> Find(Func<CategoryDTO, bool> predicate)
        {
            return Mapper.Map<IEnumerable<Category>, List<CategoryDTO>>(Database.Categories.GetAll()).Where(predicate);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
