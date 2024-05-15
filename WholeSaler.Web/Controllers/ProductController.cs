using Microsoft.AspNetCore.Mvc;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using Newtonsoft.Json;
using System.Security.Claims;
using WholeSaler.Web.Models.ViewModels.Product;
using System.Net.Http.Headers;
using System.Net.Http;
using WholeSaler.Web.Utility;

namespace WholeSaler.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }
        //public async Task<IActionResult> SearchProduct(string productName)
        //{
        //    var productApiUri = "https://localhost:7185/api/product";
        //    var result = await _httpClient.GetAsync(productApiUri);
        //    var jsonString = await result.Content.ReadAsStringAsync();
        //    return View();
        //}
        public async Task<IActionResult> Details(string productId)
        {
            var returnUrl = Url.Action("Details", "product", new { productId });

            ViewData["ReturnUrl"] = returnUrl;

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var visitorId = Request.Cookies["visitor"];
            ViewData["uId"] =userId;
            ViewData["visitorId"]=visitorId;
            var getProductUri = $"https://localhost:7185/api/product/{productId}";
            SetAuthHeader.SetAuthorizationHeader(_httpClient,Request);
            var response = await _httpClient.GetAsync(getProductUri);
            if (response.IsSuccessStatusCode)
            {
                var jsonProduct = await response.Content.ReadAsStringAsync();
                var productData = JsonConvert.DeserializeObject<ProductVM>(jsonProduct);
                if (productData != null) 
                {
                 return View(productData);
                }
                
            }
            return RedirectToAction("index", "home");
        }
        [HttpPost]
        public async Task<IActionResult> CommentToProduct(string productId,ProductCommentVM commentVM)
        {
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username)) 
            {
                return RedirectToAction("login", "user");
            }
            commentVM.Username = username;
            commentVM.CreatedDate = DateTime.Now;

            var updateProductUri = $"https://localhost:7185/api/product/AddComment/{productId}";
            var json = JsonConvert.SerializeObject(commentVM);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var result = await _httpClient.PutAsync(updateProductUri, content);
            return RedirectToAction("details", "product", new { productId = productId });
        }
    }
}
