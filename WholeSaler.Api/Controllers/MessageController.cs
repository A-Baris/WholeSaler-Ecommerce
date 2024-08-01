using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WholeSaler.Api.Controllers.Base;
using WholeSaler.Api.DTOs.Message;
using WholeSaler.Business.AbstractServices;
using WholeSaler.Entity.Entities;

namespace WholeSaler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : BaseController
    {
        private readonly IMessageServiceWithRedis _messageServiceWithRedis;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageServiceWithRedis messageServiceWithRedis,ILogger<MessageController> logger)
        {
           _messageServiceWithRedis = messageServiceWithRedis;
            _logger = logger;
        }

        [HttpGet("messagebox/{receiverName}/{receiverId}")]
        public async  Task<IActionResult> MessageBox(string receiverName,string receiverId)
        {
            var messageList = await _messageServiceWithRedis.GetAll();
            var uniqueSenderNames = messageList
                                 .Where(x => x.ReceiverName == receiverName)
                                 .Select(x => x.SenderName)
                                 .Distinct()
                                 .ToList();
            return Ok(uniqueSenderNames);
          
        }

        [HttpGet("receivedMessagefrom/{senderName}/{receiverName}")]
        public async Task<IActionResult> ReceivedMessageFrom(string senderName, string receiverName)
        {
            var messageList = await _messageServiceWithRedis.GetAll();
            var sentMessage = messageList.Where(x =>
         (x.ReceiverName == receiverName && x.SenderName == senderName) ||
         (x.SenderName == receiverName && x.ReceiverName == senderName))
         .OrderBy(x => x.CreatedDate) // Order messages by creation date
         .ToList();
            return Ok(sentMessage);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(Message message)
        {
            var result = await _messageServiceWithRedis.Create(message);
            return Ok(result);
        }
    }
}
