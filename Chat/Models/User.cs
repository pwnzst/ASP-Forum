using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Models
{
    /// <summary>
    /// The <c>User</c> class
    /// </summary>
    /// <remarks>It contains all user data such as id, login,passhash, passsalt etc.</remarks>
    public class User
    {
        /// <value>Gets the value of User Id.</value>
        public int    Id { get; set; }
        /// <value>Gets the value of User Login.</value>
        public String Login { get; set; }
        /// <value>Gets the value of User PassHash.</value>
        public String PassHash { get; set; }
        /// <value>Gets the value of User PassSalt.</value>
        public String PassSalt { get; set; }
        /// <value>Gets the value of User ShownData.</value>
        public int?    ShownData { get; set; }  // Login / RealName / Email
        /// <value>Gets the value of User real name.</value>
        public String RealName { get; set; }
        /// <value>Gets the value of User Status.</value>
        public String Status { get; set; }
        /// <value>Gets the value of User Email.</value>
        public String Email { get; set; }
        /// <value>Gets the value of User AvatarFile path</value>
        public String AvatarFile { get; set; }
        /// <value>Gets the value of User Registered datetime.</value>
        public DateTime Registered { get; set; }
        /// <value>Gets the value of User LastLogin.</value>
        public DateTime? LastLogin { get; set; }  // Nullable<DateTime>
    }
}