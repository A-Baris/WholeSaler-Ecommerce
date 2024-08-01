using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.Category;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.SubCategory;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Category;

namespace WholeSaler.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class SubCategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public SubCategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }
        public async Task<IActionResult> Create()
        {
            var categoryUri = "https://localhost:7185/api/category";
            var response = await _httpClient.GetAsync(categoryUri);
            if (response.IsSuccessStatusCode) { 
            
                var data = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<CategoryVM>>(data);
                ViewBag.Categories = categories;
                return View();
            }
            return View();
        }
        public async Task<IActionResult> Create(SubCategoryCreateVM subCategoryCreateVM)
        {
            var categoryUri = "https://localhost:7185/api/category";
            var response = await _httpClient.GetAsync(categoryUri);
            if (response.IsSuccessStatusCode)
            {

                var data = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<CategoryVM>>(data);
                ViewBag.Categories = categories;
                return View();
            }
            return View();
        }
    }
}
