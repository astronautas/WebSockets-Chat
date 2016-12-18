using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using websocket_chat_backend.Entities;

namespace backend.Controllers
{
    class ContactsController
    {
        public static string GetUserContacts(string parameters)
        {
            var p = JObject.Parse(parameters);
            var sessionKey = p.GetValue("SessionKey").Value<string>();
            string result = "";

            User user;
            DataTable contacts;
            DataTable allContacts;
            String contactInfo;

            using (var ctx = new ChatContext())
            {
                user = ctx.Users.First<User>(u => u.SessionKey == sessionKey);
            }

            // Select contact emails
            contacts = new DataTable();

            var cstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=websocket_chat_backend.Entities.ChatContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var query = "SELECT User_Email1 FROM UserUsers WHERE User_Email = @user_email";
            var allContactsQuery = "SELECT Username, Email FROM Users";

            using (var sqlConnection = new SqlConnection(cstring))
            {
                var cmd = new SqlCommand(query, sqlConnection);
                var param = new SqlParameter("user_email", user.Email);
                cmd.Parameters.Add(param);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(contacts);

                    string userEmail;

                    using (var ctx = new ChatContext())
                    {
                        user = ctx.Users.First<User>(u => u.SessionKey == sessionKey);
                        var userContacts = user.Contacts.AsEnumerable();

                        var queryResult = from contactRow in contacts.AsEnumerable()
                                      join contactFullRow in userContacts
                                      on contactRow["User_Email1"] equals contactFullRow.Email
                                      select new
                                      {
                                          username = contactFullRow.Username,
                                          email = contactFullRow.Email,
                                          status = true,
                                          chatId = (ctx.ChatRooms.Where(
                                                    r => r.Participants.ToList().FirstOrDefault(
                                                           u => u.Email == user.Email) != null &&
                                                         r.Participants.ToList().FirstOrDefault(
                                                           u => u.Email == contactFullRow.Email) != null
                                                ).FirstOrDefault())?.ChatRoomId
                                      };
                        contactInfo = JsonConvert.SerializeObject(queryResult);
                    }

                }
            }


            var response = new
            {
                Command = "contacts_data",
                contacts = JsonConvert.DeserializeObject(contactInfo)
            };

            return JsonConvert.SerializeObject(response);
        }

        public static string Create(string parameters)
        {
            var p = JObject.Parse(parameters);
            var sessionKey = p.GetValue("SessionKey").Value<string>();
            string result;

            var emailToAdd = (string)p["data"]["email"];

            using (var _chat = new ChatContext())
            {
                var user = _chat.Users.First<User>(u => u.SessionKey == sessionKey);
                var userToAdd = _chat.Users.Find(emailToAdd);

                if (userToAdd != null && !user.Contacts.Contains(userToAdd))
                {
                    user.Contacts.Add(userToAdd);
                    _chat.SaveChanges();

                    result = GetUserContacts(parameters);
                }
                else
                {
                    var response = new
                    {
                        response = "Operation failed",
                        responseCode = 400
                    };

                    result = JsonConvert.SerializeObject(response);
                }
            }

            return result;
        }

        public static string Delete(string parameters)
        {
            var p = JObject.Parse(parameters);
            var sessionKey = p.GetValue("SessionKey").Value<string>();
            string result;

            var emailToDelete = (string)p["data"]["email"];

            using (var ctx = new ChatContext())
            {
                var user = ctx.Users.First<User>(u => u.SessionKey == sessionKey);
                var contact = user.Contacts.Where(c => c.Email == emailToDelete).FirstOrDefault();

                if (contact != null)
                {
                    user.Contacts.Remove(contact);
                }

                ctx.SaveChanges();
            }

            result = GetUserContacts(parameters);

            return result;
        }

        private static bool IsOnline(User user)
        {

            return true;
        }
    }
}
