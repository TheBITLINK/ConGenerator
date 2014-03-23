using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CGApi
{
    public static class Localization
    {
        public static string LangFile { get; set; }
        public static string getlocalizedstring(string xpath)
        {
            XmlDocument lang = new XmlDocument();
            lang.Load(LangFile);
            XmlNode str = lang.SelectSingleNode(xpath);
            return str.InnerText;
        }

        public static string getlocalizedstringfromlangfile(string LngFile, string xpath)
        {
            XmlDocument lang = new XmlDocument();
            lang.Load(LngFile);
            XmlNode str = lang.SelectSingleNode(xpath);
            return str.InnerText;
        }
    }
}
