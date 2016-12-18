using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace websocket_chat_backend.Entities
{
    public class ChatRoom
    {
        public static event Action<ChatRoom> OnUpdate = delegate {};

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ChatRoomId { get;set; }

        public string Name { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<User> Participants { get; set; }
    }
}
