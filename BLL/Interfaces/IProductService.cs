using System;
using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IShopService
    {
        void Dispose();
        IEnumerable<CategoryDTO> FindCategory(Func<CategoryDTO, bool> predicate);
        IEnumerable<ProductDTO> FindProduct(Func<ProductDTO, bool> predicate);
        IEnumerable<ProviderDTO> FindProvider(Func<ProviderDTO, bool> predicate);
        IEnumerable<ProviderDTO> GetByLocation(string location);
        IEnumerable<ProviderDTO> GetProvidersForCategory(int categoryId);
        IEnumerable<ProductDTO> GetProducts();
        IEnumerable<ProductDTO> GetProviderProducts(int providerId);
        IEnumerable<ProviderDTO> GetProviders();
    }
}