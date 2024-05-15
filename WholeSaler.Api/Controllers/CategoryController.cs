using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.Logger;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServiceWithRedis _categoryWithRedis;
        private readonly ILogger<CategoryController> _logger;
        private string controllerName = "CategoryController";

        public CategoryController(ICategoryServiceWithRedis categoryWithRedis,ILogger<CategoryController> logger)
        {
           _categoryWithRedis = categoryWithRedis;
            _logger = logger;
        }
      
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _categoryWithRedis.GetAll();
                return Ok(categories);

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
                var category = await _categoryWithRedis.GetById(id);
                return category != null ? Ok(category):NotFound(id);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} GetOne - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Get [Id]:{id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }

        }
      
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {

            try
            {
                var newEntity = await _categoryWithRedis.Create(category);
                return newEntity != null ? Ok(newEntity) : BadRequest();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Create - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Create - {ex.Message} ", ex);
                return StatusCode(500, ex.Message);

            }



        }
        [HttpPut]
        public async Task<IActionResult> Update(Category category)
        {
            try
            {
                var updatedEntity = await _categoryWithRedis.Update(category.Id, category);
                return updatedEntity!=null ? Ok(updatedEntity) : BadRequest();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Update - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Update [Id]: {category.Id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }



        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _categoryWithRedis.Delete(id);
                return result?Ok():NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Delete - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Delete [Id]:{id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }


        }
    }
}
