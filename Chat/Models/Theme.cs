using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class Theme
    {
        public int Id { get; set; }
        public int Id_Author { get; set; }
        public int Id_Section { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public DateTime CreateDT { get; set; }
    }
}