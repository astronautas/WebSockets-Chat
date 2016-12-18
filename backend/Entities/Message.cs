using System;
using System.ComponentModel.DataAnnotations;

namespace websocket_chat_backend.Entities
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string Body { get; set; }
        public DateTime? Created_At { get; set; }
        
        public virtual User Author { get; set; }
        public virtual ChatRoom ChatRoom { get; set; }
    }
}
