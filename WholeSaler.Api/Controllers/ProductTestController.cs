using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Business.AbstractServices;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTestController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductTestController(IProductService productService)
        {
           _productService = productService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var prd = await _productService.GetById(id);
            return Ok(prd);
        }
    }
}
