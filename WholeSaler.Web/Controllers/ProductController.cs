using Microsoft.AspNetCore.Mvc;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using Newtonsoft.Json;
using System.Security.Claims;
using WholeSaler.Web.Models.ViewModels.Product;
using System.Net.Http.Headers;
using System.Net.Http;
using WholeSaler.Web.Utility;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.BaseProduct;
using WholeSaler.Web.Helpers.HttpClientApiRequests;

namespace WholeSaler.Web.Controllers
{
    public class ProductController : Controller
    {

        private readonly IHttpApiRequest _httpApiRequest;
        private readonly string productApiUri = "https://localhost:7185/api/product";

        public ProductController(IHttpApiRequest httpApiRequest)
        {
 
          _httpApiRequest = httpApiRequest;
        
        }
     
        public async Task<IActionResult> Details(string productId)
        {
            var returnUrl = Url.Action("Details", "product", new { productId });

            ViewData["ReturnUrl"] = returnUrl;

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var visitorId = Request.Cookies["visitor"];
            ViewData["uId"] =userId;
            ViewData["visitorId"]=visitorId;
            var getProductUri = productApiUri +$"/{productId}";
            //SetAuthHeader.SetAuthorizationHeader(Request);
            var productResponse = await _httpApiRequest.GetAsync(getProductUri);
            if (productResponse.IsSuccessStatusCode)
            {
                var productData = await _httpApiRequest.DeserializeJsonToModelForSingle<ProductVm>(productResponse);
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

            var updateProductUri = productApiUri + "/AddComment/{productId}";
            var result = await _httpApiRequest.PutAsync(updateProductUri, commentVM);
            return RedirectToAction("details", "product", new { productId = productId });
        }
    }
}
