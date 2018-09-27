using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;

namespace MyApttSocietyAPI
{
    public class Log
    {
        private static string FileName = System.Web.HttpContext.Current.Server.MapPath(@"~\Content\LogFile.txt");
        public static void log(String Message)
        {
            StreamWriter errWriter = new StreamWriter(FileName, true);
            errWriter.WriteLine(Message);

            errWriter.Close();
        }
    }
}