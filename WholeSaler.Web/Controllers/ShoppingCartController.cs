using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Helpers.IdentyClaims;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WholeSaler.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public ShoppingCartController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();

        }
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity.Name;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var checkCartUri = $"https://localhost:7185/api/shoppingcart/getcart/{userId}";
                var result = await _httpClient.GetAsync(checkCartUri);
                if (result.IsSuccessStatusCode) {
                var jsonString = await result.Content.ReadAsStringAsync();
                var shoppingCart = JsonConvert.DeserializeObject<ShoppingCartUpdateVM>(jsonString);
                ViewData["ShoppingCartId"] = shoppingCart.Id;
                ViewData["UserId"] = shoppingCart.UserId;
                List<ProductForCartVM> productsIncart = new();
                foreach (var prd in shoppingCart.Products)
                {
                    productsIncart.Add(prd);
                }
                return View(productsIncart);
                }
            
       
                     
            var visitorId = Request.Cookies["visitor"];
            if (visitorId!=null)
            {
                try
                {

               
                var checkVisitorCartUri = $"https://localhost:7185/api/shoppingcart/getcart/{visitorId}";
                var visitorCartResponse = await _httpClient.GetAsync(checkVisitorCartUri);
                    if (visitorCartResponse.IsSuccessStatusCode) {
                var jsonString = await visitorCartResponse.Content.ReadAsStringAsync();
                var shoppingCart = JsonConvert.DeserializeObject<ShoppingCartUpdateVM>(jsonString);
                ViewData["ShoppingCartId"] = shoppingCart.Id;
                ViewData["UserId"] = shoppingCart.UserId;
                List<ProductForCartVM> productsIncart = new();
                foreach (var prd in shoppingCart.Products)
                {
                    productsIncart.Add(prd);
                }
                return View(productsIncart);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            TempData["NoCartMessage"] = "Sepetiniz Boş";
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(ShoppingCartCreateVM cartCreateVM,ProductForCartVM productVM,string returnUrl)
        {
            
            if (cartCreateVM.UserId == null)
            {

                var visitorId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1),
                    Secure = true,
                    HttpOnly = true
                };
                Response.Cookies.Append("visitor", visitorId, cookieOptions);
                cartCreateVM.UserId = visitorId;
            }
            var checkCartUri = $"https://localhost:7185/api/shoppingcart/getcart/{cartCreateVM.UserId}";
            var result = await _httpClient.GetAsync(checkCartUri);
            if (!result.IsSuccessStatusCode)
            {
                var shoppingCartCreateVM = new ShoppingCartCreateVM
                {
                    UserId = cartCreateVM.UserId,
                    Products = new List<ProductForCartVM> { productVM}
                };
                var cartCreateUri = "https://localhost:7185/api/shoppingcart";
                var json = JsonConvert.SerializeObject(shoppingCartCreateVM);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var resultCart = await _httpClient.PostAsync(cartCreateUri, content);
                TempData["CartMessage"] = $"{productVM.Name} is added to the cart";

                return returnUrl!=null? Redirect(returnUrl): RedirectToAction("Index", "Home");
            }
           else
            {
                var jsonString = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ShoppingCartUpdateVM>(jsonString);
                var existingProduct = data.Products.Find(x => x.Id == productVM.Id);
                if (existingProduct != null)
                {
                    existingProduct.Quantity++;
                }
                else
                {
                    data.Products.Add(productVM);
                }

                var cartUpdateUri = "https://localhost:7185/api/shoppingcart/edit";
                var accessToken = Request.Cookies["AccessToken"];
                AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", accessToken);

                _httpClient.DefaultRequestHeaders.Authorization = authHeader;
                var jsonUpdate = JsonConvert.SerializeObject(data);
                var contentUpdate = new StringContent(jsonUpdate, System.Text.Encoding.UTF8, "application/json");             
                    var resultUpdateCart = await _httpClient.PutAsync(cartUpdateUri, contentUpdate);
                TempData["CartMessage"] = $"{productVM.Name} is added to the cart";
                return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Home");



            }
             

           
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ShoppingCartUpdateVM cartUpdateVM)
        {
            var apiUri = "https://localhost:7185/api/shoppingcart/editincart";
            var jsonUpdate = JsonConvert.SerializeObject(cartUpdateVM);
            var contentUpdate = new StringContent(jsonUpdate, System.Text.Encoding.UTF8, "application/json");
            var resultUpdateCart = await _httpClient.PutAsync(apiUri, contentUpdate);
            if(resultUpdateCart.IsSuccessStatusCode) 
            {

                return RedirectToAction("index");
            }
            return RedirectToAction("index");


        }
        [HttpGet]
        public async Task<IActionResult> TestEdit(string Id)
        {
            var apiUri = "https://localhost:7185/api/shoppingcart/editincart";
            var jsonUpdate = JsonConvert.SerializeObject(Id);
            var contentUpdate = new StringContent(jsonUpdate, System.Text.Encoding.UTF8, "application/json");
            var resultUpdateCart = await _httpClient.PutAsync(apiUri, contentUpdate);
            if (resultUpdateCart.IsSuccessStatusCode)
            {

                return RedirectToAction("index");
            }
            return RedirectToAction("index");
        }

        public async Task<IActionResult> DeleteTheProductInCart(string cartId,string productId)
        {
            var apiUri = $"https://localhost:7185/api/shoppingcart/deleteproductincart/{cartId}/{productId}";
            var result = await _httpClient.DeleteAsync(apiUri);
            return RedirectToAction("index");
        }
    }
}
