using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.Order;
using WholeSaler.Web.Models.Enums;
using WholeSaler.Web.Models.ViewModels.OrderVM;
using WholeSaler.Web.Utility;

namespace WholeSaler.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient _httpClient;
        public OrderController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient();
        }
        public IActionResult Index()
        {
            return View();
        }


        //cancellation request will review here and make approve or reject
        [HttpGet]
        public async Task<IActionResult> CancellationRequests()
        {
            SetAuthHeader.SetAuthorizationHeader(Request,_httpClient);
            var getOrdersUri = $"https://localhost:7185/api/order";
            var response = await _httpClient.GetAsync(getOrdersUri);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderForAdminVm>>(jsonString);
                var orderCancellations= orders.Where(x=>x.CancellationRequest!=null&& (x.Status == EntityStatus.Active)).ToList();
                return View(orderCancellations);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CancellationRequestConfirm(string orderId,string cartId)
        {
            int statusCode = ((int)EntityStatus.Deleted);
            var changeOrderStatusApiUri = $"https://localhost:7185/api/order/changestatus/{orderId}/{statusCode}";
            var changeShoppingCartStatusApiUri = $"https://localhost:7185/api/shoppingcart/changestatus/{cartId}/{statusCode}";
            var orderApiResponse = await _httpClient.GetAsync(changeOrderStatusApiUri);
            if (orderApiResponse.IsSuccessStatusCode)
            {
                //check response code and set
                await _httpClient.GetAsync(changeShoppingCartStatusApiUri);
                return RedirectToAction("CancellationRequests", "order", new { area = "admin" });
            }
           
            return RedirectToAction("CancellationRequests","order",new {area="admin"});
        }
        [HttpGet]
        public async Task<IActionResult> CancellationRequestReject()
        {
            return View();
        }
    }
}
