using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class Statistics
    {
        static public int GetThemePostCnt(int themeId)
        {
            return 
                (from p in ChatContext.Context.Posts
                 where p.Id_Theme == themeId
                 select p).Count();
        }

        static public Models.Post GetLastPostInTheme(int themeId)
        {
            var query = from p in ChatContext.Context.Posts
                        where p.Id_Theme == themeId
                        orderby p.CreateDT descending
                        select p;

            if (query.Count() > 0)
                return query.First();
            else
                return null;
        }
    }
}