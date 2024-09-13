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
using static System.Runtime.InteropServices.JavaScript.JSType;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Comprehensive;
using wholesaler.web.helpers.producthelper;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.BaseProduct;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted;
using FluentValidation;
using WholeSaler.Web.FluentValidation.Validators.Product;
using WholeSaler.Web.FluentValidation.Configs;


namespace WholeSaler.Web.Areas.Auth.Controllers
{
    [Area("Auth")]
    [Authorize(Roles ="storeManager,admin")]
    public class ProductController : Controller
    {
        private const string apiUri = "https://localhost:7185/api/product";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidationService<ProductComprehensiveVM> _prdValidator;
        private readonly IValidationService<ProductEditVM> _prdEditValidator;
        private readonly HttpClient _httpClient;


        public ProductController(IHttpClientFactory httpClientFactory, UserManager<AppUser> userManager, IValidationService<ProductComprehensiveVM> prdValidator, IValidationService<ProductEditVM> prdEditValidator)
        {
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _prdValidator = prdValidator;
            _prdEditValidator = prdEditValidator;
            _httpClient = _httpClientFactory.CreateClient();
        }

        [HttpGet]
        //[Route("auth/product/{productName?}")]

        public async Task<IActionResult> Index(string productName,string categoryId,string subcategoryId,string ascending)
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
                data = data.OrderByDescending(x => x.SumofSales).ToList();
             
             

                var categoryUri = "https://localhost:7185/api/category";
                var categoryResponse = await _httpClient.GetAsync(categoryUri);
                var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                var categoryData = JsonConvert.DeserializeObject<List<CategoryVM>>(categoryJson);
                ViewBag.Categories = categoryData;
                ViewData["storeId"] = storeId;

                if (!string.IsNullOrEmpty(productName))
                {

                    var lowerCasePrefix = productName.Substring(0, Math.Min(3, productName.Length)).ToLower();

                    data = data.Where(x => x.Name.ToLower().StartsWith(lowerCasePrefix)).ToList();
                    if (data.Count > 0) { return View(data); }
                    else
                    {
                        TempData["productNameMessage"] = $"{productName} is not found";
                        return RedirectToAction("index", "product", new {area="auth"});
                    }


                }

                if (categoryId != null && subcategoryId!=null)
                {
                    var filterProducts = data.Where(x => x.Category.CategoryId == categoryId && x.Category.SubCategory.Id== subcategoryId).ToList();
                    return View(filterProducts);
                }
                if (categoryId != null)
                {
                    var filterProducts = data.Where(x => x.Category.CategoryId == categoryId).ToList();
                    return View(filterProducts);
                }

                if (ascending != null)
                {
                    data = data.OrderBy(x => x.SumofSales).ToList();

                }
                return View(data);
            }
            return View();
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
                var data = ProductHelper.ChangeTypeForUpdateGet(jsonString);
                var partialViewName = $"~/Areas/Auth/Views/Shared/PartialViews/ProductUpdate/{data.Type.ToLower()}.cshtml";


                return PartialView(partialViewName, data);


            }
            return View("index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditVM editVM, List<IFormFile>? productImages, ProductImageUpdateVm imageUpdateVm)
        {
         
            var errors = _prdEditValidator.GetValidationErrors(editVM);
            if (errors.Any())
            {
                ModelStateHelper.AddErrorsToModelState(ModelState, errors);
                var partialViewName = $"~/Areas/Auth/Views/Shared/PartialViews/ProductUpdate/{editVM.Type.ToLower()}.cshtml";
                ViewBag.Errors = errors;
                return PartialView(partialViewName, editVM);

            }

            var productData = editVM;
            if (productImages.Any())
            {

                imageUpdateVm.Images = new List<ProductImage>();

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
                                imageUpdateVm.Images.Add(productImageModel);
                            }

                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                        }
                       productData = ProductHelper.ChangeTypeForUpdatePost(editVM, imageUpdateVm);
                    }
                }
            }
            
            var uri = $"{apiUri}/edit";     
            var json = JsonConvert.SerializeObject(productData);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var result = await _httpClient.PutAsync(uri, content);
            if (result.IsSuccessStatusCode) { return RedirectToAction("index", "product", new { area = "auth"}); }
            else { return RedirectToAction("edit", "product", new { area = "auth" }); }

        }
        [HttpGet]
        public async Task<IActionResult> Delete(string productId)
        {
            var deleteProductUri = $"{apiUri}/delete/{productId}";
            var result = await _httpClient.DeleteAsync(deleteProductUri);
            if (result.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Successful";
                return RedirectToAction("index", "product", new { area = "auth" });

            }
            else
            {
                TempData["ErrorMessage"] = "Failed";
                return RedirectToAction("index", "product", new { area = "auth" });

            }
        }
        [HttpGet]
        public async Task<IActionResult> ChangeStatus(string id, int statusCode)
        {
            var changeStatusApiUri = $"https://localhost:7185/api/product/changestatus/{id}/{statusCode}";
            var response = await _httpClient.GetAsync(changeStatusApiUri);
            return RedirectToAction("index", "product", new { area = "auth" });
        }
        [HttpGet]
        public async Task<IActionResult> PreviewProduct(string id)
        {      
            var getProductUri = $"https://localhost:7185/api/product/productReview/{id}";
            var response = await _httpClient.GetAsync(getProductUri);
            if (response.IsSuccessStatusCode)
            {
                var jsonProduct = await response.Content.ReadAsStringAsync();
                var productData = JsonConvert.DeserializeObject<ProductVm>(jsonProduct);
                if (productData != null)
                {
                    return View(productData);
                }

            }
            return RedirectToAction("index", "product",new {area="auth"});
        }

        [HttpGet]
        public async Task<IActionResult> SelectCategory()
        {
            var categoryUri = "https://localhost:7185/api/category";
            var response = await _httpClient.GetAsync(categoryUri);
            if (response.IsSuccessStatusCode)
            {
                var categoryJson = await response.Content.ReadAsStringAsync();
                var categoryData = JsonConvert.DeserializeObject<List<CategoryVM>>(categoryJson);
                return View(categoryData);
            }

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> SelectCategory(string categoryName,string categoryId,string subCategoryName,string subCategoryId)
        {
            TempData["CategoryName"] = categoryName;
            TempData["CategoryId"] = categoryId;
            TempData["SubCategoryName"] = subCategoryName;
            TempData["SubCategoryId"] = subCategoryId;

            return RedirectToAction("Create", "product", new {area="auth"});

        }
        [HttpGet]
        public async Task<IActionResult> Create(ProductComprehensiveVM comprehensiveVM)
        {
            var categoryName = TempData["CategoryName"] as string;
            var categoryId = TempData["CategoryId"] as string;
            var subCategoryName = TempData["SubCategoryName"] as string;
            var subCategoryId = TempData["SubCategoryId"] as string;
            var model = (categoryName, categoryId, subCategoryName, subCategoryId);
            ViewBag.Categorydata = model;

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
                if (model.subCategoryName != null) 
                { 
                var partialViewName = $"~/Areas/Auth/Views/Shared/PartialViews/Product/{model.subCategoryName.ToLower()}.cshtml";
                    return PartialView(partialViewName);
                }
                if(comprehensiveVM.Category.SubCategory.Name!=null)
                {
                    var partialViewName = $"~/Areas/Auth/Views/Shared/PartialViews/Product/{comprehensiveVM.Category.SubCategory.Name.ToLower()}.cshtml";
                    return PartialView(partialViewName,comprehensiveVM);
                }
                

            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductComprehensiveVM comprehensiveVM, List<IFormFile> productImages)
        {
            var errors = _prdValidator.GetValidationErrors(comprehensiveVM);
            if (errors.Any())
            {
                ModelStateHelper.AddErrorsToModelState(ModelState, errors);
                var partialViewName = $"~/Areas/Auth/Views/Shared/PartialViews/Product/{comprehensiveVM.Category.SubCategory.Name.ToLower()}.cshtml";
                ViewBag.Errors = errors;
                return PartialView(partialViewName, comprehensiveVM);

            }

            if (comprehensiveVM.Images == null)
            {
                comprehensiveVM.Images = new List<ProductImage>();
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
                            comprehensiveVM.Images.Add(productImageModel);
                        }

                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                }
            }

            
            var uri = $"{apiUri}/create";
            comprehensiveVM.Type = comprehensiveVM.Category.SubCategory.Name;
            comprehensiveVM = ProductHelper.SetCreateProductType(comprehensiveVM);
            var json = JsonConvert.SerializeObject(comprehensiveVM);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(uri, content);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("index", "product", new { area = "auth" });
            }
            return View(comprehensiveVM);
        }

    }
}
