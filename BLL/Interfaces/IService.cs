using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IService
    {
        IEnumerable<ProductDTO> GetAll();
        IEnumerable<ProductDTO> GetProviderProducts(ProviderDTO provider);
        IEnumerable<ProductDTO> Find(Func<ProductDTO, bool> predicate);

        IEnumerable<ProviderDTO> GetCategoryProviders(CategoryDTO category);
        IEnumerable<ProviderDTO> Find(Func<ProviderDTO, bool> predicate);

        IEnumerable<CategoryDTO> Find(Func<CategoryDTO, bool> predicate);

        void Dispose();
    }
}
