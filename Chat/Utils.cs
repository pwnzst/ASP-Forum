using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Chat
{
    public static class Utils
    {
        public static String DateRu(DateTime date)
        {
            String mnth="";
            switch (date.Month)
            {
                case 1:  mnth = "Янв"; break;
                case 2:  mnth = "Фев"; break;
                case 3:  mnth = "Мар"; break;
                case 4:  mnth = "Апр"; break;
                case 5:  mnth = "Май"; break;
                case 6:  mnth = "Июн"; break;
                case 7:  mnth = "Июл"; break;
                case 8:  mnth = "Авг"; break;
                case 9:  mnth = "Сен"; break;
                case 10: mnth = "Окт"; break;
                case 11: mnth = "Ноя"; break;
                case 12: mnth = "Дек"; break;
            }
            string dayText = "";
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:    dayText = "ВС"; break;
                case DayOfWeek.Monday:    dayText = "ПН"; break;
                case DayOfWeek.Tuesday:   dayText = "ВТ"; break;
                case DayOfWeek.Wednesday: dayText = "СР"; break;
                case DayOfWeek.Thursday:  dayText = "ЧТ"; break;
                case DayOfWeek.Friday:    dayText = "ПТ"; break;
                case DayOfWeek.Saturday:  dayText = "СБ"; break;
            }
            return String.Format("{5}, {0:00} {1} {2}, {3:00}:{4:00}", date.Day, mnth, date.Year, date.Hour, date.Minute, dayText);
            
        }

        public static string GetMD5HexString(string str)
        {
            using (var hasher = System.Security.Cryptography.MD5.Create())
            {
                byte[] strBytes = System.Text.Encoding.ASCII.GetBytes(str);
                byte[] hashBytes = hasher.ComputeHash(strBytes);
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string GetSHA256HexString(string str)
        {
            using (var hasher = System.Security.Cryptography.SHA256.Create())
            {
                byte[] strBytes = System.Text.Encoding.ASCII.GetBytes(str);
                byte[] hashBytes = hasher.ComputeHash(strBytes);
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static String GetPassSalt()
        {
            var sb = new System.Text.StringBuilder();
            var rnd = new Random();
            for (int i = 0; i < 32; i++)
            {
                sb.Append(((byte)rnd.Next()).ToString("X2"));
            }
            return sb.ToString();
        }

        public static String GetHashByPassSalt(String RawPass, String Salt)
        {
            return GetSHA256HexString(RawPass + Salt);
        }

        public static bool MoveUploadFile(HttpPostedFileWrapper file, String path)
        {

            // string path = Directory.GetCurrentDirectory();   // "C:\\Program Files (x86)\\IIS Express"
            // string path = System.AppDomain.CurrentDomain.BaseDirectory;   // ...ASP_CHAT
            // string path = System.Environment.CurrentDirectory;   // ...IIS Express
            // string path = Server.MapPath("/");   // ...ASP_CHAT

            
            string fname =
                path
                + @"\"                
                + file.FileName;
            try
            {
                using (var sw = new FileStream(fname, FileMode.Create))
                {
                    file.InputStream.CopyTo(sw);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool MoveUploadAvatar(HttpPostedFileWrapper file, String path)
        {
            string AvatarStorage =
                System
                .Configuration
                .ConfigurationManager
                .AppSettings["AvatarStorage"];

            return MoveUploadFile(file, path + @"\" + AvatarStorage);                
        }

        public static bool ValidatePassword(String RawPass)
        {
            if (string.IsNullOrWhiteSpace(RawPass))
            {
                return false;
            }
            // hasNumber
            if (!new Regex(@"[0-9]+").IsMatch(RawPass)) return false;

            // hasLowerChar
            if (!new Regex(@"[a-z]+").IsMatch(RawPass)) return false;

            // hasUpperChar
            if (!new Regex(@"[A-Z]+").IsMatch(RawPass)) return false;

            // hasMiniMaxChars
            if (!new Regex(@".{4,15}").IsMatch(RawPass)) return false;

            // hasSymbols
            if (!new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]").IsMatch(RawPass)) return false;

            return true;
        }
    }
}