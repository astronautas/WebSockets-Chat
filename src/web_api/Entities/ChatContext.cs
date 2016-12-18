using System.Data.Entity;

namespace websocket_chat_backend.Entities
{
    public class ChatContext : DbContext
    {
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Message>  Messages { get; set; }
        public DbSet<User>     Users { get; set; }

        public ChatContext() : base()
        {

        }
    }
}
