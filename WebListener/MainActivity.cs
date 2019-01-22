using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Android.Views;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebListener
{
    [Activity(Label = "WebListener", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        bool ServiceIsActive = false;
        string URL = "";
        Logger logger = new Logger();
        XML Xml = new XML();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            OnLoad();
            Button btnStart = FindViewById<Button>(Resource.Id.btnStart);
            Button btnStop = FindViewById<Button>(Resource.Id.btnStop);
            Button btnSave = FindViewById<Button>(Resource.Id.btnSave);
            Button btnDefault = FindViewById<Button>(Resource.Id.btnDefault);
            AutoCompleteTextView UrlText = FindViewById<AutoCompleteTextView>(Resource.Id.atvUrl);
            UrlText.Text = URL;
            if(ServiceIsActive)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            else
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
            btnStart.Click += delegate
             {
                 scheduleAlarm();
                 Toast.MakeText(this, "Rozpoczęto nasłuch...", ToastLength.Short).Show();
                 logger.createLog("Application is listening...", "ApplicationLogs.txt");
                 btnStart.Enabled = false;
                 btnStop.Enabled = true;
             };
            btnStop.Click += delegate
             {
                 cancelAlarm();
                 Toast.MakeText(this, "Przerwano nasłuch.", ToastLength.Short).Show();
                 logger.createLog("Listening aborted.", "ApplicationLogs.txt");
                 btnStart.Enabled = true;
                 btnStop.Enabled = false;
             };
            btnSave.Click += delegate
             {
                 string Result = Xml.SaveXml(UrlText.Text);
                 if (Result != "")
                 {
                     logger.createLog(Result, "ErrorLogs.txt");
                 }
                 else
                 {
                     URL = UrlText.Text;
                     Toast.MakeText(this, "Zapisano dane.", ToastLength.Short).Show();
                     logger.createLog("Url saved: " + URL, "ApplicationLogs.txt");
                 }
             };
            btnDefault.Click += delegate
            {
                string Result = Xml.restoreDefaults();
                if (Result != "")
                {
                    logger.createLog(Result, "ErrorLogs.txt");
                }
                string Result2 = Xml.LoadXml(ref URL);
                if (Result2 != "")
                {
                    logger.createLog(Result2, "ErrorLogs.txt");
                }
                UrlText.Text = URL;
            };
        }

        public void scheduleAlarm() //start usługi
        {
            try
            {
                Intent intent = new Intent(Application.Context, typeof(MyAlarmReceiver));
                intent.PutExtra("url", URL);
                PendingIntent Intent = PendingIntent.GetBroadcast(this, MyAlarmReceiver.REQUEST_CODE, intent, PendingIntentFlags.UpdateCurrent);
                long firstMillis = SystemClock.CurrentThreadTimeMillis();
                AlarmManager Alarm = (AlarmManager)this.GetSystemService(Context.AlarmService);
                Alarm.SetRepeating(AlarmType.RtcWakeup, firstMillis, 500, Intent);
            }
            catch(Exception ex)
            {
                logger.createLog("Application error: "+ex.Message, "ErrorLogs.txt");
            }
        }
            
        public void cancelAlarm()   //zatrzymanie usługi
        {
            try
            {
                Intent intent = new Intent(Application.Context, typeof(MyAlarmReceiver));
                PendingIntent pIntent = PendingIntent.GetBroadcast(this, MyAlarmReceiver.REQUEST_CODE, intent, PendingIntentFlags.UpdateCurrent);
                AlarmManager alarm = (AlarmManager)this.GetSystemService(Context.AlarmService);
                alarm.Cancel(pIntent);
            }
            catch (Exception ex)
            {
                logger.createLog("Application error: " + ex.Message, "ErrorLogs.txt");
            }
        }

        public void OnLoad()    //czynności przy starcie aplikacji
        {
            var RootPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;    //utworzenie katalogu do logów
            var filePath = Path.Combine(RootPath, "WebListenerStorage");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                string Result=Xml.restoreDefaults();
                if(Result!="")
                {
                    logger.createLog(Result, "ErrorLogs.txt");
                }
            }
            string Result2 = Xml.LoadXml(ref URL);
            if (Result2 != "")
            {
                logger.createLog(Result2, "ErrorLogs.txt");
            }
            Intent intent = new Intent(Application.Context, typeof(MyAlarmReceiver));       //sprawdzamy, czy usługa już działa w tle (zapobiegnięcie duplikacji usług)
            ServiceIsActive = PendingIntent.GetBroadcast(this, MyAlarmReceiver.REQUEST_CODE, intent, PendingIntentFlags.NoCreate) != null;  
            if(ServiceIsActive)
            {
                logger.createLog("Service is already running!", "ApplicationLogs.txt");
            }
        }
    }
}

