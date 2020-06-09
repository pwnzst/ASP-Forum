using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    /// <summary>
    /// The <c>Section</c> class
    /// </summary>
    /// <remarks>It contains all section data</remarks>
    public class Section
    {
        /// <value>Gets the value of Section Id.</value>
        public int Id { get; set; }

        /// <value>Gets the value of Section Author Id.</value>
        public int Id_Author { get; set; }

        /// <value>Gets the value of Section Title.</value>
        public String Title { get; set; }

        /// <value>Gets the value of Section Description.</value>
        public String Description { get; set; }

        /// <value>Gets the value of Section creation time.</value>
        public DateTime CreateDT { get; set; }
    }
}