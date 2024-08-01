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
                return new Laptop
                {
                    Name = dto.Name,
                    Type = dto.Type,
                    Color = dto.Color,
                    Brand = dto.Brand,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    Stock = dto.Stock,
                    Description = dto.Description,
                    Images = dto.Images,
                    Category = dto.Category,
                    Store = dto.Store,
                    RAM = dto.Laptop.RAM,
                    Processor = dto.Laptop.Processor,

                };
            }
            if (dto.Television != null)
            {
                return new Television
                {
                    Name = dto.Name,
                    Type = dto.Type,
                    Color = dto.Color,
                    Brand = dto.Brand,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    Stock = dto.Stock,
                    Description = dto.Description,
                    Images = dto.Images,
                    Category = dto.Category,
                    Store = dto.Store,
                    Inc = dto.Television.Inc,
                    Os = dto.Television.Os,

                };
            }


            throw new ArgumentException("Invalid product type.");
        }
        
        public static Product GetProduct(GetOneProductDto entity)
        {
            if (entity.Category.SubCategory.Name == "Laptop")
            {
                return new Laptop
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Color = entity.Color,
                    Brand = entity.Brand,
                    Quantity = entity.Quantity,
                    UnitPrice = entity.UnitPrice,
                    Stock = entity.Stock,
                    SumOfSales = entity.SumOfSales,
                    Comments = entity.Comments,
                    Description = entity.Description,
                    Images = entity.Images,
                    Category = entity.Category,
                    Store = entity.Store,
                    RAM = entity.Laptop.RAM,
                    Processor = entity.Laptop.Processor,


                };
            }
            if (entity.Category.SubCategory.Name == "Television")
            {
                return new Television
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Color = entity.Color,
                    Brand = entity.Brand,
                    Quantity = entity.Quantity,
                    UnitPrice = entity.UnitPrice,
                    Stock = entity.Stock,
                    SumOfSales = entity.SumOfSales,
                    Comments = entity.Comments,
                    Description = entity.Description,
                    Images = entity.Images,
                    Category = entity.Category,
                    Store = entity.Store,
                    Os = entity.Television.Os,
                    Inc = entity.Television.Inc


                };
            }
            throw new ArgumentException("Invalid product type.");
        }
        public static Product UpdateProductFromDto(ProductUpdateDto dto)
        {
            if (dto.Laptop != null)
            {
                return new Laptop
                {
                    Id = dto.Laptop.Id,
                    Name = dto.Laptop.Name,
                    Color = dto.Laptop.Color,
                    Brand = dto.Laptop.Brand,
                    Quantity = dto.Laptop.Quantity,
                    UnitPrice = dto.Laptop.UnitPrice,
                    Stock = dto.Laptop.Stock,
                    SumOfSales = dto.Laptop.SumOfSales,
                    Description = dto.Laptop.Description,
                    Images = dto.Laptop.Images,
                    Category = dto.Laptop.Category,
                    Store = dto.Laptop.Store,
                    RAM = dto.Laptop.RAM,
                    Processor = dto.Laptop.Processor,

                };
            }
            if (dto.Television != null)
            {
                return new Television
                {
                    Id = dto.Television.Id,
                    Type = dto.Television.Type,
                    Name = dto.Television.Name,
                    Color = dto.Television.Color,
                    Brand = dto.Television.Brand,
                    Quantity = dto.Television.Quantity,
                    UnitPrice = dto.Television.UnitPrice,
                    Stock = dto.Television.Stock,
                    SumOfSales = dto.Television.SumOfSales,
                    Description = dto.Television.Description,
                    Images = dto.Television.Images,
                    Category = dto.Television.Category,
                    Store = dto.Television.Store,
                    Inc = dto.Television.Inc,
                    Os = dto.Television.Os,

                };
            }


            throw new ArgumentException("Invalid product type.");
        }
    }
}
