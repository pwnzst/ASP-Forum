using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Chat.Models
{
    public class ChatContext : DbContext
    {
        public DbSet<User>    Users    { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Theme>   Themes   { get; set; }
        public DbSet<Post>    Posts    { get; set; }

        public ChatContext() : base("chatDB") { Context = this; }

        public static ChatContext Context { get; set; }

        /*public ChatContext() : base("chatMy")
        { }
            String createQuery = "CREATE TABLE IF NOT EXISTS Users ( ";
            bool f1 = true;
            bool pk = false;
            foreach (System.Reflection.PropertyInfo p in typeof(User).GetProperties())
            {
                if (f1) f1 = false;
                else createQuery += ", ";

                if (p.Name.ToUpper() == "ID") pk = true;

                createQuery += p.Name + " ";
                switch (p.PropertyType.Name)
                {
                    case "Int32": createQuery += "INT"; break;
                    case "String":
                    case "string": createQuery += "VARCHAR(128)"; break;
                    case "DateTime": createQuery += "DATETIME"; break;
                    case "Nullable`1":
                        if (p.PropertyType.FullName.Contains("DateTime"))
                        {
                            createQuery += "DATETIME"; break;
                        }
                        if (p.PropertyType.FullName.Contains("Int32"))
                        {
                            createQuery += "INT"; break;
                        }
                        break;
                    default: createQuery += "VARCHAR(32)"; break;
                }

            }
            if (pk) createQuery += ",primary key(id)";
            createQuery += ") ENGINE=InnoDB DEFAULT CHARSET=utf8";
            var con = new MySql.Data.MySqlClient.MySqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["chatMy"].ConnectionString);
            con.Open();
            new MySql.Data.MySqlClient.MySqlCommand(createQuery, con).ExecuteNonQuery();

        }*/
    }
}