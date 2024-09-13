using System.Net.WebSockets;
using System.Text;
using WholeSaler.Web.WebSockets.Services;
using static System.Net.WebRequestMethods;

namespace WholeSaler.Web.WebSockets.EntityHandlers
{
    public class OrderWebSocketHandler : IWebSocketHandlerService
    {
        public async Task HandleAsync(WebSocket webSocket, string message)
        {
            if (message.Contains("GetOrders"))
            {
                HttpClient client = new HttpClient();
                var apiUri = "https://localhost:7185/api/order";
                var orderResponse = await client.GetAsync(apiUri);
                var ordersJson = await orderResponse.Content.ReadAsStringAsync();
                var response = Encoding.UTF8.GetBytes(ordersJson);
                await webSocket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
