using chat_server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace backend
{
    class MessageDispatcher : WebSocketBehavior
    {
        private static List<MessageDispatcher> _dispatchers = new List<MessageDispatcher>();

        public MessageDispatcher()
        {
            _dispatchers.Add(this);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _dispatchers.Remove(this);
            base.OnClose(e);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            dynamic jsonData = JsonConvert.DeserializeObject(e.Data);
            var command = jsonData.Command.Value;

            Action<string> callback = (data) => {
                var response = JsonConvert.DeserializeObject(data);


                Send(data);
            };

            Router.Instance.Route(command, e.Data, callback);
        }

        public static void MassSend(List<string> sessionKeys, string data)
        {

            foreach (var key in sessionKeys)
            {
                // Dispatcher - single connection to the user. All connections provide
                // session key as query argument
                var test = _dispatchers[0].Context.QueryString.Get("sessionKey");
                var test2 = key;

                var user = _dispatchers.Where(d => d.Context.QueryString.Get("sessionKey") == key).
                           FirstOrDefault();

                if (user != null)
                {
                    user.Send(data);
                }
            }
        }
    }
}
