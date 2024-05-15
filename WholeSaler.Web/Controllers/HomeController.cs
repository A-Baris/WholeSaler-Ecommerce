using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Category;
using WholeSaler.Web.Helpers.IdentyClaims;
using WholeSaler.Web.Models;

using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;


namespace WholeSaler.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();


        }
        [HttpGet]
        public async Task<IActionResult> Index(string productName, string categoryName)
        {

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["uId"] = userId;
            ViewData["visitorId"] = Request.Cookies["visitor"];
            var categoryUri = "https://localhost:7185/api/category";
            var responseCategory = await _httpClient.GetAsync(categoryUri);



            var apiUri = "https://localhost:7185/api/product";
            var response = await _httpClient.GetAsync(apiUri);
            if (response.IsSuccessStatusCode)
            {


                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<ProductForCartVM>>(jsonString);
                if (responseCategory.IsSuccessStatusCode)
                {
                    var jsonCategory = await responseCategory.Content.ReadAsStringAsync();
                    var categoryData = JsonConvert.DeserializeObject<List<CategoryVM>>(jsonCategory);
                    ViewBag.Categories = categoryData;
                    if (categoryName != null)
                    {
                        var productForCategory = data.Where(x => x.Category.CategoryName == categoryName).ToList();
                        return View(productForCategory);
                    }

                }

                if (!string.IsNullOrEmpty(productName))
                {

                    var lowerCasePrefix = productName.Substring(0, Math.Min(3, productName.Length)).ToLower();

                    data = data.Where(x => x.Name.ToLower().StartsWith(lowerCasePrefix)).ToList();
                    if (data.Count > 0) { return View(data); }
                    else
                    {
                        TempData["productNameMessage"] = $"{productName} is not found";
                        return RedirectToAction("index");
                    }


                }
                return View(data);
            }
            return View();

        }


    


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
