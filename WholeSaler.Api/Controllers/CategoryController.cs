using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServiceWithRedis _categoryWithRedis;
        

        public CategoryController(ICategoryServiceWithRedis categoryWithRedis)
        {
           _categoryWithRedis = categoryWithRedis;
         
        }
      
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
         
                var categories = await _categoryWithRedis.GetAll();
                return Ok(categories);
            
          
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
          
                var category = await _categoryWithRedis.GetById(id);
                return Ok(category);
          
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
          
           
            await _categoryWithRedis.Create(category);
            return Ok(category);
            
          
        }
        [HttpPut]
        public async Task<IActionResult> Update(Category category)
        {
          
                await _categoryWithRedis.Update(category.Id,category);
                return Ok("Updated");
            
         
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _categoryWithRedis.Delete(id);
                return Ok("Deleted");
            
         
        }
    }
}
