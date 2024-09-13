using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Api.Controllers.Base;
using WholeSaler.Api.DTOs.Store;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.ConcreteServices;
using WholeSaler.Entity.Entities;
using WholeSaler.Entity.Entities.MongoIdentity;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : BaseController
    {
        private readonly IStoreServiceWithRedis _storeService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ILogger<StoreController> _logger;
        private readonly IMapper _mapper;
        private const string controllerName = "StoreController";

        public StoreController(IStoreServiceWithRedis storeService,UserManager<AppUser> userManager,RoleManager<AppRole> roleManager,ILogger<StoreController> logger,IMapper mapper)
        {
            _storeService = storeService;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
           _mapper = mapper;
        }
        [HttpGet]
      
        public async Task<IActionResult> GetAll()
        {

            var stores = await _storeService.GetAll();
            return Ok(stores);


        }
        [HttpGet("{id}")]
   
        public async Task<IActionResult> GetOne(string id)
        {
            try
            {
                return await ValidateAndExecute(id,
                    async (storeId) => await _storeService.GetById(storeId),
                    store =>
                    {
                        var storeDto = _mapper.Map<StoreDto>(store);
                        return Ok(storeDto);
                    });

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} GetOne Id: {id} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} GetOne Id: {id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }

        }
        [HttpGet("mystore/{userId}")]

        public async Task<IActionResult> GetUserStore(string userId)
        {
            try
            {
               if(userId == null) { return BadRequest("UserId can not be null"); }
                var stores = await _storeService.GetAll();
                var userStore = stores.Where(x=>x.UserId == userId).FirstOrDefault();
                var storeDto = _mapper.Map<StoreDto>(userStore);
                if (userStore != null) { return Ok(storeDto); }
                else { return NotFound(null); }


            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} GetUserStore UserId: {userId} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} GetUserStore UserId: {userId} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }


        }


        [HttpPost]
        public async Task<IActionResult> Create(Store store)
        {
            try
            {
                return await ValidateAndExecute(store,
                    async(str)=>await _storeService.Create(str),
                    result =>
                    {
                        var storeDto = _mapper.Map<StoreDto>(result);
                        return result != null ? Ok(storeDto) : StatusCode(500);
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
        [HttpPut]
        public async Task<IActionResult> Update(Store store)
        {
            try
            {
                return await ValidateAndExecute(store,
                    async (updatedStore) => await _storeService.Update(updatedStore.Id, updatedStore),
                    result =>
                    {
                        var storeDto=_mapper.Map<StoreDto>(result);
                        return Ok(storeDto);
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Update Id:{store.Id} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Update Id:{store.Id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }

          


        }
        [Authorize(Roles ="admin")]
        [HttpGet("ApproveApplication/{storeId}")]
        public async Task<IActionResult> ApproveApplication(string storeId)
        {
            try
            {
                return await ValidateAndExecute(storeId,
                    async (id) => await _storeService.GetById(id),
                    async result =>
                    {
                        var user = await _userManager.FindByIdAsync(result.UserId);
                        user.StoreId = result.Id;
                        var roles =  _roleManager.Roles.ToList();
                        var authRole = roles.Where(x => x.Name == "storeManager").FirstOrDefault();
                        user.AddRole(authRole.Id);
                        await _userManager.UpdateAsync(user);
                        result.AdminConfirmation=Entity.Entities.Enums.AdminConfirmation.Accepted;
                        var updatedStore = await _storeService.Update(result.Id, result);
                        var storeDto=_mapper.Map<StoreDto>(updatedStore);
                        return updatedStore!=null? Ok(storeDto):NotFound(storeId);
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} ApproveApplication Id:{storeId} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} ApproveApplication Id:{storeId} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }
          
        }
        [HttpGet("RejectApplication/{storeId}")]
        public async Task<IActionResult> RejectApplication(string storeId)
        {
            try
            {
                return await ValidateAndExecute(storeId,
                    async (id) => await _storeService.GetById(id),
                    async result =>
                    {
                        result.AdminConfirmation = Entity.Entities.Enums.AdminConfirmation.Rejected;
                        var updatedStore = await _storeService.Update(result.Id, result);
                        var storeDto = _mapper.Map<StoreDto>(updatedStore);
                        return updatedStore != null ? Ok(storeDto) : NotFound(storeId);
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} RejectApplication Id:{storeId} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} RejectApplication Id:{storeId} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }
   
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                return await ValidateAndExecute(id,
                    async (storeId) => await _storeService.Delete(id),
                    result =>
                    {
                        return result == true ? Ok(true) : BadRequest(false);
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Delete Id:{id} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Delete Id:{id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }



        }

      

    }
}
