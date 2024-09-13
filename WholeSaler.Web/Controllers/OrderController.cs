using Microsoft.AspNet.SignalR.Client.Http;
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
using WholeSaler.Web.Helpers.HttpClientApiRequests;
using WholeSaler.Web.Helpers.IdentyClaims;
using WholeSaler.Web.Models.ViewModels.OrderVM;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
using static System.Net.WebRequestMethods;
namespace WholeSaler.Web.Controllers
{
    public class OrderController : Controller
    {
       
        private readonly IHttpApiRequest _httpApiRequest;
        private readonly string orderApiUri = "https://localhost:7185/api/order";
        public OrderController(IHttpApiRequest httpApiRequest)
        {

           _httpApiRequest = httpApiRequest;
    
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
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
            ViewData["userId"] = userId;
            ViewData["userName"] = username;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateVM createVM)
        {

            var orderResponse = await _httpApiRequest.PostAsync(orderApiUri, createVM);
            if (orderResponse.IsSuccessStatusCode)
            {
               
                var orderData = await _httpApiRequest.DeserializeJsonToModelForSingle<OrderVM>(orderResponse);
                var editProductUri = $"https://localhost:7185/api/product/updateStocks";
                var editResult = await _httpApiRequest.PutAsync(editProductUri, orderData.Products);
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
            ViewBag.OrderId = orderId;
            var getOrderUri = orderApiUri + $"/{orderId}";
            var orderResponse = await _httpApiRequest.GetAsync(getOrderUri);
            if (orderResponse.IsSuccessStatusCode)
            {
                var orderDetail = await _httpApiRequest.DeserializeJsonToModelForSingle<OrderInformationVM>(orderResponse);
                var shoppingCartId = orderDetail.ShoppingCartId;
                if (shoppingCartId != null)
                {
                    var getOneCartUri = $"https://localhost:7185/api/shoppingcart/{shoppingCartId}";
                    var cartResponse = await _httpApiRequest.GetAsync(getOneCartUri);
                    if (cartResponse.IsSuccessStatusCode)
                    {
                        var shoppingCart = await _httpApiRequest.DeserializeJsonToModelForSingle<ShoppingCartUpdateVM>(cartResponse);
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
            var getOrdersUri = orderApiUri + $"/PrivateUserOrders/{userId}";
            var response = await _httpApiRequest.GetAsync(getOrdersUri);
            if (response.IsSuccessStatusCode)
            {
                var orderDatas = await _httpApiRequest.DeserializeJsonToModelForList<OrderInformationVM>(response);
                return View(orderDatas);
            }
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        public async Task<IActionResult> OrderCancellationRequest(string orderId)
        {
            ViewData["orderId"] = orderId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OrderCancellationRequest(OrderEditVm editVm)
        {
            var orderEditUri= orderApiUri + "/edit";
            var orderResult = await _httpApiRequest.PutAsync(orderEditUri, editVm);
     
                
                return View();
        }
    }
}
