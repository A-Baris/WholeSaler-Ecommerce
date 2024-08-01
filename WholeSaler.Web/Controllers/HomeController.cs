using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Newtonsoft.Json;

using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.Category;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Category;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Helpers.HttpClientApiRequests;
using WholeSaler.Web.Helpers.IdentyClaims;
using WholeSaler.Web.Helpers.PropertyCoppier;
using WholeSaler.Web.Models;
using WholeSaler.Web.Models.ViewModels.Product;
using WholeSaler.Web.Models.ViewModels.Product.Filters;
using WholeSaler.Web.Models.ViewModels.Product.Filters.Electronics;
using WholeSaler.Web.Models.ViewModels.Product.Filters.PersonalCleaning;
using WholeSaler.Web.Models.ViewModels.Product.Services;
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
        public async Task<IActionResult> Index(string productName, string categoryName, string subCategoryName, ProductFilterVM productFilterVM)
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
                    if (productFilterVM != null)
                    {

                        if (!string.IsNullOrEmpty(productFilterVM.Color))
                        {
                            data = data.Where(p => p.Color == productFilterVM.Color).ToList();
                        }
                    }
                    if (subCategoryName != null)
                    {

                        var productforSubCategory = data.Where(x => x.Category.SubCategory.Name == subCategoryName).ToList();
                        return View(productforSubCategory);
                    }
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


        [HttpGet]
        public async Task<IActionResult> Category(string productName, string categoryName, string subCategoryName)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["uId"] = userId;
            ViewData["visitorId"] = Request.Cookies["visitor"];
            ViewData["categoryName"] = categoryName;
            ViewData["subCategoryName"] = subCategoryName;

            if (subCategoryName != null)
            {


                BaseProductFilterVM filterVM;

                switch (subCategoryName.ToLower())
                {
                    case "television":
                        filterVM = new TelevisionFilterVM();
                        break;
                    case "computer":
                        filterVM = new LaptopFilterVM();
                        break;
                    case "perfume":
                        filterVM = new PerfumeFilterVM();
                        break;
                    default:
                        filterVM = new BaseProductFilterVM();
                        break;
                }

                ViewBag.FilterVM = filterVM;
            }
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

                    if (subCategoryName != null)
                    {

                        var productforSubCategory = data.Where(x => x.Category.SubCategory.Name == subCategoryName).ToList();
                        return View(productforSubCategory);
                    }
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
        [HttpPost]
        public async Task<IActionResult> Category(ProductForFilter productFilterVM, string categoryName, string subCategoryName)
        {

            if (Request.Query.ContainsKey("filter"))
            {
                var testQuery = JsonConvert.DeserializeObject<ProductForFilter>(Request.Query["filter"]);

            }

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["uId"] = userId;
            ViewData["visitorId"] = Request.Cookies["visitor"];
            ViewData["categoryName"] = categoryName;
            ViewData["subCategoryName"] = subCategoryName;
            ViewBag.ProductFilterVM = productFilterVM;


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
                    if (productFilterVM != null)
                    {
                        BaseProductFilterVM filterVM;
                        switch (subCategoryName.ToLower())
                        {
                            case "television":
                                filterVM = new TelevisionFilterVM();
                                break;
                            case "computer":
                                filterVM = new LaptopFilterVM();
                                break;
                            default:
                                filterVM = new BaseProductFilterVM();
                                break;
                        }
                        CopyProperty.CopyProperties(productFilterVM, filterVM);
                        //data = _productFilterService.GetFilteredProducts(filterVM, data);

                        ViewBag.ProductFilterVM = productFilterVM;
                        return View(data);

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
