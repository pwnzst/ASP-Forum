using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    /// <summary>
    /// The <c>Theme</c> class. 
    /// </summary>
    /// <remarks>It contains fields same as in database</remarks>
    public class Theme
    {
        /// <value>Gets the value of Theme Id.</value>
        public int Id { get; set; }

        /// <value>Gets the value of Theme author Id.</value>
        public int Id_Author { get; set; }

        /// <value>Gets the value of Theme section Id.</value>
        public int Id_Section { get; set; }

        /// <value>Gets the value of Theme Title.</value>
        public String Title { get; set; }

        /// <value>Gets the value of Theme Description.</value>
        public String Description { get; set; }

        /// <value>Gets the value of Theme creationTime.</value>
        public DateTime CreateDT { get; set; }
    }
}