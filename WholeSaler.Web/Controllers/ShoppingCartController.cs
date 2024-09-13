using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Category;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Helpers.HttpClientApiRequests;
using WholeSaler.Web.Helpers.IdentyClaims;
using WholeSaler.Web.Helpers.QueryHelper;
using WholeSaler.Web.Models.ViewModels.Product;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WholeSaler.Web.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly IHttpApiRequest _httpApiRequest;
        private readonly string cartApiUri = "https://localhost:7185/api/shoppingcart";

        public ShoppingCartController(IHttpApiRequest httpApiRequest)
        {
          
           _httpApiRequest = httpApiRequest;
   

        }
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity.Name;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var shoppingCart = await CheckCart(userId);
            if (shoppingCart!=null)
            {
               
                ViewData["ShoppingCartId"] = shoppingCart.Id;
                ViewData["Uid"] = shoppingCart.UserId;
                List<ProductForCartVM> productsIncart = new();
                foreach (var prd in shoppingCart.Products)
                {
                    productsIncart.Add(prd);
                }
                return View(productsIncart);
            }



            var visitorId = Request.Cookies["visitor"];
            if (visitorId != null)
            {
                try
                {


                    var visitorShoppingCart = await CheckCart(visitorId);
                    if (visitorShoppingCart!=null)
                    {
                        
                        ViewData["ShoppingCartId"] = visitorShoppingCart.Id;
                        ViewData["Uid"] = visitorShoppingCart.UserId;
                        List<ProductForCartVM> productsIncart = new();
                        foreach (var prd in visitorShoppingCart.Products)
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
            TempData["ErrorMessage"] = "Sepetiniz Boş";
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(ShoppingCartCreateVM cartCreateVM, ProductForCartVM productVM,string returnUrl)
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
            var shoppingCart = await CheckCart(cartCreateVM.UserId);
            if (shoppingCart==null)
            {
                var shoppingCartCreateVM = new ShoppingCartCreateVM
                {
                    UserId = cartCreateVM.UserId,
                    Products = new List<ProductForCartVM> { productVM }
                };
                var cartCreateUri = cartApiUri;
                var resultCart = await _httpApiRequest.PostAsync(cartCreateUri, shoppingCartCreateVM);
                TempData["CartMessage"] = $"{productVM.Name} is added to the cart";

               
            }
            else
            {
                
                var existingProduct = shoppingCart.Products.Find(x => x.Id == productVM.Id);
                if (existingProduct != null)
                {
                    existingProduct.Quantity++;
                }
                else
                {
                    shoppingCart.Products.Add(productVM);
                }

                var cartUpdateUri = cartApiUri + "/edit";
                var resultUpdateCart = await _httpApiRequest.PutAsync(cartUpdateUri, shoppingCart);
                TempData["CartMessage"] = $"{productVM.Name} is added to the cart";
                return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Home");



            }

            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Home");


        }
        [HttpPost]
        public async Task<IActionResult> Edit(ShoppingCartUpdateVM cartUpdateVM)
        {
            var apiUri = cartApiUri + "/editincart";
            var resultUpdateCart = await _httpApiRequest.PutAsync(apiUri, cartUpdateVM);
            if (resultUpdateCart.IsSuccessStatusCode)
            {

                return RedirectToAction("index");
            }
            return RedirectToAction("index");


        }
    

        public async Task<IActionResult> DeleteTheProductInCart(string cartId, string productId)
        {
            var apiUri = cartApiUri + $"/deleteproductincart/{cartId}/{productId}";
            var response = await _httpApiRequest.DeleteAsync(apiUri);
            return RedirectToAction("index");
        }
        private async Task<ShoppingCartUpdateVM> CheckCart(string id)
        {
            string checkCartUri = cartApiUri + $"/getcart/{id}";
            var response = await _httpApiRequest.GetAsync(checkCartUri);
            if (response.IsSuccessStatusCode)
            {
                return await _httpApiRequest.DeserializeJsonToModelForSingle<ShoppingCartUpdateVM>(response);
            }
            return null;
        }
    }
}
