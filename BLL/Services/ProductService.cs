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
    public class ShopService : IShopService
    {
        IUnitOfWork Database { get; set; }

        public ShopService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            return Mapper.Map<IEnumerable<Product>, List<ProductDTO>>(Database.Products.GetAll());
        }

        public IEnumerable<ProductDTO> GetProviderProducts(int providerId)
        {
            var result = new List<ProductDTO>();

            foreach(var product in GetProducts())
                foreach (var provider in product.Providers)
                    if (provider.Id == providerId)
                        result.Add(product);

            return result;
        }

        public IEnumerable<ProductDTO> FindProduct(Func<ProductDTO, bool> predicate)
        {
            return GetProducts().Where(predicate);
        }
        
        public IEnumerable<ProviderDTO> GetProviders()
        {
            return Mapper.Map<IEnumerable<Provider>, List<ProviderDTO>>(Database.Providers.GetAll());
        }

        public IEnumerable<ProviderDTO> GetProvidersForCategory(int categoryId)
        {
            var products = Mapper.Map<IEnumerable<Product>, List<ProductDTO>>(Database.Products.GetAll());
            List<ProviderDTO> providers = new List<ProviderDTO>();

            foreach (var product in products)
                if (product.CategoryId == categoryId)
                    foreach (var provider in product.Providers)
                        if(!providers.Exists(x => x.Name == provider.Name))
                            providers.Add(provider);

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
