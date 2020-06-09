using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    /// <summary>
    /// The <c>Post</c> class
    /// </summary>
    /// <remarks>It contains all needed Post data such as Id, authorId, themeId etc. </remarks>
    public class Post
    {
        /// <value>Gets the value of Post Id.</value>
        public int Id { get; set; }
        /// <value>Gets the value of Post Autor Id.</value>
        public int Id_Author { get; set; }
        /// <value>Gets the value of Post Theme Id.</value>
        public int Id_Theme { get; set; }
        /// <value>Gets the value of Post Cite Id.</value>
        public int Id_Cite { get; set; }
        /// <value>Gets the value of Post Message.</value>
        public String Message { get; set; }
        /// <value>Gets the value of Post Creation datetime.</value>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// This method gets the cite message
        /// </summary>
        /// <returns>Cite message</returns>
        /// <example>
        /// <code>
        /// return ChatContext.Context.Posts.Find(Id_Cite)?.Message;
        /// </code>
        /// </example>
        public String GetCiteMessage()
        {
            return ChatContext.Context.Posts.Find(Id_Cite)?.Message;
        }

        /// <summary>
        /// This method gets the cite author
        /// </summary>
        /// <returns>Cite author</returns>
        /// <example>
        /// <code>
        /// Post citedPost = ChatContext.Context.Posts.Find(Id_Cite);
        /// if (citedPost == null)
        ///      return "";
        /// return UserUtil.GetUserShown(citedPost.Id_Author);
        /// </code>
        /// </example>
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
