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

        public ProductController(IProductServiceWithRedis productService,IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
       
     
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {

            var products = await _productService.GetAll();
            return Ok(products);


        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {

            var product = await _productService.GetById(id);
            return Ok(product);

        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDTO product )
        {

           var newProduct = _mapper.Map<Product>(product);
            await _productService.Create(newProduct);
            return Ok(product);


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
