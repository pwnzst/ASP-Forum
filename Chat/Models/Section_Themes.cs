using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    /// <summary>
    /// The <c>Section_Themes</c> class
    /// </summary>
    public class Section_Themes
    {
        /// <value>the section object</value>
        public Section section;

        /// <value>List of themes</value>
        public List<Theme> themes;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <example>
        /// <code>
        /// public Section_Themes()
        /// {
        ///    themes = new List<Theme>();
        /// }
        /// </code>
        /// </example>
        public Section_Themes()
        {
            themes = new List<Theme>();
        }
    }
}