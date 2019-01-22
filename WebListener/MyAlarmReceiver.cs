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
    [BroadcastReceiver]
    public class MyAlarmReceiver : BroadcastReceiver
    {
        public static int REQUEST_CODE = 12345;
        public static String ACTION = "com.codepath.example.servicesdemo.alarm";
        public override void OnReceive(Context context, Intent intent)
        {
            string URL = "";
            URL= intent.GetStringExtra("url");
            Intent i = new Intent(context, typeof(MyTestService));
            i.PutExtra("url", URL);
            context.StartService(i);
        }
    }
}