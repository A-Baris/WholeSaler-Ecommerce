using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Category;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Store;
using WholeSaler.Web.Helpers.IdentyClaims;
using WholeSaler.Web.MongoIdentity;
using WholeSaler.Web.Utility;

namespace WholeSaler.Web.Areas.Auth.Controllers
{
    [Area("auth")]
    public class StoreController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly HttpClient _httpClient;
     

        private const string apiUri = "https://localhost:7185/api/store";


      public StoreController(IHttpClientFactory httpClientFactory,UserManager<AppUser> userManager)
        {
            _httpClientFactory = httpClientFactory;
           _userManager = userManager;
            _httpClient = _httpClientFactory.CreateClient();
     
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        
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
