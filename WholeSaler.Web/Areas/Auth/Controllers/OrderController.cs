
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;
using System.Security.Claims;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Order;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.BaseProduct;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Store;
using WholeSaler.Web.Models.ViewModels.OrderVM;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;
using WholeSaler.Web.MongoIdentity;

namespace WholeSaler.Web.Areas.Auth.Controllers
{
    [Area("auth")]
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly HttpClient _httpClient;
        public OrderController(IHttpClientFactory httpClientFactory,UserManager<AppUser> userManager)
        {
            this.httpClientFactory = httpClientFactory;
           _userManager = userManager;
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<IActionResult> Index()
        {
           
            return View();
        }
        public async Task<IActionResult> OrderDetail(string orderId)
        {

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var storeId = user.StoreId;
                if (storeId != null)
                {
                    var orderListUri = $"https://localhost:7185/api/order/{orderId}";
                    var orderResponse = await _httpClient.GetAsync(orderListUri);
                    if (orderResponse.IsSuccessStatusCode)
                    {
                        var jsonString = await orderResponse.Content.ReadAsStringAsync();
                        var order = JsonConvert.DeserializeObject<OrderVM>(jsonString);
                       order.Products = order.Products.Where(x=>x.Store.StoreId==storeId).ToList();
                        decimal totalAmount = 0;
                       foreach (var prd in order.Products)
                        {
                           
                          totalAmount += (decimal)(prd.UnitPrice * prd.Quantity);
                         
                         
                        }
                       order.TotalOrderAmount = totalAmount;
                        return View(order);
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> PreparingStatus(string orderId,string productId)
        {
            var orderUri = $"https://localhost:7185/api/order/{orderId}";
            var order= await _httpClient.GetAsync(orderUri);
            if (order.IsSuccessStatusCode)
            {
                var jsonString = await order.Content.ReadAsStringAsync();
                var orderData = JsonConvert.DeserializeObject<OrderEditVm>(jsonString);
                foreach (var prd in orderData.Products)
                {
                    if (prd.Id==productId)
                    {
                        prd.OrderStatus = Web.Models.Enums.ProductOrderStatus.Preparing;
                    }
                }

                var orderEditUri = $"https://localhost:7185/api/order/edit";
                var json = JsonConvert.SerializeObject(orderData);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var orderResult = await _httpClient.PutAsync(orderEditUri, content);

            }


            return RedirectToAction("OrderDetail", "order", new { area = "auth", orderId = orderId } );
        }
        public async Task<IActionResult> ShippedStatus(string orderId, string productId)
        {
            var orderUri = $"https://localhost:7185/api/order/{orderId}";
            var order = await _httpClient.GetAsync(orderUri);
            if (order.IsSuccessStatusCode)
            {
                var jsonString = await order.Content.ReadAsStringAsync();
                var orderData = JsonConvert.DeserializeObject<OrderEditVm>(jsonString);
                foreach (var prd in orderData.Products)
                {
                    if (prd.Id == productId)
                    {
                        prd.OrderStatus = Web.Models.Enums.ProductOrderStatus.Shipped;
                    }
                }

                var orderEditUri = $"https://localhost:7185/api/order/edit";
                var json = JsonConvert.SerializeObject(orderData);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var orderResult = await _httpClient.PutAsync(orderEditUri, content);
                return RedirectToAction("OrderDetail", "order", new { area = "auth", orderId = orderId });

            }


            return RedirectToAction("OrderDetail", "order", new { area = "auth", orderId = orderId });
        }

        public async Task<IActionResult> PrintOrderDetail(string orderId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var storeId = user.StoreId;
                if (storeId != null)
                {
                    var orderListUri = $"https://localhost:7185/api/order/{orderId}";
                    var orderResponse = await _httpClient.GetAsync(orderListUri);
                    if (orderResponse.IsSuccessStatusCode)
                    {
                        var jsonString = await orderResponse.Content.ReadAsStringAsync();
                        var order = JsonConvert.DeserializeObject<OrderVM>(jsonString);
                        order.Products = order.Products.Where(x => x.Store.StoreId == storeId).ToList();
                        decimal totalAmount = 0;
                        foreach (var prd in order.Products)
                        {

                            totalAmount += (decimal)(prd.UnitPrice * prd.Quantity);


                        }
                        order.TotalOrderAmount = totalAmount;
                        CreatePdfWithTable(order);
                        return RedirectToAction("OrderDetail", "order", new { area = "auth", orderId = orderId });
                    }
                }
            }
            return RedirectToAction("OrderDetail", "order", new { area = "auth", orderId = orderId });
        }
        public void CreatePdfWithTable(OrderVM order)
        {
            // Create a new PDF document
            var document = new PdfDocument();
            document.Info.Title = "Order Details";

            // Create an empty page
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Define the font and starting point
            var font = new XFont("Verdana", 10, XFontStyleEx.Regular);
            var xPos = 50;
            var yPos = 50;


           

            // Draw company name at the top center
            gfx.DrawString("WholeSaller Company", new XFont("Verdana", 12, XFontStyleEx.Bold), XBrushes.Black,
                            new XRect(xPos, yPos, page.Width - 40, page.Height), XStringFormats.TopCenter);
            yPos += 20;
            gfx.DrawString($"{order.CreatedDate}", new XFont("Verdana", 12, XFontStyleEx.Bold), XBrushes.Black, new XRect(xPos, yPos, page.Width-60, page.Height), XStringFormats.TopRight);
            yPos += 30;
            // Draw title
            gfx.DrawString("Order Details", new XFont("Verdana", 12, XFontStyleEx.Bold), XBrushes.Black, new XRect(xPos, yPos, page.Width, page.Height), XStringFormats.TopLeft);

            yPos += 30; // Move down for the table

            // Draw table headers
            gfx.DrawString("Product Name", font, XBrushes.Black, new XRect(xPos, yPos, 100, 20), XStringFormats.TopLeft);
            gfx.DrawString("Quantity", font, XBrushes.Black, new XRect(xPos + 100, yPos, 100, 20), XStringFormats.TopLeft);
            gfx.DrawString("Unit Price", font, XBrushes.Black, new XRect(xPos + 200, yPos, 100, 20), XStringFormats.TopLeft);
            gfx.DrawString("Total Price", font, XBrushes.Black, new XRect(xPos + 300, yPos, 100, 20), XStringFormats.TopLeft);
          

            yPos += 30; // Move down for the rows

            // Draw table rows
            foreach (var product in order.Products)
            {
                gfx.DrawString(product.Name, font, XBrushes.Black, new XRect(xPos, yPos, 100, 20), XStringFormats.TopLeft);
                gfx.DrawString(product.Quantity.ToString(), font, XBrushes.Black, new XRect(xPos + 100, yPos, 100, 20), XStringFormats.TopLeft);
                gfx.DrawString($"{product.UnitPrice} TL", font, XBrushes.Black, new XRect(xPos + 200, yPos, 100, 20), XStringFormats.TopLeft);
                gfx.DrawString($"{product.UnitPrice * product.Quantity} TL", font, XBrushes.Black, new XRect(xPos + 300, yPos, 100, 20), XStringFormats.TopLeft);
               
                yPos += 20; // Move down for the next row
            }

            // Draw total order amount
            yPos += 20;
            gfx.DrawString($"Total Order Amount: {order.TotalOrderAmount} TL", font, XBrushes.Black, new XRect(xPos, yPos, 500, 20), XStringFormats.TopLeft);
            yPos += 20;

            gfx.DrawString("Adress:", new XFont("Verdana", 12, XFontStyleEx.Bold), XBrushes.Black, new XRect(xPos, yPos, page.Width, page.Height), XStringFormats.TopLeft);
            yPos += 20;      
            gfx.DrawString($"Country :{order.ShippingAddress.Country}", font, XBrushes.Black, new XRect(xPos, yPos, page.Width, page.Height), XStringFormats.TopLeft);
            yPos += 20;
            gfx.DrawString($"City :{order.ShippingAddress.City}", font, XBrushes.Black, new XRect(xPos, yPos, page.Width, page.Height), XStringFormats.TopLeft);
            yPos += 20;
            gfx.DrawString($"District :{order.ShippingAddress.District}", font, XBrushes.Black, new XRect(xPos, yPos, page.Width, page.Height), XStringFormats.TopLeft);
            yPos += 20;
            gfx.DrawString($"Neighborhood :{order.ShippingAddress.Neighborhood}", font, XBrushes.Black, new XRect(xPos, yPos, page.Width, page.Height), XStringFormats.TopLeft);
            yPos += 20;
            gfx.DrawString($"Apartment :{order.ShippingAddress.ApartmentInfo}", font, XBrushes.Black, new XRect(xPos, yPos, page.Width, page.Height), XStringFormats.TopLeft);
           


            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = $"{order.Username}_{timestamp}.pdf";

            // Define a custom file path to save the PDF
            // Example: Saving in the project's OrderDocuments folder
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "OrderDocuments");
            string filePath = Path.Combine(directoryPath, fileName);

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Save the document
            document.Save(filePath);
            //PrintPdf(filePath);

        }
        //private void PrintPdf(string filePath)
        //{
        //    try
        //    {
        //        // Start the process to print the PDF file
        //        ProcessStartInfo printProcess = new ProcessStartInfo
        //        {
        //            FileName = filePath,
        //            UseShellExecute = true,
        //            Verb = "print"
        //        };

        //        using (Process process = Process.Start(printProcess))
        //        {
        //            process.WaitForExit();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions
        //        Console.WriteLine("An error occurred while printing the PDF: " + ex.Message);
        //    }
        //}
    }
}
