using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using WholeSaler.Api.Controllers.Base;
using WholeSaler.Business.AbstractServices;

namespace WholeSaler.Api.Controllers
{
    [Route("ws/order")]
    [ApiController]
    public class OrderWebSocketController : BaseController
    {
        private static List<WebSocket> webSocketClients = new List<WebSocket>();
        private readonly IOrderServiceWithRedis _orderServiceWithRedis;

        public OrderWebSocketController(IOrderServiceWithRedis orderServiceWithRedis)
        {
            _orderServiceWithRedis = orderServiceWithRedis;
        }

        [HttpGet]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                webSocketClients.Add(webSocket);

                // Start listening for messages
                await ReceiveMessages(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        private async Task ReceiveMessages(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;
            string statusFilter = "Active"; // Default status filter

            while (webSocket.State == WebSocketState.Open)
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine("Message received from client: " + message); // Log received message
                    var request = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

                    if (request.ContainsKey("action") && request["action"] == "filter")
                    {
                        statusFilter = request["status"];
                    }

                    await SendOrderUpdates(webSocket, statusFilter);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    webSocketClients.Remove(webSocket);
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocket client", CancellationToken.None);
                }
            }
        }

        private async Task SendOrderUpdates(WebSocket webSocket, string statusFilter)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var orders = await _orderServiceWithRedis.GetAll();
                    orders = orders.OrderByDescending(x => x.CreatedDate).ToList();
                    var filteredOrders = orders
                        .Where(order => order.Status.ToString() == statusFilter || (statusFilter == "Active" && order.Status.ToString() == null))
                        .ToList();

                    var orderDataJson = JsonConvert.SerializeObject(filteredOrders);
                    var encodedMessage = Encoding.UTF8.GetBytes(orderDataJson);

                    await webSocket.SendAsync(new ArraySegment<byte>(encodedMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending data: " + ex.Message);
                }

                await Task.Delay(10000); // Adjust this delay as needed
            }
        }
    }
}
