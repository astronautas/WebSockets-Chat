using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace websocket_chat_backend.Entities
{
    public class User : IEquatable<User>
    {
        [Key]
        [StringLength(100)]
        public string Email { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public virtual ICollection<User>     Contacts { get; set; }
        public virtual ICollection<User>     IsContactOf { get; set; }
        public virtual ICollection<ChatRoom> ChatRooms { get; set; }

        [Index("Session_Key_Index", 1)]
        [StringLength(20)]
        public string SessionKey { get; set; }

        public User()
        {

        }

        public bool Equals(User other)
        {
            return other.Email == Email;
        }
    }
}
