using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using websocket_chat_backend.Entities;

namespace backend.Controllers
{
    class PagesController
    {
        // Returns most active author as AUTHOR | MESSAGE_COUNT
        public static string MostActiveAuthors(string requestParams)
        {
            string topAuthors;
            string allMsgCount;

            using (var ctx = new ChatContext())
            {
                var query = (from msg in ctx.Messages
                             group msg.MessageId by msg.Author.Username into tA
                             select new { name = tA.Key != null ? tA.Key : "Unknown", msgCount = tA.Count()}).Take(5).AsEnumerable();

                topAuthors = JsonConvert.SerializeObject(query);
            }

            var response = new
            {
                Command = "top_authors",
                data =  JsonConvert.DeserializeObject(topAuthors)
     
            };

            return JsonConvert.SerializeObject(response);
        }
    }
}
