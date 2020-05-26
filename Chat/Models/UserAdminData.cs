using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class UserAdminData
    {
        public User     user          { get; set; }
        public bool     isBan         { get; set; }
        public String   shown         { get; set; }
        public int      postCnt       { get; set; }
        public int      themeCnt      { get; set; }
        public int      secCnt        { get; set; }
        public Post     lastPost      { get; set; }
        public int      postsLastDay  { get; set; }
        public int      cites         { get; set; }
  
    }
}