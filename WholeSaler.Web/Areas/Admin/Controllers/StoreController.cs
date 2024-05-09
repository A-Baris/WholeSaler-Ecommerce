using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WholeSaler.Web.Areas.Admin.Models.ViewModels.Store;

namespace WholeSaler.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class StoreController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public StoreController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }
        [HttpGet]
        public async Task<IActionResult> StoreApplications()
        {
            var getStoresUri = "https://localhost:7185/api/store";
            var result = await _httpClient.GetAsync(getStoresUri);
            if (result.IsSuccessStatusCode)
            {
                var json= await result.Content.ReadAsStringAsync();
                var stores = JsonConvert.DeserializeObject<List<StoreVM>>(json);
                var storeForWaitingApp = stores.Where(x=>x.AdminConfirmation == Models.Enums.AdminConfirmation.Waiting).ToList();
                return View(storeForWaitingApp);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ApproveApplication(string storeId)
        {
            
            var approveStoreUri = $"https://localhost:7185/api/store/ApproveApplication/{storeId}";
  
            var result = await _httpClient.GetAsync(approveStoreUri);
            if (result.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "";
                return RedirectToAction("StoreApplications", "store", new { area = "admin" });
            }
            TempData["errorMessage"] = "";
            return RedirectToAction("StoreApplications", "store", new { area = "admin" });
        }
        [HttpGet]
        public async Task<IActionResult> RejectApplication(string storeId)
        {
            
            var rejectStoreUri = $"https://localhost:7185/api/store/RejectApplication/{storeId}";          
            var result = await _httpClient.GetAsync(rejectStoreUri);
            if (result.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "";
                return RedirectToAction("StoreApplications", "store", new { area = "admin" });
            }
            TempData["errorMessage"] = "";
            return RedirectToAction("StoreApplications", "store", new { area = "admin" });
        }
    }
}
