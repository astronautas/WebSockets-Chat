using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using websocket_chat_backend.Entities;

namespace backend.Controllers
{
    class MessageController
    {
        private static ChatContext _chat;

        public static string Create(string parameters)
        {
            var p = JObject.Parse(parameters);
            var sessionKey = p.GetValue("SessionKey").Value<string>();
            string result;

            var body   = (string)p["data"]["msg"];
            var chatId = (int)p["data"]["chatId"];
            

            using (var ctx = new ChatContext())
            {
                var chatRoom = ctx.ChatRooms.Find(chatId);

                var user = ctx.Users.First<User>(u => u.SessionKey == sessionKey);

                var msg = new Message() { Body = body, ChatRoom = chatRoom, Author =  user };
                chatRoom.Messages.Add(msg);

                ctx.SaveChanges();

                var response = new
                {
                    Command = "chat_room_data",
                    data = JsonConvert.DeserializeObject(ChatRoomController.Get(chatRoom.ChatRoomId))
                };

                result = JsonConvert.SerializeObject(response);

                // Make all the chat users update their chats
                var keys = chatRoom.Participants.Select(s => s.SessionKey).ToList();
                MessageDispatcher.MassSend(keys, result);
            }

            return result;
        }

        public static void Get()
        {
        }

        public static void Update()
        {

        }

        public static void Delete()
        {

        }
    }
}
