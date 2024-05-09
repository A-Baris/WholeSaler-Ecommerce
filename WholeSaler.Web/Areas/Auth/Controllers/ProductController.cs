using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.Store;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Category;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Store;
using WholeSaler.Web.Helpers.CookieHelper;
using WholeSaler.Web.Helpers.IdentyClaims;
using WholeSaler.Web.Helpers.ImageHelper;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
using WholeSaler.Web.MongoIdentity;

namespace WholeSaler.Web.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class ProductController : Controller
    {
        private const string apiUri = "https://localhost:7185/api/product";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly HttpClient _httpClient;


        public ProductController(IHttpClientFactory httpClientFactory, UserManager<AppUser> userManager)
        {
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _httpClient = _httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string categoryId)
        {
            var storeId = "";
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                storeId = user.StoreId;
                if (storeId == null)
                {
                    return RedirectToAction("Index", "home", new { area = "" });
                }
            }
            var uri = $"https://localhost:7185/api/product/mystore/{storeId}";
            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<MyStoreVM>>(jsonString);
                var categoryUri = "https://localhost:7185/api/category";
                var categoryResponse = await _httpClient.GetAsync(categoryUri);
                var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                var categoryData = JsonConvert.DeserializeObject<List<CategoryVM>>(categoryJson);
                ViewBag.Categories = categoryData;
                ViewData["storeId"] = storeId;
                if (categoryId != null)
                {
                    var filterProducts = data.Where(x => x.Category.CategoryId == categoryId).ToList();
                    return View(filterProducts);
                }


                return View(data);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string storesGetFromeApi = $"https://localhost:7185/api/store";
            var response = await _httpClient.GetAsync(storesGetFromeApi);
            if (response.IsSuccessStatusCode)
            {

                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<StoreVM>>(jsonString);
                var userStore = data.Where(x => x.UserId == userId).FirstOrDefault();
                ViewData["StoreName"] = userStore.Name;
                ViewData["StoreId"] = userStore.Id;
                return View();

            }


            return RedirectToAction("login", "home", new { area = "" });
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM createVM, List<IFormFile> productImages)
        {
            if (createVM.Images == null)
            {
                createVM.Images = new List<ProductImage>();
            }

            foreach (var productImage in productImages)
            {

                if (productImage != null && productImage.Length > 0)
                {
                    string imageResult = ImageUpload.ImageChangeName(productImage.FileName);

                    if (!string.IsNullOrEmpty(imageResult) && imageResult != "0")
                    {

                        var productImageModel = new ProductImage
                        {
                            FileName = imageResult,

                            Path = Path.Combine("wwwroot\\images\\products", imageResult)
                        };


                        string path = Path.Combine(Directory.GetCurrentDirectory(), productImageModel.Path);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await ImageResize.ResizeImage(productImage.OpenReadStream(), stream, 600, 400);
                        }
                        try
                        {
                            createVM.Images.Add(productImageModel);
                        }

                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                }
            }

            var uri = $"{apiUri}";
            var json = JsonConvert.SerializeObject(createVM);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(uri, content);
            if (result.IsSuccessStatusCode) { return RedirectToAction("index", "store", new { area = "auth", storeId = createVM.Store.StoreId }); }
            else { return RedirectToAction("create", "product", new { area = "auth" }); }
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var uri = $"{apiUri}/{id}";
            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProductCreateVM>(jsonString);
                return View(data);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var uri = $"{apiUri}/{id}";
            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProductEditVM>(jsonString);

                return View(data);


            }
            return View("index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditVM editVM, List<IFormFile>? productImages)
        {
            if (productImages.Any())
            {
              
                    editVM.Images = new List<ProductImage>();
                
                foreach (var productImage in productImages)
                {

                    if (productImage != null && productImage.Length > 0)
                    {
                        string imageResult = ImageUpload.ImageChangeName(productImage.FileName);

                        if (!string.IsNullOrEmpty(imageResult) && imageResult != "0")
                        {

                            var productImageModel = new ProductImage
                            {
                                FileName = imageResult,

                                Path = Path.Combine("wwwroot\\images\\products", imageResult)
                            };


                            string path = Path.Combine(Directory.GetCurrentDirectory(), productImageModel.Path);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await ImageResize.ResizeImage(productImage.OpenReadStream(), stream, 600, 400);
                            }
                            try
                            {
                                editVM.Images.Add(productImageModel);
                            }

                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                        }
                    }
                }
            }
            var uri = $"{apiUri}/edit";
            var json = JsonConvert.SerializeObject(editVM);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var result = await _httpClient.PutAsync(uri, content);
            if (result.IsSuccessStatusCode) { return RedirectToAction("index", "product", new { area = "auth"}); }
            else { return RedirectToAction("edit", "product", new { area = "auth" }); }

        }
        [HttpGet]
        public async Task<IActionResult> Delete(string productId, string storeId)
        {
            var deleteProductUri = $"{apiUri}/delete/{productId}";
            var result = await _httpClient.DeleteAsync(deleteProductUri);
            if (result.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Successful";
                return RedirectToAction("mystore", "store", new { area = "auth", storeId = storeId });

            }
            else
            {
                TempData["ErrorMessage"] = "Failed";
                return RedirectToAction("mystore", "store", new { area = "auth", storeId = storeId });

            }
        }
        [HttpGet]
        public async Task<IActionResult> ChangeStatus(string id, int statusCode)
        {
            var changeStatusApiUri = $"https://localhost:7185/api/product/changestatus/{id}/{statusCode}";
            var response = await _httpClient.GetAsync(changeStatusApiUri);
            return RedirectToAction("index", "product", new { area = "auth" });
        }

    }
}
