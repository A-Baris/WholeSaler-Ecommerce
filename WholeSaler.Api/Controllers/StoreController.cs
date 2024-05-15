using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Business.ConcreteServices;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreServiceWithRedis _storeService;

        public StoreController(IStoreServiceWithRedis storeService)
        {
            _storeService = storeService;
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

            var store = await _storeService.GetById(id);
            return Ok(store);

        }
        [HttpGet("mystore/{userId}")]

        public async Task<IActionResult> GetUserStore(string userId)
        {

            var store = await _storeService.GetAll();
            var userStore = store.Where(x=>x.UserId == userId).FirstOrDefault();
            return Ok(userStore);

        }


        [HttpPost]
        public async Task<IActionResult> Create(Store store)
        {


            await _storeService.Create(store);
            return Ok(store);


        }
        [HttpPut]
        public async Task<IActionResult> Update(Store store)
        {

            await _storeService.Update(store.Id, store);
            return Ok("Updated");


        }
        [HttpGet("ApproveApplication/{storeId}")]
        public async Task<IActionResult> ApproveApplication(string storeId)
        {
            var store = await _storeService.GetById(storeId);
            store.AdminConfirmation = Entity.Entities.Enums.AdminConfirmation.Accepted;
            var edit = await _storeService.Update(storeId, store);
            return edit!=null?Ok():BadRequest();            
        }
        [HttpGet("RejectApplication/{storeId}")]
        public async Task<IActionResult> RejectApplication(string storeId)
        {
            var store = await _storeService.GetById(storeId);
            store.AdminConfirmation = Entity.Entities.Enums.AdminConfirmation.Rejected;
            var edit = await _storeService.Update(storeId, store);
            return edit != null ? Ok() : BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _storeService.Delete(id);
            return Ok("Deleted");


        }

      

    }
}
