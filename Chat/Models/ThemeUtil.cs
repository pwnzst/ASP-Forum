using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{

    /// <summary>
    /// The <c>ThemeUtil</c> class. 
    /// </summary>
    public class ThemeUtil
    {
        /// <value> The value of Models.ChatContext chat</value>
        static Models.ChatContext chat;

        /// <summary>
        /// It gets the name of theme by Id.
        /// It checks if chat is not null and finds needed theme by id.
        /// </summary>
        /// <param name="themeId">Id of the needed theme</param>
        /// <returns>Theme theme that we need</returns>
        /// <example>
        /// <code>
        /// if (chat == null) chat = new Models.ChatContext();
        /// return chat.Themes.Find(themeId);
        /// </code>
        /// </example>
        public static Theme GetTheme(int themeId)
        {
            if (chat == null) chat = new Models.ChatContext();
            return chat.Themes.Find(themeId);
        }
    }
}