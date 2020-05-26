using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class IndexData
    {
        public Theme theme { get; set; }
        public Post lastPost { get; set; }
        public String lastPostAuthorShown { get; set; }
        public String authorShown { get; set; }
        public int postsCnt { get; set; }
    }
}