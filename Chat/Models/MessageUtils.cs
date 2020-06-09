using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    /// <summary>
    /// The <c>MessageUtils</c> class
    /// </summary>
    public class MessageUtils
    {
        // :) -> 😇
        /// <summary>
        /// This method replaces txt to smiley image in the message
        /// </summary>
        /// <param name="msg">smiley text, for example ":)"</param>
        /// <returns>replace result</returns>
        public static String ReplaceSmiley( String msg )
        {
            String ret = msg;
            foreach (Smiley s in Smiley.GetList())
            {
                if (String.IsNullOrEmpty(s.ReplTxt))
                {
                    ret = ret.Replace(s.Combo, 
                        $"<img src='/images/{s.ReplImg}' style='width:30px'/>");
                }
                else
                {
                    ret = ret.Replace(s.Combo, s.ReplTxt);
                }
            }
            return ret;
        }
    }
}