using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Chat.Models
{
    /// <summary>
    /// The <c>Smiley</c> class
    /// </summary>
    /// It was created for smiley for the chat
    public class Smiley
    {
        /// <value>Gets the value of text combo.</value>
        public String Combo   { get; set; }
        /// <value>Gets the value of replace text.</value>
        public String ReplTxt { get; set; }
        /// <value>Gets the value of replace image.</value>
        public String ReplImg { get; set; }

        private static List<Smiley> list;

        /// <summary>
        /// This function gets List of smiley.
        /// It checkes if list is not null and adds to the list data (combo,ReplTxt,ReplImg).
        /// </summary>
        /// <returns>list</returns>
        /// <example>
        /// <code>
        /// string path = System.AppDomain.CurrentDomain.BaseDirectory;
        /// doc.Load(path + @"\App_Data\Smiley.xml");
        /// foreach(XmlNode smiley in doc.GetElementsByTagName("smiley"))
        /// {
        ///      list.Add(new Smiley()
        ///      {
        ///             Combo = smiley.Attributes["Combo"].Value,
        ///             ReplTxt = smiley.Attributes["ReplTxt"]?.Value,
        ///             ReplImg = smiley.Attributes["ReplImg"]?.Value
        ///      });
        ///  }
        /// </code>
        /// </example>
    public static List<Smiley> GetList()
        {
            if (list == null)
            {
                list = new List<Smiley>();
                //  Наполнить коллекцию
                var doc = new XmlDocument();
                try
                {
                    string path = System.AppDomain.CurrentDomain.BaseDirectory;
                    doc.Load(path + @"\App_Data\Smiley.xml");
                    // XmlNodeList smileys = doc.GetElementsByTagName("smiley");
                    foreach(XmlNode smiley in doc.GetElementsByTagName("smiley"))
                    {
                        list.Add(new Smiley() {
                            Combo   = smiley.Attributes["Combo"].Value,
                            ReplTxt = smiley.Attributes["ReplTxt"]?.Value,
                            ReplImg = smiley.Attributes["ReplImg"]?.Value

                        });
                    }
                    doc = null;
                }
                catch
                {
                    list.Add(new Smiley() { Combo = ":)", ReplTxt = "😇" });
                    list.Add(new Smiley() { Combo = "]:", ReplImg = "devil.gif" });
                }
                
            }
            return list;
        }
    }
}