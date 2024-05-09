using AutoMapper;
using WholeSaler.Api.DTOs.ProductDTOs;
using WholeSaler.Api.DTOs.ProductDTOs.EmbedDTOs;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Embeds.Product;

namespace WholeSaler.Api.AutoMapper.Mapping
{
    public class MapProfile: Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryEmbedDTO>().ReverseMap();
            CreateMap<ProductStore, ProductStoreEmbedDTO>().ReverseMap();
         
        }
    }
}
