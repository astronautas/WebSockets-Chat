using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace websocket_chat_backend.Entities
{
    public class ChatContext : DbContext
    {
        public DbSet<Chat>    Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User>    User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./ChatDb.db");
        }
    }
}
