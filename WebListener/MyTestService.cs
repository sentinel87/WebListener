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

namespace WebListener
{
    [Service]
    public class MyTestService : IntentService
    {
        Logger logger = new Logger();
        SmsModule Sms = new SmsModule();
        WebConnector Connector = new WebConnector();
        public MyTestService(): base("MyTestService")
        {

        }
        protected override void OnHandleIntent(Intent intent)
        {
            string URL = "";
            URL = intent.GetStringExtra("url");
            logger.createLog("Service started...", "ServiceLogs.txt");
            logger.createLog("Url passed: "+URL, "ServiceLogs.txt");
            List<SMSObject> ToSend = Connector.parseJSON(URL);
            foreach(SMSObject obj in ToSend)
            {
                Sms.sendSMS(obj);
            }
        }


    }
}