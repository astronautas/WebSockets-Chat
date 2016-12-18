using chat_server;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace backend
{
    class Program
    {
        static void Main(string[] args)
        {

            new Thread(new ThreadStart(() =>
            {
                var wssv = new WebSocketServer(IPAddress.Parse("127.0.0.1"), 5000);
                wssv.AddWebSocketService<MessageDispatcher>("/Chat");

                wssv.Start();

                Console.WriteLine("Websocket server listening...");
                Console.WriteLine("Press ENTER to stop the server");

                Console.ReadLine();
                Console.WriteLine("Stopping server");

                wssv.Stop();
            })).Start();

            Routes.Load(Router.Instance);
        }
    }
}
