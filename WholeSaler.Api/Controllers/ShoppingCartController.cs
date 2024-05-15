using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Api.DTOs.ShoppingCartDtos;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartServiceWithRedis _cartServiceWithRedis;
        private readonly IMapper _mapper;

        public ShoppingCartController(IShoppingCartServiceWithRedis cartServiceWithRedis,IMapper mapper)
        {
            _cartServiceWithRedis = cartServiceWithRedis;
            _mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var shoppingCart = await _cartServiceWithRedis.GetById(id);
            return shoppingCart!=null ? Ok(shoppingCart) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShoppingCart shoppingCart)
        {
            var result = await _cartServiceWithRedis.Create(shoppingCart);
            return result != null ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _cartServiceWithRedis.Delete(Id);
            return result == true ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("deleteproduct/{id}")]
        public async Task<IActionResult> DeleteProductInCart(string Id)
        {
            var result = await _cartServiceWithRedis.DeleteProductInCart(Id);
            return result == true ? Ok(result) : BadRequest(result);
        }
    
        [HttpPut("edit")]
        public async Task<IActionResult> Edit(ShoppingCart shoppingCart)
        {


            var result = await _cartServiceWithRedis.Update(shoppingCart.Id, shoppingCart);
            return result != null ? Ok(result) : BadRequest(result);
        }
       

        [HttpPut("EditInCart")]
        public async Task<IActionResult> EditInCart(ShoppingCartEditDto shoppingCart)
        {
            var entity = _mapper.Map<ShoppingCart>(shoppingCart);
            var existingShoppingCart = await _cartServiceWithRedis.GetById(shoppingCart.Id); // Mevcut alışveriş sepetini al
            if (existingShoppingCart == null)
            {
                return BadRequest("Alışveriş sepeti bulunamadı.");
            }

            // Kullanıcı ID'sini güncelle (gerekirse)
            existingShoppingCart.UserId = shoppingCart.UserId;

            // Her bir ürün için döngü yaparak güncelleme işlemini gerçekleştir
            foreach (var updatedProduct in shoppingCart.Products)
            {
                // Güncellenecek ürünün mevcut alışveriş sepetindeki index'ini bul
                var indexOfUpdatedProduct = existingShoppingCart.Products.FindIndex(p => p.Id == updatedProduct.Id);
                if (indexOfUpdatedProduct == -1)
                {
                    return BadRequest("Ürün alışveriş sepetinde bulunamadı.");
                }

                // Ürünün miktarını güncelle
                existingShoppingCart.Products[indexOfUpdatedProduct].Quantity = updatedProduct.Quantity;
            }

            // Değişiklikleri veritabanına kaydet
            var result = await _cartServiceWithRedis.Update(existingShoppingCart.Id, existingShoppingCart);
            return result != null ? Ok(result) : BadRequest(result);

        }
        [HttpGet("getcart/{id}")]
        public async Task<IActionResult> GetCart(string Id)
        {
            var result = await _cartServiceWithRedis.GetCartForUser(Id);
            return result != null ? Ok(result) : BadRequest(null);
        }
        [HttpGet("checkCartProduct/{cartId}/{productId}")]
        public async Task<IActionResult> CheckCartProduct(string cartId,string productId)
        {
            var result = await _cartServiceWithRedis.CheckProductInCart(cartId, productId);
            return result !=null ? Ok(result) : BadRequest();

        }
        [HttpDelete("deleteproductincart/{cartId}/{productId}")]
        public async Task<IActionResult> DeleteTheProductFromCart(string cartId,string productId)
        {
            var cart = await _cartServiceWithRedis.GetById(cartId);
            if (cart != null)
            {
                foreach (var item in cart.Products)
                {
                    if (productId == item.Id)
                    {
                        cart.Products.Remove(item);
                       await _cartServiceWithRedis.Update(cartId, cart);
                    }                      
                    if(cart.Products.Count < 1)
                    {
                        await _cartServiceWithRedis.Delete(cartId);
                    }
                    
                }
               
                return Ok(cart);
            }
            return BadRequest();
        }
        [HttpGet("GetProductsInOrder/{Id}")]
        public async Task<IActionResult> GetProductsInOrder(string Id)
        {
            var products = await _cartServiceWithRedis.GetProductsInOrder(Id);
            return Ok(products);

        }

    }
}

