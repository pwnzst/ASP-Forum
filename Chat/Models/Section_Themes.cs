using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class Section_Themes
    {
        public Section section;
        public List<Theme> themes;
        public Section_Themes()
        {
            themes = new List<Theme>();
        }
    }
}