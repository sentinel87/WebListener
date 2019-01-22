using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WebListener
{
    public class Logger
    {
        public void createLog(string msg,string filename)
        {
            var RootPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = Path.Combine(RootPath, "WebListenerStorage");
            string Date = DateTime.Now.ToLongTimeString(); 
            string Combine = Date + " " + msg;
            using (StreamWriter Writer = new StreamWriter(filePath+"/"+filename, true))
            {
                Writer.WriteLine(Combine);
                Writer.Close();
            }
        }
    }
}