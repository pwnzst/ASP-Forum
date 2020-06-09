using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    /// <summary>
    /// The <c>AuthData</c> class
    /// </summary>
    /// <remarks>It contains authdata (login, password)</remarks>
    public class AuthData
    {
        /// <value>Gets the value of User login.</value>
        public String UserLogin { get; set; }
        /// <value>Gets the value of User password.</value>
        public String UserPassword { get; set; }
    }
}