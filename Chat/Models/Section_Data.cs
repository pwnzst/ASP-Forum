using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class Section_Data
    {
        public Section section;
        public List<IndexData> data;
        public Section_Data()
        {
            data = new List<IndexData>();
        }
    }
}