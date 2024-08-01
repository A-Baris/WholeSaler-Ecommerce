using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.Store;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Category;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Store;
using WholeSaler.Web.FluentValidation.Configs;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
using WholeSaler.Web.MongoIdentity;

namespace WholeSaler.Web.Controllers
{
    public class StoreController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidationService<StoreCreateVM> _storeCreateValidator;
        private readonly HttpClient _httpClient;

        public StoreController(IHttpClientFactory httpClientFactory,UserManager<AppUser> userManager,IValidationService<StoreCreateVM> storeCreateValidator)
        {
           _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _storeCreateValidator = storeCreateValidator;
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<IActionResult> Index(string storeId,string productName,string categoryName)
        {
            ViewData["StoreId"]=storeId;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["uId"] = userId;
            ViewData["visitorId"] = Request.Cookies["visitor"];
            var categoryUri = "https://localhost:7185/api/category";
            var responseCategory = await _httpClient.GetAsync(categoryUri);

            var storeUri = $"https://localhost:7185/api/store/{storeId}";
            var storeResponse = await _httpClient.GetAsync(storeUri);
            if (storeResponse.IsSuccessStatusCode) 
            {
                var storeJson = await storeResponse.Content.ReadAsStringAsync();
                var storeData = JsonConvert.DeserializeObject<StoreVM>(storeJson);
                ViewBag.StoreInfo = storeData;
            }


            var apiUri = "https://localhost:7185/api/product";
            var response = await _httpClient.GetAsync(apiUri);
            if (response.IsSuccessStatusCode)
            {


                var jsonString = await response.Content.ReadAsStringAsync();
                var productData = JsonConvert.DeserializeObject<List<ProductForCartVM>>(jsonString);
                var data = productData.Where(x => x.Store.StoreId == storeId).ToList();
               
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();


        }
        [HttpPost]
        public async Task<IActionResult> Create(StoreCreateVM createVM)
        {
            var errors = _storeCreateValidator.GetValidationErrors(createVM);
            if (errors.Any())
            {
                ModelStateHelper.AddErrorsToModelState(ModelState, errors);
                return View(createVM);
            }
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (userId == null)
            {
                return RedirectToAction("login", "user");

            }


            createVM.UserId = userId;

            var uri = $"https://localhost:7185/api/store";
            var json = JsonConvert.SerializeObject(createVM);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, content);
            if (response.IsSuccessStatusCode) 
            {
                var jsonStore = await response.Content.ReadAsStringAsync();
                var storeData = JsonConvert.DeserializeObject<StoreVM>(jsonStore);
                user.StoreId = storeData.Id;
               var updatedUser= _userManager.UpdateAsync(user);

                return RedirectToAction("ApplicationDetails", "store");

            }
            //hata mesajı
           return RedirectToAction("create","store");
        }
        [HttpGet]
        public  IActionResult ApplicationDetails()
        {
          
            return View();
        }
    }
}
