using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace Chat.Models
{
    public class UserUtil
    {
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

        public static User GetUser(int userId)
        {
            return Models.ChatContext.Context.Users.Find(userId);
        }

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


        public static int GetPostCnt(int userId)
        {
            var user = Models.ChatContext.Context.Users.Find(userId);
            if (user == null)
                return 0;
            return (from p in Models.ChatContext.Context.Posts
                    where p.Id_Author == userId
                    select p).Count();
        }

        public static int GetThemeCnt(int userId)
        {
            var user = Models.ChatContext.Context.Users.Find(userId);
            if (user == null)
                return 0;
            return (from t in Models.ChatContext.Context.Themes
                    where t.Id_Author == userId
                    select t).Count();
        }

        public static int GetSectionsCnt(int userId)
        {
            var user = Models.ChatContext.Context.Users.Find(userId);
            if (user == null)
                return 0;
            return (from s in Models.ChatContext.Context.Sections
                    where s.Id_Author == userId
                    select s).Count();

        }

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

        public static int GetCommentsCnt(int userId)
        {
            var user = Models.ChatContext.Context.Users.Find(userId);
            if (user == null)
                return 0;
            return (from c in Models.ChatContext.Context.Posts
                    where c.Id_Author == userId && c.Id_Cite != 0
                    select c).Count();
        }

        static public bool IsBanned(int userId)
        {
            var user = Models.ChatContext.Context.Users.Find(userId);
            if (user == null)
                return true;
            return IsBanned(user);
        }

        static public bool IsBanned(User user)
        {
            return user.LastLogin.GetValueOrDefault().Equals
                   (DateTime.Parse(
                       System
                       .Configuration
                       .ConfigurationManager
                       .AppSettings["banDate"]));
        }

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