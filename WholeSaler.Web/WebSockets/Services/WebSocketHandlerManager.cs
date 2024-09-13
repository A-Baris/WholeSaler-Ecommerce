using Microsoft.AspNet.SignalR.WebSockets;
using System.Net.WebSockets;
using System.Text;

namespace WholeSaler.Web.WebSockets.Services
{
    public class WebSocketHandlerManager
    {
        private readonly IDictionary<string, IWebSocketHandlerService> _handlers;

        public WebSocketHandlerManager(IEnumerable<IWebSocketHandlerService> handlers)
        {
            // Gelen handler'ları isimlerinden dinamik olarak ayırıyoruz
            _handlers = handlers.ToDictionary(h => h.GetType().Name.Replace("WebSocketHandler", ""), h => h);
        }

        public async Task HandleAsync(WebSocket webSocket, string message)
        {
            // Mesaja göre doğru handler'ı bul ve işle
            foreach (var handlerKey in _handlers.Keys)
            {
                if (message.Contains(handlerKey))
                {
                    await _handlers[handlerKey].HandleAsync(webSocket, message);
                    return;
                }
            }

            // Eğer uygun bir handler bulunamazsa
            var errorMessage = Encoding.UTF8.GetBytes("No handler found for this entity.");
            await webSocket.SendAsync(new ArraySegment<byte>(errorMessage), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
