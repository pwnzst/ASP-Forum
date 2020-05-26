using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Chat.Models
{
    public class Smiley
    {
        public String Combo   { get; set; }
        public String ReplTxt { get; set; }
        public String ReplImg { get; set; }

        private static List<Smiley> list;

        public static List<Smiley> GetList()
        {
            if (list == null)
            {
                list = new List<Smiley>();
                // Д.З. Наполнить коллекцию
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