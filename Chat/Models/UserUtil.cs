using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace Chat.Models
{
    /// <summary>
    /// The <c>UserUtil</c> class
    /// </summary>
    public class UserUtil
    {
        /// <summary>
        /// It gets user shown. 
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>What is shown (login/name/email)</returns>
        public static String GetUserShown(int userId)
        {
            var user = GetUser(userId);
            if (user != null)
            {
                switch (user.ShownData)
                {
                    case 1: return user.RealName;
                    case 2: return user.Email;
                    default: return user.Login;
                }
            }
            return null;
        }

        /// <summary>
        /// It gets user by his Id
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>user</returns>
        /// <example>
        /// <code>
        /// return Models.ChatContext.Context.Users.Find(userId);
        /// </code></example>
        public static User GetUser(int userId)
        {
            return Models.ChatContext.Context.Users.Find(userId);
        }

        /// <summary>
        /// It gets user by his authData
        /// </summary>
        /// <param name="authData">user authData</param>
        /// <returns>user</returns>
        public static User GetUser(AuthData authData)
        {
            var user = from u in Models.ChatContext.Context.Users
                       where u.Login == authData.UserLogin
                       select u;
            foreach (var u in user)
            {
                if (
                    Utils.GetHashByPassSalt(authData.UserPassword, u.PassSalt)
                    ==
                    u.PassHash)
                {
                    return u;
                }
            }
            return null;
        }

        /// <summary>
        /// It counts posts, where user is an author.
        /// It creates a query to database to find out how many posts has created the user.
        /// </summary>
        /// <param name="userId">user id </param>
        /// <returns>ammount of posts</returns>
        public static int GetPostCnt(int userId)
        {
            var user = Models.ChatContext.Context.Users.Find(userId);
            if (user == null)
                return 0;
            return (from p in Models.ChatContext.Context.Posts
                    where p.Id_Author == userId
                    select p).Count();
        }

        /// <summary>
        /// It counts themes, where user is an author.
        /// It creates a query to database to find out how many themes has created the user.
        /// </summary>
        /// <param name="userId">user id </param>
        /// <returns>ammount of themes</returns>
        public static int GetThemeCnt(int userId)
        {
            var user = Models.ChatContext.Context.Users.Find(userId);
            if (user == null)
                return 0;
            return (from t in Models.ChatContext.Context.Themes
                    where t.Id_Author == userId
                    select t).Count();
        }

        /// <summary>
        /// It counts sections, where user is an author.
        /// It creates a query to database to find out how many sections has created the user.
        /// </summary>
        /// <param name="userId">user id </param>
        /// <returns>ammount of sections</returns>
        public static int GetSectionsCnt(int userId)
        {
            var user = Models.ChatContext.Context.Users.Find(userId);
            if (user == null)
                return 0;
            return (from s in Models.ChatContext.Context.Sections
                    where s.Id_Author == userId
                    select s).Count();

        }

        /// <summary>
        /// It gets the last post of the current user.
        /// It creates a query to database to find out the last post that was created/sent by the user.
        /// </summary>
        /// <param name="userId">user id </param>
        /// <returns>post</returns>
        public static Post GetLastPost(int userId)
        {
            var user = Models.ChatContext.Context.Users.Find(userId);

            //if (user == null)
            //    return null;

            var query = (from p in Models.ChatContext.Context.Posts
                         where p.Id_Author == userId
                         orderby p.CreateDT descending
                         select p);
            if (query.Count() > 0) return query.First();
            else return null;
        }

        /// <summary>
        /// It counts comments, that user has done.
        /// It creates a query to database to find out how many comments has done the user.
        /// </summary>
        /// <param name="userId">user id </param>
        /// <returns>ammount of comments</returns>
        public static int GetCommentsCnt(int userId)
        {
            var user = Models.ChatContext.Context.Users.Find(userId);
            if (user == null)
                return 0;
            return (from c in Models.ChatContext.Context.Posts
                    where c.Id_Author == userId && c.Id_Cite != 0
                    select c).Count();
        }

        /// <summary>
        /// It finds the user by userId and calls the boolean isBanned function.
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>boolean function IsBanned(user)</returns>
        static public bool IsBanned(int userId)
        {
            var user = Models.ChatContext.Context.Users.Find(userId);
            if (user == null)
                return true;
            return IsBanned(user);
        }

        /// <summary>
        /// It checkes if the user is banned
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>the result</returns>
        /// <example>
        /// <code>
        ///  return user.LastLogin.GetValueOrDefault().Equals
        ///     (DateTime.Parse(
        ///         System
        ///         .Configuration
        ///         .ConfigurationManager
        ///         .AppSettings["banDate"]));
        /// </code>
        /// </example>
        static public bool IsBanned(User user)
        {
            return user.LastLogin.GetValueOrDefault().Equals
                   (DateTime.Parse(
                       System
                       .Configuration
                       .ConfigurationManager
                       .AppSettings["banDate"]));
        }

        /// <summary>
        /// Function sends the message to Email via smtpClient.
        /// </summary>
        /// <param name="sendTo">email adress of the user, who to send </param>
        /// <param name="message">text of the message</param>
        public static void SendMail(string sendTo, string message)
        {
            try
            {
                MailAddress send_from = new MailAddress("testunproverialovich@gmail.com", "Форум");
                MailAddress send_to = new MailAddress(sendTo, "Пользователь форума");

                using (MailMessage mailMassage = new MailMessage(send_from, send_to))
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    mailMassage.Subject = "Оповещение с форума";
                    mailMassage.Body = message;
                    mailMassage.IsBodyHtml = true;

                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(send_from.Address, "Ntcnbhjdfybt");

                    smtpClient.Send(mailMassage);



                }


            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }



    }
}