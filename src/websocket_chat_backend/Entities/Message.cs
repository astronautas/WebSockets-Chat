using System.ComponentModel.DataAnnotations;

namespace websocket_chat_backend.Entities
{
    public class Message
    {
        public int MessageId;
        public string Body { get; set; }

        [Timestamp]
        public byte[] Created_At { get; set; }
        
        public User Author { get; set; }
    }
}
