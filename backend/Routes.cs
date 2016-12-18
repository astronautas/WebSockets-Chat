using backend.Interfaces;
using chat_server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Controllers;

namespace backend
{
    class Routes
    {
        public static void Load(IRoute router)
        {
            router.AddAsyncRoute("contacts", ContactsController.GetUserContacts);
            router.AddAsyncRoute("add_contact", ContactsController.Create);
            router.AddAsyncRoute("delete_contact", ContactsController.Delete);
            router.AddAsyncRoute("chat", ChatRoomController.OpenChat);
            router.AddAsyncRoute("update_chat", ChatRoomController.Update);
            router.AddAsyncRoute("send_message", MessageController.Create);
            router.AddAsyncRoute("top_authors", PagesController.MostActiveAuthors);
        }
    }
}
