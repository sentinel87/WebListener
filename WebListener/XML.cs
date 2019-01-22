using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WebListener
{
    public class XML
    {
        public string SaveXml(string url)
        {
            var RootPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var AppPath = Path.Combine(RootPath, "WebListenerStorage");
            var filePath = Path.Combine(AppPath, "Configuration.xml");
            string Result = "";
            try
            {
                XmlTextWriter Writer = new XmlTextWriter(filePath, System.Text.Encoding.UTF8);
                Writer.WriteStartDocument(true);
                Writer.Formatting = Formatting.Indented;
                Writer.Indentation = 2;
                Writer.WriteStartElement("Config");
                Writer.WriteStartElement("UrlString");
                Writer.WriteString(url);
                Writer.WriteEndElement();
                Writer.WriteEndElement();
                Writer.WriteEndDocument();
                Writer.Close();
            }
            catch (Exception e)
            {
                Result = "Config save error! Reason: " + e.Message;
            }
            return Result;
        }

        public string restoreDefaults()
        {
            string Result = SaveXml("http://hds/sdf/index.php");
            return Result;
        }

        public string LoadXml(ref string url)
        {
            XmlDocument xml = new XmlDocument();
            var RootPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var AppPath = Path.Combine(RootPath, "WebListenerStorage");
            var filePath = Path.Combine(AppPath, "Configuration.xml");
            string Result = "";
            try
            {
                xml.Load(filePath);
                XmlNodeList xnList = xml.SelectNodes("/Config");
                foreach (XmlNode xn in xnList)
                {
                    url = xn["UrlString"].InnerText;
                }
            }
            catch (Exception e)
            {
                Result = "Config save error! Reason: " + e.Message;
            }
            return Result;
        }
    }
}