using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text;
using WholeSaler.Web.Hubs.Services;
using WholeSaler.Web.Models.ViewModels.Message;

namespace WholeSaler.Web.Hubs
{
    public class MessageHub:Hub<IMessageHub>
    {
        private static int ConnectedClientNumber = 0;
       


        public override async Task OnConnectedAsync()
        {
            ConnectedClientNumber++;
            await Clients.All.ReceiveConnectedClientNumber(ConnectedClientNumber);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClientNumber--;
            await Clients.All.ReceiveConnectedClientNumber(ConnectedClientNumber);

            await base.OnDisconnectedAsync(exception);
        }
        public async Task BroadcastMessage(string message)
        {
            await Clients.All.ReceiveMessage(message);

        }
        public async Task BroadcastTypedMessage(SendMessageVM message)
        {
            var senderUserId = Context.UserIdentifier;
            message.SenderId = "12345";
            message.SenderName = "signalR";
            message.ReceiverId = "6789";
            message.ReceiverName = "signalRreceiver";


            HttpClient client = new HttpClient();
            var messageUri = "https://localhost:7185/api/message/create";
            string json = JsonConvert.SerializeObject(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(messageUri, content);



            await Clients.All.ReceiveTypedMessage(message);

        }

        public async Task BroadcastMessageForPrivateClient(string message)
        {
            await Clients.Caller.ReceiveMessageForPrivateClient(message);

        }
        public async Task BroadcastMessageForOthersClient(string message)
        {
            await Clients.Others.ReceiveMessageForOthersClient(message);

        }

        public async Task BroadcastMessageForSelectedClient(string connectionId, string message)
        {
            await Clients.Client(connectionId).ReceiveMessageForSelectedClient(message);

        }

        public async Task BroadcastGroupClientMessage(string groupName, string message)
        {
            await Clients.Group(groupName).ReceiveMessageForGroupClients(message);
        }

        public async Task JoinToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessageForPrivateClient($"{groupName} grubuna katıldın.");
            await Clients.Group(groupName).ReceiveMessageForGroupClients($"User-{Context.ConnectionId} gruba katıldı.");
        }
        public async Task LeaveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessageForPrivateClient($"{groupName} 'dan ayrıldınız.");
            await Clients.Group(groupName).ReceiveMessageForGroupClients($"Kullanıcı({Context.ConnectionId}) {groupName} grubundan ayrıldı.");
        }
    }
}
