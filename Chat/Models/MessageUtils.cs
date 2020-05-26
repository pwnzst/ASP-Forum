using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class MessageUtils
    {
        // :) -> 😇
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