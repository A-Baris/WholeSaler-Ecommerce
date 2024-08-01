using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using WholeSaler.Web.Models.ViewModels.Message;
using WholeSaler.Web.MongoIdentity;

namespace WholeSaler.Web.Areas.Auth.Controllers
{
    [Area("auth")]
    public class MessageController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public MessageController(UserManager<AppUser> userManager,IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }
        [HttpGet]
        public async Task<IActionResult> SendMessage(string receiverId, string receiverName)
        {
            var receiverModel = (receiverId: receiverId, receiverName: receiverName);
            ViewData["ReceiverId"] = receiverModel.receiverId;
            ViewData["ReceiverName"] = receiverModel.receiverName;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageVM sendMessageVM)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            sendMessageVM.SenderName = user.UserName;
            sendMessageVM.SenderId = userId.ToString();

            var messageCreateUri = "https://localhost:7185/api/message/create";
            var messageJson = JsonConvert.SerializeObject(sendMessageVM);
            var messageContent = new StringContent(messageJson, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(messageCreateUri, messageContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("messagebox", "message", new {area="auth"});
            }
            return View(sendMessageVM);
        }






        [HttpGet]
        public async Task<IActionResult> MessageBox()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.UserName = user.UserName;
            var receivedMessageUri = $"https://localhost:7185/api/message/messagebox/{user.UserName}/{user.Id}";
            var response = await _httpClient.GetAsync(receivedMessageUri);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var senders = JsonConvert.DeserializeObject<List<string>>(jsonData);
                return View(senders);
            }
            return RedirectToAction("messagebox", "message", new { area = "auth" });

        }
        [HttpGet]
        public async Task<IActionResult> ReceivedMessageFrom(string sender)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            var receivedMessageUri = $"https://localhost:7185/api/message/receivedMessagefrom/{sender}/{user.UserName}";
            var response = await _httpClient.GetAsync(receivedMessageUri);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.ReceiverName = sender;
                var receiverUser = await _userManager.FindByNameAsync(sender);
                ViewBag.ReceiverId = receiverUser.Id.ToString();
                var jsonData = await response.Content.ReadAsStringAsync();
                var messages = JsonConvert.DeserializeObject<List<ReceivedMessageVM>>(jsonData);
                return PartialView(messages);
            }
            return RedirectToAction("messagebox", "message", new { area = "auth" });

        }
    }
}

