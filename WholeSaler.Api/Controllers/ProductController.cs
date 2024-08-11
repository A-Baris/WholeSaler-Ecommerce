using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using WholeSaler.Api.Controllers.Base;
using WholeSaler.Api.DTOs;
using WholeSaler.Api.DTOs.ProductDTOs;
using WholeSaler.Api.DTOs.ProductDTOs.FilterProducts;
using WholeSaler.Api.DTOs.ProductDTOs.TProducts;
using WholeSaler.Api.Helpers.ProductHelper;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Embeds.Product;
using WholeSaler.Entity.Entities.Products;
using WholeSaler.Entity.Entities.Products.Features;
using static MongoDB.Libmongocrypt.CryptContext;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductServiceWithRedis _productService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
      
        private string controllerName = "ProductController";

        public ProductController(IProductServiceWithRedis productService, IMapper mapper, ILogger<ProductController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
   
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAll();
                var settings = new JsonSerializerSettings
                {
                    //TypeNameHandling = TypeNameHandling.Auto,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var jsonResult = JsonConvert.SerializeObject(products, settings);

                return Ok(jsonResult);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} GetAll - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} GetAll - {ex.Message}", ex);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("getforfilter")]
        public async Task<IActionResult> GetProductsForFilter()
        {
            try
            {
                var products = await _productService.GetAll();
           

                var productGeneral = new ProductFilterDto();

                foreach (var product in products)
                {
                    switch (product.Type)
                    {
                        case "Television":
                            productGeneral.Televisions.Add(JsonConvert.DeserializeObject<Television>(JsonConvert.SerializeObject(product)));
                            break;
                        case "Laptop":
                            productGeneral.Laptops.Add(JsonConvert.DeserializeObject<Laptop>(JsonConvert.SerializeObject(product)));
                            break;
                        case "Perfume":
                            productGeneral.Perfumes.Add(JsonConvert.DeserializeObject<Perfume>(JsonConvert.SerializeObject(product)));
                            break;
                    }
                }

                if (products.Any())
                {
                    return Ok(productGeneral);
                }
                return Ok(null);

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} GetAll - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} GetAll - {ex.Message}", ex);
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            try
            {
                return await ValidateAndExecute(id,
                    async (prd) => await _productService.GetById(prd),
                    result =>
                    {

                        
                        return result != null ? Ok(result) : NotFound();
                    });



            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} GetOne - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} GetOne - {ex.Message}", ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ProductCreateDTO createDTO)
        {
            var createProduct = ProductHelper.CreateProductFromDto(createDTO);
          
            
            try
            {
                return await ValidateAndExecute(
           createProduct,
           async (prod) => await _productService.Create(prod),
           result =>
           {
               var productDto = _mapper.Map<ProductDto>(result);
               return Ok(productDto);
           });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Create - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Create - {ex.Message}", ex);
                return StatusCode(500, ex.Message);
            }


         

        }
        [HttpPut("edit")]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDto update)
        {
          
            try
            {
                var updatedEntity = ProductHelper.UpdateProductFromDto(update);
                return await ValidateAndExecute(updatedEntity,
                    async (prd) => await _productService.Update(prd.Id, prd),
                    result =>
                    {
                       
                        return result != null ? Ok(result) : BadRequest();
                    });

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Update - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Update - {ex.Message}", ex);
                return StatusCode(500, ex.Message);
            }


        }
     

        [HttpGet("changeStatus/{id}/{statusCode}")]
        public async Task<IActionResult> ChangeStatus(string id, int statusCode)
        {
            try
            {
                var model = (id: id, statusCode: statusCode);
                return await ValidateAndExecute(model,
                    async (prd) => await _productService.ChangeStatusOfEntity(prd.id,prd.statusCode),
                    result => 
                    {
                        if (result)
                        {
                            return Ok("Status changed successfully.");
                        }
                        else
                        {
                            return NotFound("Product not found or status could not be changed.");
                        }
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} ChangeStatus - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} ChangeStatus - {ex.Message}", ex);
                return StatusCode(500, ex.Message);
            }
           
        }

        [HttpPut("AddComment/{productId}")]
        public async Task<IActionResult> Update(Comment comment, string productId)
        {
            try
            {
                var model = (comment: comment, productId: productId);
                return await ValidateAndExecute(model,
                    async (cmt) => await _productService.GetById(productId),
                    async prd =>
                    {
                        if (prd.Comments == null)
                        {
                            prd.Comments = new List<Comment>();
                        }
                        prd.Comments.Add(comment);
                        var result = await _productService.Update(productId, prd);
                        return result != null ? Ok(result) : NotFound();

                    });

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} AddComment - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} AddComment - {ex.Message}", ex);
                return StatusCode(500, ex.Message);
            }
          

        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                return await ValidateAndExecute(id,
                async (prd) => await _productService.Delete(prd),
                result =>
                {
                    if (result)
                    {
                        return Ok("Deleted.");
                    }
                    else
                    {
                        return NotFound("Product not found or it could not be deleted.");
                    }

                });

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Delete - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Delete - {ex.Message}", ex);
                return StatusCode(500, ex.Message);
            }
        
        }


        [HttpGet("mystore/{storeId}")]
        public async Task<IActionResult> GetPrivateStoreWithProducts(string storeId)
        {
            var products = await _productService.GetTheStoreWithProducts(storeId);
            return Ok(products);

        }
        [HttpGet("productReview/{id}")]
        public async Task<IActionResult> GetProductReviewForStore(string id)
        {
            var product = await _productService.GetProductForReview(id);
            return Ok(product);
        }
       
        [HttpPut("updateStocks")]
        public async Task<IActionResult> UpdateStocks([FromBody] List<ProductDto> products)
        {

            int quantity = 0;
            foreach (var prd in products)
            {
                
                var editPrd = await _productService.GetById(prd.Id);
                editPrd.Stock -= prd.Quantity;
                quantity = Convert.ToInt32(prd.Quantity);
                editPrd.SumOfSales = quantity + editPrd.SumOfSales;
                await _productService.Update(prd.Id, editPrd);
            }

            return Ok();
        }

        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            var prd = await _productService.GetProduct(id);
            return Ok(prd);
        }

       
    }
}
