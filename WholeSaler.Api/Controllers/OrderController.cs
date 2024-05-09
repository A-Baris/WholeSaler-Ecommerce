using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServiceWithRedis _orderServiceWithRedis;
        private readonly IShoppingCartServiceWithRedis _shoppingCartServiceWithRedis;

        public OrderController(IOrderServiceWithRedis orderServiceWithRedis,IShoppingCartServiceWithRedis shoppingCartServiceWithRedis)
        {
            _orderServiceWithRedis = orderServiceWithRedis;
            _shoppingCartServiceWithRedis = shoppingCartServiceWithRedis;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string Id)
        {
            var result = await _orderServiceWithRedis.GetById(Id);
            return result != null ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {

            var result = await _orderServiceWithRedis.Create(order);
            if(result != null)
            {
                await _shoppingCartServiceWithRedis.SetPassiveStatusOfShoppingCart(order.ShoppingCartId);
                return Ok(result);
            }
            return BadRequest();
           


        }
        [HttpGet("PrivateUserOrders/{userId}")]
        public async Task<IActionResult> GetPrivateUserOrders(string userId)
        {
            var getOrders = await _orderServiceWithRedis.GetAll();
            var privateUserOrders = getOrders.Where(x=>x.UserId== userId).ToList().OrderByDescending(x=>x.CreatedDate);
            return privateUserOrders != null ? Ok(privateUserOrders) : BadRequest(privateUserOrders);

        }
    }
}
