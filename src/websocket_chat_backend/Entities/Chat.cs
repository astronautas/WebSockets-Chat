using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace websocket_chat_backend.Entities
{
    public class Chat
    {
        public int ChatId { get; set; }
        public int Name { get; set; }

        public List<User> Users { get; set; }
    }
}
