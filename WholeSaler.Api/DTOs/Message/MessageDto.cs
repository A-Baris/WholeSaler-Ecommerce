using WholeSaler.Entity.Entities.Embeds.Message;

namespace WholeSaler.Api.DTOs.Message
{
    public class MessageDto
    {
        public string Id { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public List<MessageImage>? Images { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
