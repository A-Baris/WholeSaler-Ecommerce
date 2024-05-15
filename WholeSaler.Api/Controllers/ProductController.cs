using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Api.DTOs.ProductDTOs;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.Embeds.Product;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServiceWithRedis _productService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private string controllerName = "ProductController";

        public ProductController(IProductServiceWithRedis productService,IMapper mapper,ILogger<ProductController> logger)
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
                return Ok(products);
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
        [Authorize(Roles= "assistant")]
      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            try
            {
                var product = await _productService.GetById(id);
                return product!=null ? Ok(product) : NotFound();
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

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDTO product)
        {
            try
            {
                var newProduct = _mapper.Map<Product>(product);
                await _productService.Create(newProduct);
                return Ok(product);
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
        public async Task<IActionResult> Update(Product product)
        {

            await _productService.Update(product.Id, product);
            return Ok("Updated");


        }

        [HttpGet("changeStatus/{id}/{statusCode}")]
        public async Task<IActionResult> ChangeStatus(string id, int statusCode)
        {
            var result =await _productService.ChangeStatusOfEntity(id, statusCode);
            return result?Ok(result):BadRequest(result);
        } 

        [HttpPut("AddComment/{productId}")]
        public async Task<IActionResult> Update(Comment comment,string productId)
        {
            var product = await _productService.GetById(productId);
           if(product.Comments ==null)
            {
                product.Comments = new List<Comment>();           
            }
            product.Comments.Add(comment);
            await _productService.Update(product.Id, product);
            return Ok("Updated");


        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productService.Delete(id);
            return Ok("Deleted");


        }
        
        
        [HttpGet("mystore/{storeId}")]
        public async Task<IActionResult> GetPrivateStoreWithProducts(string storeId)
        {
            var products = await _productService.GetTheStoreWithPRoducts(storeId);
            return Ok(products);

        }
        [HttpPut("updateStocks")]
        public async Task<IActionResult> UpdateStocks(List<Product> products)
        {
            int quantity = 0;
            foreach (var prd in products)
            {
                
                prd.Stock -= prd.Quantity;
                quantity = Convert.ToInt32(prd.Quantity);
                prd.SumOfSales = quantity+prd.SumOfSales;
                await _productService.Update(prd.Id, prd);
            }

            return Ok();
        }

    }
}
