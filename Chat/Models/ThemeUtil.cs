using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class ThemeUtil
    {
        static Models.ChatContext chat;

        public static Theme GetTheme(int themeId)
        {
            if (chat == null) chat = new Models.ChatContext();
            return chat.Themes.Find(themeId);
        }
    }
}