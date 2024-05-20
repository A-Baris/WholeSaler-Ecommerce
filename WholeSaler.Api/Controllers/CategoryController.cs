using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Api.Controllers.Base;
using WholeSaler.Api.DTOs.Category;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.Logger;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CategoryController : BaseController
    {
        private readonly ICategoryServiceWithRedis _categoryWithRedis;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;
        private string controllerName = "CategoryController";

        public CategoryController(ICategoryServiceWithRedis categoryWithRedis,ILogger<CategoryController> logger,IMapper mapper)
        {
           _categoryWithRedis = categoryWithRedis;
            _logger = logger;
            _mapper = mapper;
        }
      
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _categoryWithRedis.GetAll();
                if (categories == null) return NotFound();
                var categoryList = _mapper.Map<List<CategoryDto>>(categories);
                
                return Ok(categoryList);

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
                    async (ctgr) => await _categoryWithRedis.GetById(ctgr),
                    result =>
                    {
                        return result!=null ? Ok(result) : NotFound();
                    });
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
                return await ValidateAndExecute(category,
                    async (ctgr) => await _categoryWithRedis.Create(ctgr),
                    result =>
                    {

                        var categoryDto = _mapper.Map<CategoryDto>(result);
                        return categoryDto != null ? Ok(categoryDto) : BadRequest();
                    });
                
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
                return await ValidateAndExecute(category,
                    async (ctgr) => await _categoryWithRedis.Update(ctgr.Id, ctgr),
                    result =>
                    {
                        var categoryDto = _mapper.Map<CategoryDto>(result);
                        return categoryDto != null ? Ok(categoryDto) : BadRequest();
                    });
                
             
               
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
                return await ValidateAndExecute(id,
                    async (ctgr) => await _categoryWithRedis.Delete(ctgr),
                    result =>
                    {
                        return result ? Ok() : NotFound();
                    });
               
              
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
