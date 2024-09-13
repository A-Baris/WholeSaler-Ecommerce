using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Text.Unicode;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.Category;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Category;
using WholeSaler.Web.FluentValidation.Configs;
using WholeSaler.Web.FluentValidation.Validators.Category;
using WholeSaler.Web.Utility;

namespace WholeSaler.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles ="admin")]
    
    
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IValidationService<CategoryVM> _categoryCreateValidate;
        private readonly HttpClient _httpClient;

        public CategoryController(IHttpClientFactory httpClientFactory,IValidationService<CategoryVM> categoryCreateValidate)
        {
            _httpClientFactory = httpClientFactory;
           _categoryCreateValidate = categoryCreateValidate;
            _httpClient = _httpClientFactory.CreateClient();
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoryUri = "https://localhost:7185/api/category";
            var response = await _httpClient.GetAsync(categoryUri);
            if (response.IsSuccessStatusCode)
            {
                var categoryJson = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<CategoryVM>>(categoryJson);
                return View(data);
            }
            return View(null);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM categoryCreate,string subCategoryNames)
        {
             
            string[] subCategoriesArray = subCategoryNames.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (subCategoriesArray.Length > 0)
            {
                categoryCreate.SubCategories = new List<SubCategoryVM>();
                for (int i = 0; i < subCategoriesArray.Length; i++)
                {
                    var newSubCategory = new SubCategoryVM();
                    newSubCategory.Id = ObjectId.GenerateNewId().ToString();
                    newSubCategory.Name= subCategoriesArray[i];
                    categoryCreate.SubCategories.Add(newSubCategory);

                }
            }

            var categoryCreateUri = "https://localhost:7185/api/category";
            var json = JsonConvert.SerializeObject(categoryCreate);
            var content = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var response = await _httpClient.PostAsync(categoryCreateUri,content);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Category is created";
                return RedirectToAction("create", "category", new {area="admin"});
            }
            TempData["ErrorMessage"] = $"#Error : {response.StatusCode}";
            return RedirectToAction("create", "category", new { area = "admin" });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var getCategory = $"https://localhost:7185/api/category/{id}";
            var response = await _httpClient.GetAsync(getCategory);
            if (response.IsSuccessStatusCode) { 
            var categoryJson = await response.Content.ReadAsStringAsync();
            var category = JsonConvert.DeserializeObject<CategoryVM>(categoryJson);
                return View(category);
            }
            TempData["ErrorMessage"] = "Category Id is not found";
            return View("index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryVM category)
        {
            var errors = _categoryCreateValidate.GetValidationErrors(category);
            if (errors.Any())
            {
                ModelStateHelper.AddErrorsToModelState(ModelState, errors);
                return View(category);
            }
            var categoryEditUri = "https://localhost:7185/api/category/edit";
            var categoryJson = JsonConvert.SerializeObject(category);
            var categoryContent = new StringContent(categoryJson, System.Text.Encoding.UTF8, "application/json");
            //SetAuthHeader.SetAuthorizationHeader(Request);
            var response = await _httpClient.PutAsync(categoryEditUri, categoryContent);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Updated is successful";
            return RedirectToAction("index", "category", new {area="admin"}); 
            }

            TempData["ErrorMessage"] = "Unexpected error (Try Again)";
            return RedirectToAction("index", "category", new { area = "admin" });
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var categoryDeleteUri = $"https://localhost:7185/api/category/delete/{id}";
            var response = await _httpClient.DeleteAsync(categoryDeleteUri);
            if(response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Deleted is successful";
                return RedirectToAction("index", "category", new { area = "admin" });
            }
            TempData["ErrorMessage"] = "Unexpected error (Try Again)";
            return RedirectToAction("index", "category", new {area="admin"});
        }
    }
}
