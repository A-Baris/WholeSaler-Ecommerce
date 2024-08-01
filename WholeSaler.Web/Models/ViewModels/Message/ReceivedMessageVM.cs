namespace WholeSaler.Web.Models.ViewModels.Message
{
    public class ReceivedMessageVM
    {
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public List<MessageImageVM>? Images { get; set; } 
        public string Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
