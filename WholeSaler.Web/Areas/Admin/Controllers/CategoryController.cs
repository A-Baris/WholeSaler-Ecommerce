using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Unicode;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.Category;

namespace WholeSaler.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    
    
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM categoryCreate)
        {
            var categoryCreateUri = "https://localhost:7185/api/category";
            var json = JsonConvert.SerializeObject(categoryCreate);
            var content = new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            var result = await _httpClient.PostAsync(categoryCreateUri,content);
            if (result.IsSuccessStatusCode)
            {
                TempData["success"] = "Category is created";
                return RedirectToAction("create", "category", new {area="admin"});
            }
            return View();
        }
    }
}
