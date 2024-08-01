using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities.Embeds.Message;
using WholeSaler.Entity.Entities.Embeds.Product;

namespace WholeSaler.Entity.Entities
{
    public class Message:BaseEntity
    {
        public string ReceiverId { get; set; }  
        public string ReceiverName { get; set; }  
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string? Header { get; set; }
        public string Body { get; set; }
        public List<MessageImage>? Images { get; set; }
    }
}
