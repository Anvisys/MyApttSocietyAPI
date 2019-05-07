using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace MyApttSocietyAPI.Models
{
    public class Utility
    {
        public static string sendSMS(String textMessage)

        {
            String message = HttpUtility.UrlEncode(textMessage);
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "msfOwE8inQk-tKtISb5ZeGF09HkktGreaH64DJlkg3"},
                {"numbers" , "+918602026330,8851644379 ,+919591033223"},
                {"message" , message},
                {"sender" , "TXTLCL"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                return result;
            }
        }


       

        public static bool NotifyEmployee(String textMessage, String MobileNumber)
        {
            sendSMS(textMessage, MobileNumber);
            return true;
        }

        public static string sendSMS(String textMessage, String MobileNumber)
        {
            String message = HttpUtility.UrlEncode(textMessage);
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "msfOwE8inQk-tKtISb5ZeGF09HkktGreaH64DJlkg3"},
                {"numbers" , MobileNumber},
                {"message" , message},
                {"sender" , "TXTLCL"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                return result;

            }
        }


        public static void SendGCMNotification(string receiverID, string NotificationText)
        {

            string url = "https://android.googleapis.com/gcm/send";
            String RegID = receiverID;
            String API_KEY = "AIzaSyBBny4kCJmN1Ep3NlUxImIF7iHz6OC6YWQ";
            String SENDER_ID = "288809313667";
            WebRequest tRequest;

            tRequest = WebRequest.Create(url);

            tRequest.Method = "post";

            // tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";


            tRequest.ContentType = "application/json";

            tRequest.Headers.Add(string.Format("Authorization: key={0}", API_KEY));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            string postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":108,\"delay_while_idle\":true,\"data\": { \"msg\" : " + "\"" + NotificationText + "\",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\"},\"registration_ids\":[\"" + receiverID + "\"]}";

            Byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);

            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);

            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            String sResponseFromServer = tReader.ReadToEnd();   //Get response from GCM server.

            //lblnotifResp.Text = sResponseFromServer;      //Assigning GCM response to Label text 

            tReader.Close();
            dataStream.Close();
            tResponse.Close();
        }

        public static void SendMail(string EmailID, string EmailSubject, string EmailBody)
        {
            try
            {

                string URI = "http://www.nestin.online/php/MyApttMail.php";
                WebRequest request = WebRequest.Create(URI);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postData = "ToMail=" + EmailID + "&Subject=" + EmailSubject + "&html_Message=" + EmailBody;
                Stream dataStream = request.GetRequestStream();
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream());
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception ex)
            { }

        }

        public static string sendSMS2Resident(String textMessage, String MobileNumber)
        {
            String message = HttpUtility.UrlEncode(textMessage);
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "msfOwE8inQk-tKtISb5ZeGF09HkktGreaH64DJlkg3"},
                {"numbers" , MobileNumber},
                {"message" , message},
                {"sender" , "TXTLCL"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                return result;
            }
        }

    }
}