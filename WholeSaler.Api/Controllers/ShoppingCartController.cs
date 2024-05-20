using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Api.Controllers.Base;
using WholeSaler.Api.DTOs.ProductDTOs;
using WholeSaler.Api.DTOs.ShoppingCartDtos;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartServiceWithRedis _cartServiceWithRedis;
        private readonly IMapper _mapper;
        private readonly ILogger<ShoppingCartController> _logger;
        private const string controllerName = "ShoppingCartController";

        public ShoppingCartController(IShoppingCartServiceWithRedis cartServiceWithRedis,IMapper mapper,ILogger<ShoppingCartController> logger)
        {
            _cartServiceWithRedis = cartServiceWithRedis;
            _mapper = mapper;
           _logger = logger;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var entity = await _cartServiceWithRedis.GetById(id);
                var cartDto= _mapper.Map<ShoppingCartDto>(entity);
                return entity != null ? Ok(cartDto) : NotFound();

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} GetById - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Get [Id]:{id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }
            
           
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShoppingCart shoppingCart)
        {
            try
            {
                return await ValidateAndExecute(shoppingCart,
                    async (sc) => await _cartServiceWithRedis.Create(sc),
                    result =>
                    {
                        var cart = _mapper.Map<ShoppingCartDto>(result);
                        return result != null ? Ok(cart) : StatusCode(500);
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
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                return await ValidateAndExecute(id,
                    async (cartId) => await _cartServiceWithRedis.Delete(cartId),
                    result => 
                    {
                        return result == true ? Ok(result) : BadRequest(result);
                    });

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Delete Id: {id} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Delete Id: {id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }
         
        }
        [HttpDelete("deleteproduct/{id}")]
        public async Task<IActionResult> DeleteProductInCart(string id)
        {
            try
            {
                return await ValidateAndExecute(id,
                    async (prdId) => await _cartServiceWithRedis.DeleteProductInCart(prdId),
                    result =>
                    {
                        return result == true ? Ok(result) : BadRequest(result);
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} DeleteProductInCart Id: {id} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} DeleteProductInCart Id: {id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }

          
        }
    
        [HttpPut("edit")]
        public async Task<IActionResult> Edit(ShoppingCart shoppingCart)
        {
            try
            {
                return await ValidateAndExecute(shoppingCart,
                    async (sc) => await _cartServiceWithRedis.Update(sc.Id, sc),
                    updatedCart =>
                    {
                        var shoppingCart = _mapper.Map<ShoppingCartDto>(updatedCart);
                        return updatedCart != null ? Ok(shoppingCart) : StatusCode(500);
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} Edit Id: {shoppingCart.Id} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} Edit Id: {shoppingCart.Id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }

        }
       

        [HttpPut("EditInCart")]
        public async Task<IActionResult> EditInCart(ShoppingCartEditDto shoppingCart)
        {
            try
            {
                return await ValidateAndExecute(shoppingCart,
                    async (sc) => await _cartServiceWithRedis.GetById(sc.Id),
                    async existingCart =>
                    {
                        if (existingCart == null)
                        {
                            return BadRequest($"Cart-{shoppingCart.Id} is not found");
                        }
                        existingCart.UserId=shoppingCart.UserId;
                        foreach (var updatedProduct in shoppingCart.Products)
                        {
                           
                            var indexOfUpdatedProduct = existingCart.Products.FindIndex(p => p.Id == updatedProduct.Id);
                            if (indexOfUpdatedProduct == -1)
                            {
                                return BadRequest("There is no any product in cart");
                            }

                           
                            existingCart.Products[indexOfUpdatedProduct].Quantity = updatedProduct.Quantity;
                        }

                        var result = await _cartServiceWithRedis.Update(existingCart.Id, existingCart);
                        var cartDto = _mapper.Map<ShoppingCartDto>(result);
                        return result != null ? Ok(cartDto) : BadRequest(cartDto);
                    });

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} EditInCart Id: {shoppingCart.Id} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} EditInCart Id: {shoppingCart.Id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }
           
        }
        [HttpGet("getcart/{id}")]
        public async Task<IActionResult> GetCartForUser(string id)
        {
            try
            {
                return await ValidateAndExecute(id,
                    async(cartId)=>await _cartServiceWithRedis.GetCartForUser(cartId),
                    result =>
                    {
                        var cartDto = _mapper.Map<ShoppingCartDto>(result);
                        return result != null ? Ok(cartDto) : BadRequest(null);

                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} GetCartForUser Id: {id} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} GetCartForUser Id: {id} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }

        }
        [HttpGet("checkCartProduct/{cartId}/{productId}")]
        public async Task<IActionResult> CheckCartProduct(string cartId,string productId)
        {
            try
            {
                var model = (cartId:cartId, productId:productId);
                return await ValidateAndExecute(model,
                    async (cp) => await _cartServiceWithRedis.CheckProductInCart(cp.cartId, cp.productId),
                    result =>
                    {
                        var productDto = _mapper.Map<ProductDto>(result);
                        return result != null ? Ok(productDto) : BadRequest();
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} CheckCartProduct Id: {cartId} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} CheckCartProduct Id: {cartId} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }
           

        }
        [HttpDelete("deleteproductincart/{cartId}/{productId}")]
        public async Task<IActionResult> DeleteTheProductFromCart(string cartId,string productId)
        {
            try
            {

                var model = (cartId: cartId, productId: productId);
                return await ValidateAndExecute(model,
                    async (model) => await _cartServiceWithRedis.GetById(model.cartId),
                    async cart =>
                    {
                        if (cart == null) { return BadRequest(); }
                        else
                        {
                            foreach (var item in cart.Products)
                            {
                                if (productId == item.Id)
                                {
                                    cart.Products.Remove(item);
                                    await _cartServiceWithRedis.Update(cartId, cart);
                                }
                                if (cart.Products.Count < 1)
                                {
                                    await _cartServiceWithRedis.Delete(cartId);
                                }

                            }
                            return Ok(true);
                        }

                    });

            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"#{controllerName} DeleteTheProductFromCart Id: {cartId} - {ex.Message}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"#{controllerName} DeleteTheProductFromCart Id: {cartId} - {ex.Message}", ex);
                return StatusCode(500, ex.Message);

            }
   
        }
        [HttpGet("GetProductsInOrder/{Id}")]
        public async Task<IActionResult> GetProductsInOrder(string Id)
        {
            var products = await _cartServiceWithRedis.GetProductsInOrder(Id);
            return Ok(products);

        }

    }
}

