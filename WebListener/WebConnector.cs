using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;

namespace WebListener
{
    public class WebConnector
    {
        Logger logger = new Logger();
        //string json = @"[{""phone"":""3123123"",""body"":""error""},{""phone"":""668960359"",""body"":""Wysłano pierwszego smsa!""},{""phone"":""asdcg67"",""body"":""error""},{""phone"":""668960359"",""body"":""Wysłano drugiego smsa!""}]";
        //string Url = "http://hds/sdf/index.php";

        public List<SMSObject> parseJSON(string url)
        {
            List<SMSObject> Objects = new List<SMSObject>();
            try
            {
                WebClient Clt = new WebClient();
                string msg = Clt.DownloadString(url);
                JArray Arr = JArray.Parse(msg);
                Objects = Arr.ToObject<List<SMSObject>>();
            }
            catch(Exception ex)
            {
                logger.createLog("Server connection Error: " + ex.Message, "ErrorLogs.txt");
            }
            return Objects;
        }
    }
}