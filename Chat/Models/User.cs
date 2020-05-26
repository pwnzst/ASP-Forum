using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Models
{
    public class User
    {
        public int    Id { get; set; }
        public String Login { get; set; }
        public String PassHash { get; set; }
        public String PassSalt { get; set; }
        public int?    ShownData { get; set; }  // Login / RealName / Email
        public String RealName { get; set; }
        public String Status { get; set; }
        public String Email { get; set; }
        public String AvatarFile { get; set; }
        //[Column("AvatarFile", TypeName = "Varchar(64)")]
        //public HttpPostedFile AvatarFile { get; set; }
        public DateTime Registered { get; set; }
        public DateTime? LastLogin { get; set; }  // Nullable<DateTime>
    }
}