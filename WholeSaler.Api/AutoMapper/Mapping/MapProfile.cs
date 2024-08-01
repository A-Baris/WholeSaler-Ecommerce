using AutoMapper;
using WholeSaler.Api.DTOs;
using WholeSaler.Api.DTOs.Category;
using WholeSaler.Api.DTOs.Order;
using WholeSaler.Api.DTOs.ProductDTOs;
using WholeSaler.Api.DTOs.ProductDTOs.EmbedDTOs;
using WholeSaler.Api.DTOs.ShoppingCartDtos;
using WholeSaler.Api.DTOs.Store;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Embeds.Category;
using WholeSaler.Entity.Entities.Embeds.Product;
using WholeSaler.Entity.Entities.Products.Features;

namespace WholeSaler.Api.AutoMapper.Mapping
{
    public class MapProfile: Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryEmbedDTO>().ReverseMap();
            CreateMap<ProductSubCategory, SubCategoryDTo>().ReverseMap();
            CreateMap<ProductStore, ProductStoreEmbedDTO>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<SubCategory,SubCategoryDTo>().ReverseMap();
         
           

            CreateMap<ShoppingCart, ShoppingCartEditDto>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<Store,StoreDto>().ReverseMap();

   
          


        }
    }
}
