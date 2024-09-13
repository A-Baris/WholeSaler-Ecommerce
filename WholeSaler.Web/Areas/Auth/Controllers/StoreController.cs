using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.Store;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Category;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Store;
using WholeSaler.Web.Helpers.HttpClientApiRequests;
using WholeSaler.Web.Helpers.IdentyClaims;
using WholeSaler.Web.MongoIdentity;
using WholeSaler.Web.Utility;

namespace WholeSaler.Web.Areas.Auth.Controllers
{
    [Area("auth")]
    [Authorize(Roles = "storeManager,admin")]
    public class StoreController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpApiRequest _httpApiRequests;
        private readonly HttpClient _httpClient;
     
        private const string apiUri = "https://localhost:7185/api/store";


      public StoreController(IHttpClientFactory httpClientFactory,UserManager<AppUser> userManager,IHttpApiRequest httpApiRequests)
        {
            _httpClientFactory = httpClientFactory;
           _userManager = userManager;
         _httpApiRequests = httpApiRequests;
            _httpClient = _httpClientFactory.CreateClient();
     
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            var getStoreUri = $"https://localhost:7185/api/store/mystore/{userId}";
            //var response =await _httpClient.GetAsync(getStoreUri);
            var response = await _httpApiRequests.GetAsync(getStoreUri);
            if (response.IsSuccessStatusCode)
            {
                //var storeJson = await response.Content.ReadAsStringAsync();
                //var storeData = JsonConvert.DeserializeObject<StoreVM>(storeJson);
                var storeData = await _httpApiRequests.DeserializeJsonToModelForSingle<StoreVM>(response);
                if (storeData.AdminConfirmation != Admin.Models.Enums.AdminConfirmation.Accepted)
                {
                    return RedirectToAction("applicationdetails", "store");
                }

                else
                {
                    return View();
                }
            }


            return RedirectToAction("create", "store");
        }

        [HttpGet]
        public async Task<IActionResult> SalesReport(DateTime startDate, DateTime? endDate)
        {
           

            if (endDate == null)
            {
                endDate = DateTime.UtcNow;
            }

            string formattedStartDate = startDate.ToString("yyyy-MM-ddTHH:mm:ss");
            string formattedEndDate = endDate.Value.ToString("yyyy-MM-ddTHH:mm:ss");
            var userId= HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           var user = await _userManager.FindByIdAsync(userId);
            var getSalesReportUri = $"https://localhost:7185/api/order/SalesReport/{user.StoreId}/{formattedStartDate}/{formattedEndDate}";
        
            var salesReport = await _httpClient.GetAsync(getSalesReportUri);
            if (salesReport.IsSuccessStatusCode) 
            { 
            var jsonReport = await salesReport.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<SalesReportVM>>(jsonReport);
                if (data.Count > 0 && data.Any())
                {
                    return View(data);
                }
                else 
                {
                    TempData["NotFoundMessage"] = "Data was not found in requested dates";
                    return View(data);
                }
       
            }
            
            return View(null);
        }


        //order cancellation requests
        [HttpGet]
        public async Task<IActionResult> CancellationRequests(string orderId)
        {

            return View();
        }



        #region mystore
        //public async Task<IActionResult> MyStore(string storeId)
        //{
        //    SetAuthorizationHeader();

        //    var uri = $"https://localhost:7185/api/product/mystore/{storeId}";
        //    var response = await _httpClient.GetAsync(uri);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var jsonString = await response.Content.ReadAsStringAsync();
        //        var data = JsonConvert.DeserializeObject<List<MyStoreVM>>(jsonString);
        //        return View(data);
        //    }
        //    return View();
        //} 
        #endregion

        private void SetAuthorizationHeader()
        {

            var _accessToken = Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }
            else
            {

                RedirectToAction("Login", "home");
            }
        }
    }
}
