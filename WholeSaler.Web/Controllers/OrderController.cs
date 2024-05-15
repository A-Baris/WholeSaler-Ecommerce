using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Helpers.EmailActions;
using WholeSaler.Web.Helpers.IdentyClaims;
using WholeSaler.Web.Models.ViewModels.OrderVM;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
using static System.Net.WebRequestMethods;
namespace WholeSaler.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }
        [HttpGet]
        public IActionResult Create(string cartId, string totalAmount)
        {
            if (cartId == null)
            {
                return RedirectToAction("index", "shoppingCart");
            }
            ViewBag.CartId = cartId;
            if (totalAmount == null)
            {
                return RedirectToAction("index", "shoppingCart");
            }
            ViewBag.CartTotalAmount = decimal.Parse(totalAmount);
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "home");
            }
            ViewData["userId"] = userId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateVM createVM)
        {
            var orderCreateUri = $"https://localhost:7185/api/order";
            var json = JsonConvert.SerializeObject(createVM);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var orderResult = await _httpClient.PostAsync(orderCreateUri, content);
            if (orderResult.IsSuccessStatusCode)
            {
                var orderJson = await orderResult.Content.ReadAsStringAsync();
                var orderData = JsonConvert.DeserializeObject<OrderVM>(orderJson);
                var editProductUri = $"https://localhost:7185/api/product/updateStocks";
                var prdJson = JsonConvert.SerializeObject(orderData.Products);
                var prdContent = new StringContent(prdJson, System.Text.Encoding.UTF8, "application/json");
                var editResult = await _httpClient.PutAsync(editProductUri, prdContent);
                var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (userEmail != null)
                {
                    string subject = $"Sipariş Bilgisi";
                    string body = $"Sipariş Onay Tarihi : {orderData.CreatedDate}\nSipariş Tutarı : {orderData.TotalOrderAmount} TL ";
                    //try
                    //{
                    //    EmailSender.SendEMail(userEmail, subject, body);
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}
                }
                TempData["OrderId"] = orderData.Id;
                return RedirectToAction("details", "Order", new { orderId = orderData.Id });
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Details(string orderId)
        {
            var getOrderUri = $"https://localhost:7185/api/order/{orderId}";
            var orderResponse = await _httpClient.GetAsync(getOrderUri);
            if (orderResponse.IsSuccessStatusCode)
            {
                var orderJson = await orderResponse.Content.ReadAsStringAsync();
                var orderDetail = JsonConvert.DeserializeObject<OrderInformationVM>(orderJson);
                var shoppingCartId = orderDetail.ShoppingCartId;
                if (shoppingCartId != null)
                {
                    var getOneCartUri = $"https://localhost:7185/api/shoppingcart/{shoppingCartId}";
                    var response = await _httpClient.GetAsync(getOneCartUri);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var shoppingCart = JsonConvert.DeserializeObject<ShoppingCartUpdateVM>(jsonString);
                        List<ProductForCartVM> productsIncart = new();
                        foreach (var prd in shoppingCart.Products)
                        {
                            productsIncart.Add(prd);
                        }
                        return View(productsIncart);
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllOrders()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var getOrdersUri = $"https://localhost:7185/api/order/PrivateUserOrders/{userId}";
            var response = await _httpClient.GetAsync(getOrdersUri);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var orderDatas = JsonConvert.DeserializeObject<List<OrderInformationVM>>(jsonString);
                return View(orderDatas);
            }
            return RedirectToAction("index", "home");
        }
    }
}
