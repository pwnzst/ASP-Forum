using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int Id_Author { get; set; }
        public int Id_Theme { get; set; }
        public int Id_Cite { get; set; }
        public String Message { get; set; }
        public DateTime CreateDT { get; set; }

        public String GetCiteMessage()
        {
            return ChatContext.Context.Posts.Find(Id_Cite)?.Message;
        }

        public String GetCiteAuthor()
        {
            Post citedPost = ChatContext.Context.Posts.Find(Id_Cite);
            if (citedPost == null)
                return "";

            return UserUtil.GetUserShown(citedPost.Id_Author);
        }

    }
}

// <span>@post.Message</span>
// 1. @Utils.ReplaceSmiley(post.Message)
// 2. @Models.MessageUtils.ReplaceSmiley(post.Message)
// 3. @post.ReplacedSmileyMessage
