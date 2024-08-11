using Newtonsoft.Json.Linq;
using WholeSaler.Api.DTOs.ProductDTOs;
using WholeSaler.Api.DTOs.ProductDTOs.TProducts;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Embeds.Product;
using WholeSaler.Entity.Entities.Products;

namespace WholeSaler.Api.Helpers.ProductHelper
{
    public class ProductHelper
    {
        public static Product CreateProductFromDto(ProductCreateDTO dto)
        {
            if (dto.Laptop!=null)
            {
                var laptop = new Laptop();
                laptop = dto.Laptop;
                return laptop;
            }
            if (dto.Television != null)
            {
               var tv = new Television();
                tv = dto.Television;
                return tv;
            }
            if (dto.Perfume != null)
            {
               var perfume = new Perfume();
                perfume = dto.Perfume;
                return perfume;
            }


            throw new ArgumentException("Invalid product type.");
        }
        
        public static Product GetProduct(GetOneProductDto entity)
        {
            if (entity.Category.SubCategory.Name == "Laptop")
            {
                var laptop = new Laptop();
                laptop = entity.Laptop;
                return laptop;
            }
            if (entity.Category.SubCategory.Name == "Television")
            {
                var tv = new Television();
                tv = entity.Television;
                return tv;
    //            return new Television
    //            {
    //                Id = entity.Id,
    //                Name = entity.Name,
    //                Color = entity.Color,
    //                Brand = entity.Brand,
    //                Quantity = entity.Quantity,
    //                UnitPrice = entity.UnitPrice,
    //                Stock = entity.Stock,
    //                SumOfSales = entity.SumOfSales,
    //                Comments = entity.Comments,
    //                Description = entity.Description,
    //                Images = entity.Images,
    //                Category = entity.Category,
    //                Store = entity.Store,
    //                OperatingSystem = entity.Television.OperatingSystem,
    //                Inc = entity.Television.Inc,
    //                DisplayTech= entity.Television.DisplayTech,
    //                Resolution = entity.Television.Resolution,
    //                RefreshRate = entity.Television.RefreshRate,
    //                YearOfProduction = entity.Television.YearOfProduction,
    //                SateliteReceiver= entity.Television.SateliteReceiver,
    //                SmartTv = entity.Television.SmartTv,
    //                Wifi = entity.Television.Wifi,
    //                Hdr= entity.Television.Hdr,

      


    //};
            }
            if (entity.Category.SubCategory.Name =="Perfume")
            {
                var perfume = new Perfume();
                perfume = entity.Perfume;
                return perfume;
            }


            throw new ArgumentException("Invalid product type.");
        }
        public static Product UpdateProductFromDto(ProductUpdateDto dto)
        {
            if (dto.Laptop != null)
            {
                var laptop = new Laptop();
                laptop = dto.Laptop;
                return laptop;
            }
            if (dto.Television != null)
            {
                var tv = new Television();
                tv = dto.Television;
                return tv;
            }
            if (dto.Perfume != null)
            {
                var perfume = new Perfume();
                perfume = dto.Perfume;
                return perfume;
            }


            throw new ArgumentException("Invalid product type.");
        }
    }
}
