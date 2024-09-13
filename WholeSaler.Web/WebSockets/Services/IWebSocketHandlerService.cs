using System.Net.WebSockets;

namespace WholeSaler.Web.WebSockets.Services
{
    public interface IWebSocketHandlerService
    {
        Task HandleAsync(WebSocket webSocket, string message);
    }
}
