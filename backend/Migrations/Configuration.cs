namespace backend.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using websocket_chat_backend.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<websocket_chat_backend.Entities.ChatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "websocket_chat_backend.Entities.ChatContext";
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(websocket_chat_backend.Entities.ChatContext context)
        {
            // Deletes all data, from all tables, except for __MigrationHistory
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'IF OBJECT_ID(''?'') NOT IN (ISNULL(OBJECT_ID(''[dbo].[__MigrationHistory]''),0)) DELETE FROM ?'");
            context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");

            var lukas = new User()
            {
                Email = "lukas@gmail.net",
                SessionKey = "12345678912345678900",
                Username = "Lukas"
            };

            var tom = new User()
            {
                Email = "tomas@gmail.net",
                SessionKey = "12345678912345678911",
                Username = "Tomas"
            };

            var pedro = new User()
            {
                Email = "pedro@gmail.net",
                SessionKey = "12345678912345678912",
                Username = "Pedro"
            };

            var antuan = new User()
            {
                Email = "antuan@gmail.net",
                SessionKey = "12345678912345678712",
                Username = "Antuan"
            };

            lukas.Contacts = new List<User>();
            lukas.Contacts.Add(tom);

            context.Users.Add(lukas);
            context.Users.Add(tom);
            context.Users.Add(pedro);
            context.Users.Add(antuan);

            //context.Users.Add(new User()
            //{
            //    Email = "peter@gmail.net",
            //    Username = "Peter"
            //});

            //context.Users.Add(new User()
            //{
            //    Email = "solomon@gmail.net",
            //    Username = "Solomon"
            //});

            //context.SaveChanges();
        }
    }
}
