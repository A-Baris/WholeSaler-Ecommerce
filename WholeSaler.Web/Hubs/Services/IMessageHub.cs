using WholeSaler.Web.Models.ViewModels.Message;

namespace WholeSaler.Web.Hubs.Services
{
    public interface IMessageHub
    {
        Task ReceiveMessage(string message);
        Task ReceiveTypedMessage(SendMessageVM message);
        Task ReceiveConnectedClientNumber(int clientNumber);
        Task ReceiveMessageForPrivateClient(string message);
        Task ReceiveMessageForOthersClient(string message);
        Task ReceiveMessageForSelectedClient(string message);
        Task ReceiveMessageForGroupClients(string message);
    }
}
