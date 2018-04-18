using AutoMapper;
using BLL.DTO;
using DAL.Shared.Entities;

namespace BLL.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<Provider, ProviderDTO>();
                cfg.CreateMap<Category, CategoryDTO>();
            });
        }
    }
}
