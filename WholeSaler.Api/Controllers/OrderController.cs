using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Globalization;
using WholeSaler.Api.Controllers.Base;
using WholeSaler.Api.DTOs.Order;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderServiceWithRedis _orderServiceWithRedis;
        private readonly IShoppingCartServiceWithRedis _shoppingCartServiceWithRedis;
        private readonly ILogger<Order> _logger;
        private readonly IMapper _mapper;
        private string controllerName = "OrderController";

        public OrderController(IOrderServiceWithRedis orderServiceWithRedis, IShoppingCartServiceWithRedis shoppingCartServiceWithRedis, ILogger<Order> logger,IMapper mapper)
        {
            _orderServiceWithRedis = orderServiceWithRedis;
            _shoppingCartServiceWithRedis = shoppingCartServiceWithRedis;
            _logger = logger;
            _mapper = mapper;
        }
        [Authorize(Roles ="admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var orders = await _orderServiceWithRedis.GetAll();
                return Ok(orders);
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
        [Authorize(Roles ="admin")]
        [HttpPut("edit")]
        public async Task<IActionResult> Update(Order order)
        {
            try
            {
                return await ValidateAndExecute(order,
                    async (ord) => await _orderServiceWithRedis.Update(ord.Id, ord),
                    result =>
                    {
                        var updatedOrder = _mapper.Map<OrderDto>(result);
                        return result != null ? Ok(updatedOrder) : BadRequest();

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                return await ValidateAndExecute(id,
                    async (ord) => await _orderServiceWithRedis.GetById(ord),
                    result =>
                    {
                        return result != null ? Ok(result) : NotFound();
                    });

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} GetById - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} GetById - {ex.Message}", ex);
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            try
            {
                return await ValidateAndExecute(order.ShoppingCartId,
                    async (sc) => await _shoppingCartServiceWithRedis.GetById(sc),
                    async shoppingCart =>

                            {
                                if (shoppingCart != null)
                                {
                                    order.Products = new List<Product>();
                                    order.Products.AddRange(shoppingCart.Products);
                                   var newOrder = await _orderServiceWithRedis.Create(order);
                                   if(newOrder != null)
                                    {
                                        await _shoppingCartServiceWithRedis.SetPassiveStatusOfShoppingCart(order.ShoppingCartId);
                                        var orderDto = _mapper.Map<Order>(newOrder);
                                        return Ok(orderDto);
                                    }
                                }
                                return NotFound();
                            }

                );

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
        [HttpGet("PrivateUserOrders/{userId}")]
        public async Task<IActionResult> GetPrivateUserOrders(string userId)
        {
            try
            {
                return await ValidateAndExecute(userId,
                    async(ord)=>await _orderServiceWithRedis.GetAll(),
                    result =>
                    {
                        var userOrders = result.Where(x=>x.UserId == userId).ToList().OrderByDescending(x=>x.CreatedDate);
                        return userOrders != null ? Ok(userOrders) : NotFound(userOrders);
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} GetPrivateUserOrders - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} GetPrivateUserOrders - {ex.Message}", ex);
                return StatusCode(500, ex.Message);
            }
            
        }
        [HttpGet("SalesReport/{storeId}/{startDate}/{endDate}")]
        public async Task<IActionResult> CalculateDailySalesPriceByStoreId(string storeId, DateTime startDate, DateTime endDate)
        {
            try
            {
               var tModel = (storeId:storeId, startDate:startDate, endDate:endDate);

                return await ValidateAndExecute(tModel,
                    async (sse) => await _orderServiceWithRedis.GetSalesReport(sse.storeId,sse.startDate,sse.endDate),
                    report =>
                    {
                        return report!=null ? Ok(report) : NotFound(report);
                    }
                    );

                
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} CalculateDailySalesPriceByStoreId - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} CalculateDailySalesPriceByStoreId - {ex.Message}", ex);
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
                    async (ord) => await _orderServiceWithRedis.ChangeStatusOfEntity(ord.id, ord.statusCode),
                    result =>
                    {
                        if (result)
                        {
                            return Ok("Status changed successfully.");
                        }
                        else
                        {
                            return NotFound("Order is not found or status could not be changed.");
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

    }
}
