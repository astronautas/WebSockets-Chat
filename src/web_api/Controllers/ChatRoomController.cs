using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using websocket_chat_backend.Entities;

namespace backend.Controllers
{
    class ChatRoomController
    {
        public static string OpenChat(string parameters)
        {
            var p = JObject.Parse(parameters);
            var sessionKey = p.GetValue("SessionKey").Value<string>();
            string result;
            
            using (var _chat = new ChatContext())
            {
                var id = (string)p["data"]["chatId"];

                // Create a new chat between current and provided users
                // If it exists, load previous one data
                if (id.Length != 0 && id != null)
                {
                    var room_data = JObject.Parse(Get(Int32.Parse(id)));

                    var response = new
                    {
                        Command = "chat_room_data",
                        data    = room_data
                    };

                    result = JsonConvert.SerializeObject(response);
                }
                else
                {
                    var user1 = _chat.Users.First<User>(u => u.SessionKey == sessionKey);
                    var emailToChatWith = (string)p["data"]["email"];
                    var user2 = _chat.Users.First<User>(u => u.Email == emailToChatWith);

                    var users = new List<User>() { user1, user2 };
                    var room_data = JObject.Parse(Create(users, _chat));

                    var response = new
                    {
                        Command = "chat_room_data",
                        data = room_data
                    };

                    result = JsonConvert.SerializeObject(response);
                }
            }


            return result;
        }

        public static string Create(List<User> Users, ChatContext ctx = null)
        {
            string result = null;

            if (ctx == null)
            {
                ctx = new ChatContext();
            }

            using (ctx)
            {
                var chat = new ChatRoom()
                {
                    Name = "Empty",
                    Participants = Users
                };

                var participants = from user in chat.Participants
                                   select new
                                   {
                                       username = user.Username,
                                       email = user.Email,
                                       status = true
                                   };

                ctx.ChatRooms.Add(chat);
                ctx.SaveChanges();

                var data = new
                {
                    participants = participants.ToList(),
                    messages     = new List<Message>(),
                    chat_id      = chat.ChatRoomId,
                    chat_name    = chat.Name
                };

                result = JsonConvert.SerializeObject(data);

                ctx.SaveChanges();
            }

            return result;
        }

        public static string Get(int id)
        {
            string result = null;

            using (var ctx = new ChatContext())
            {
                var chat = ctx.ChatRooms.Find(id);

                if (chat != null)
                {
                    var messages = from msg in chat.Messages
                                   select new
                                   {
                                       username = msg.Author?.Username,
                                       message = msg.Body
                                   };

                    var participants = from user in chat.Participants
                                       select new
                                       {
                                           username = user.Username,
                                           email    = user.Email,
                                           status   = true
                                       };

                    var data = new
                    {
                        participants = participants.AsEnumerable(),
                        messages   = messages.Skip(messages.Count() - 10).AsEnumerable(),
                        chat_id = chat.ChatRoomId,
                        chat_name = chat.Name
                    };

                    result = JsonConvert.SerializeObject(data);
                }
            }

            return result;
        }

        public static string Update(string requestData)
        {
            var p = JObject.Parse(requestData);
            var sessionKey = p.GetValue("SessionKey").Value<string>();
            string result;

            var id = (int)p["data"]["chatId"];
            var newName = (string)p["data"]["chat_room_name"];

            using (var ctx = new ChatContext())
            {
                var chatRoom = ctx.ChatRooms.Find(id);

                if (chatRoom != null)
                {
                    chatRoom.Name = newName;
                    ctx.SaveChanges();
                }
            }

            result = Get(id);

            return result;
        }

        public static void ChatUpdated(ChatRoom chatRoom)
        {
            // Inform all online chat room participants
            // that the room data has been updated
            var keys = (from user in chatRoom.Participants
                        select user.SessionKey).ToList();

            var data = new
            {
                Command = "chat_room_data"
            };

            MessageDispatcher.MassSend(keys, JsonConvert.SerializeObject(data));
        }
    }
}
