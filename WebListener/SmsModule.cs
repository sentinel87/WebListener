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
using Android.Telephony;

namespace WebListener
{
    public class SmsModule
    {
        Logger logger = new Logger();
        public void sendSMS(SMSObject Obj)
        {
            string number = Obj.phone;
            if (number.Length != 9) { logger.createLog("Sending sms error: number "+Obj.phone+ " does not have 9 digits!", "ErrorLogs.txt"); return; }
            try
            {
                int check = Int32.Parse(number);
                SmsManager sms = SmsManager.Default;
                IList<string> parts = sms.DivideMessage(Obj.body);
                sms.SendMultipartTextMessage(Obj.phone, null, parts, null, null);
            }
            catch (Exception ex)
            {
                logger.createLog("Sending sms error to " + Obj.phone+": "+ex.Message, "ErrorLogs.txt");
                return;
            }
            logger.createLog("Sms was send to: "+Obj.phone, "ServiceLogs.txt");
        }
    }
}