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
    public class ProductService : IProductService
    {
        IUnitOfWork Database { get; set; }

        public ProductService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            return Mapper.Map<IEnumerable<Product>, List<ProductDTO>>(Database.Products.GetAll());
        }

        public IEnumerable<ProductDTO> GetProviderProducts(ProviderDTO provider)
        {
            return GetProducts().Where(x => x.Providers.Contains(provider));
        }

        public IEnumerable<ProductDTO> FindProduct(Func<ProductDTO, bool> predicate)
        {
            return GetProducts().Where(predicate);
        }
        
        public IEnumerable<ProviderDTO> GetProviders()
        {
            return Mapper.Map<IEnumerable<Provider>, List<ProviderDTO>>(Database.Providers.GetAll());
        }

        public IEnumerable<ProviderDTO> GetProvidersForCategory(CategoryDTO category)
        {
            var products = Mapper.Map<IEnumerable<Product>, List<ProductDTO>>(Database.Products.GetAll()).Where(x => x.Category == category);
            List<ProviderDTO> providers = new List<ProviderDTO>();

            foreach (var el in products)
                foreach (var el2 in el.Providers)
                    providers.Add(el2);

            return providers;
        }

        public IEnumerable<ProviderDTO> FindProvider(Func<ProviderDTO, bool> predicate)
        {
            return GetProviders().Where(predicate);
        }

        public IEnumerable<ProviderDTO> GetByLocation(string location)
        {
            return GetProviders().Where(x => x.Location.Contains(location));
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<CategoryDTO> FindCategory(Func<CategoryDTO, bool> predicate)
        {
            return Mapper.Map<IEnumerable<Category>, List<CategoryDTO>>(Database.Categories.GetAll()).Where(predicate);
        }
    }
}
