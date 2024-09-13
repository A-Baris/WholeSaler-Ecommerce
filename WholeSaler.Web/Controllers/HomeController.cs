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
using WholeSaler.Web.Helpers.ProductHelper;
using WholeSaler.Web.Helpers.PropertyCoppier;
using WholeSaler.Web.Models;
using WholeSaler.Web.Models.ViewModels.Product;
using WholeSaler.Web.Models.ViewModels.Product.Custom;
using WholeSaler.Web.Models.ViewModels.Product.Filters;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
namespace WholeSaler.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductFilterService _productFilterService;
        private readonly IHttpApiRequest _httpApiRequest;
        private readonly string _getProductsUri = "https://localhost:7185/api/product";
        private readonly string _getCategoriesUri = "https://localhost:7185/api/category";
        public HomeController(ILogger<HomeController> logger, IProductFilterService productFilterService, IHttpApiRequest httpApiRequest)
        {
            _logger = logger;
            _productFilterService = productFilterService;
            _httpApiRequest = httpApiRequest;

        }

        [HttpGet]
        public async Task<IActionResult> Index(string productName, string categoryName, string subCategoryName)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ViewData["uId"] = userId;
                ViewData["visitorId"] = Request.Cookies["visitor"];




                var productData = await GetProductData();
                var categoryData = await GetCategoryData();
                ViewBag.Categories = categoryData;
                if (subCategoryName != null)
                {
                    var productforSubCategory = productData.Where(x => x.Category.SubCategory.Name == subCategoryName).ToList();
                    return View(productforSubCategory);
                }
                if (categoryName != null)
                {
                    var productForCategory = productData.Where(x => x.Category.CategoryName == categoryName).ToList();
                    return View(productForCategory);
                }

                if (!string.IsNullOrEmpty(productName))
                {
                    var lowerCasePrefix = productName.Substring(0, Math.Min(3, productName.Length)).ToLower();
                    productData = productData.Where(x => x.Name.ToLower().StartsWith(lowerCasePrefix)).ToList();
                    if (productData.Count > 0) { return View(productData); }
                    else
                    {
                        TempData["ErrorMessage"] = $"{productName} is not found";
                        return RedirectToAction("index");
                    }
                }
                return View(productData);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Index is not found";
                return View();
            }
          
        }
        [HttpGet]
        public async Task<IActionResult> Category(string productName, string categoryName, string subCategoryName)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["uId"] = userId;
            ViewData["visitorId"] = Request.Cookies["visitor"];
            ViewData["categoryName"] = categoryName;
            ViewData["subCategoryName"] = subCategoryName;


         
             var productData = await GetProductData();
            if (productData.Count < 1) { return View(productData); }

                var categoryData = await GetCategoryData();
                ViewBag.Categories = categoryData;
                  
                    if (subCategoryName != null)
                    {
                        var productforSubCategory = productData.Where(x => x.Category.SubCategory.Name.Equals(subCategoryName, StringComparison.OrdinalIgnoreCase)).ToList();
                        return View(productforSubCategory);
                    }
                    if (categoryName != null)
                    {
                        var productForCategory = productData.Where(x => x.Category.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase)).ToList();
                        return View(productForCategory);
                    }
                
                if (!string.IsNullOrEmpty(productName))
                {
                    var lowerCasePrefix = productName.Substring(0, Math.Min(3, productName.Length)).ToLower();
                    productData = productData.Where(x => x.Name.ToLower().StartsWith(lowerCasePrefix)).ToList();
                    if (productData.Count > 0) { return View(productData); }
                    else
                    {
                        TempData["ErrorMessage"] = $"{productName} is not found";
                        return RedirectToAction("index");
                    }
                }
                return View(productData);
            
            
        }
        [HttpPost]
        public async Task<IActionResult> Category(ProductFilterVm productFilterVM, string categoryName, string subCategoryName)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["uId"] = userId;
            ViewData["visitorId"] = Request.Cookies["visitor"];
            ViewData["categoryName"] = categoryName;
            ViewData["subCategoryName"] = subCategoryName;

            var categoryData = await GetCategoryData();
            ViewBag.Categories = categoryData;
            var apiUri = "https://localhost:7185/api/product/getforfilter";
            var response = await _httpApiRequest.GetAsync(apiUri);
            if (response.IsSuccessStatusCode)
            {
                var data = await _httpApiRequest.DeserializeJsonToModelForSingle<ProductGeneralVm>(response);
                var filter = _productFilterService.GetFilteredProducts(data, productFilterVM);
                ViewBag.ProductFilterVM = filter.Item2;
                return View(filter.Item1);
            }
            return View(null);
        }


        private async Task<List<ProductForCartVM>> GetProductData()
        {
            var response = await _httpApiRequest.GetAsync(_getProductsUri);
            if (response.IsSuccessStatusCode)
            {
                return await _httpApiRequest.DeserializeJsonToModelForList<ProductForCartVM>(response);
            }
            return new List<ProductForCartVM>();
        }

        private async Task<List<CategoryVM>> GetCategoryData()
        {
            var response = await _httpApiRequest.GetAsync(_getCategoriesUri);
            if (response.IsSuccessStatusCode)
            {
                return await _httpApiRequest.DeserializeJsonToModelForList<CategoryVM>(response);
            }
            return new List<CategoryVM>();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
