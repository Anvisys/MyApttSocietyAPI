using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Web.Http.Description;
using MyApttSocietyAPI.Models;

using System.IO;
using System.Drawing;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Web.Http.Cors;

namespace MyApttSocietyAPI.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Notifications")]
    public class NotificationsController : ApiController
    {


        [Route("All")]
        [HttpGet]  // GET: api/Notifications
        public IQueryable<Notification> Get()
        {
            var context = new SocietyDBEntities();
            var notifications = (from notes in context.Notifications
                                 orderby notes.Date descending
                                 select notes).Take(10);
            if (notifications.Count() < 1)
            {
                Notification tempNotice = new Notification();
                tempNotice.ID = -99;
                tempNotice.Notification1 = "No Notification in Data";
                tempNotice.Date = DateTime.Now;
                tempNotice.AttachName = null;
                List<Notification> newList = new List<Notification> { tempNotice };
                
               return  newList.AsQueryable();
            }
            else {

                return notifications; }
            
        }

        [Route("{SocietyID}/{Status}/{PageNumber}/{count}")]
        [HttpGet]

        public IEnumerable<Notification> GetNotice(int SocietyID, String Status, int PageNumber ,int count)
        {
            //int count = 10;
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    if (Status == "Open")
                    {
                        var notification = (from notes in context.Notifications
                                            where notes.SocietyID == SocietyID && notes.EndDate > DateTime.UtcNow
                                            select notes).ToList();

                        return notification.Skip((PageNumber - 1) * count).Take(count);
                    }
                    else {
                        var notification = (from notes in context.Notifications
                                            where notes.SocietyID == SocietyID && notes.EndDate < DateTime.UtcNow
                                            select notes).ToList();

                        return notification.Skip((PageNumber - 1) * count).Take(count);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
          
               
        }

        [Route("old/{SocietyID}")]
        [HttpGet]

        public IEnumerable<Notification> GetOldNotice(int SocietyID)
        {
            using (var context = new SocietyDBEntities())
            {
                var notification = (from notes in context.Notifications
                                    where notes.SocietyID == SocietyID && notes.EndDate < DateTime.UtcNow
                                    select notes).Take(10);

                return notification.ToList();
            }

        }



        // POST: api/Notifications
        public async Task<Notice> Post([FromBody]Notice value)
        {
            Log.log("Value Passed is , " + value.Notice_ID + "  :  " + DateTime.Now.ToString());
            try
            {
               // string file = System.Web.HttpContext.Current.Server.MapPath(@"~..\MyAptt.com\Images\") + value.Notice_ID + "\\" + value.FileName;

               //string docPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\MyAptt.com\Images\" + value.Notice_ID + "\\" + value.FileName);
                string docPath = @"http://www.MyAptt.com/AppImage/Notice/" + value.Notice_ID + "/" + value.FileName;
                Log.log("Path is --- " + docPath);
              //  byte[] img = System.IO.File.ReadAllBytes(docPath);
             //   String imgString = Convert.ToBase64String(img);
               
                value.imageByte = await GetImageAsync(docPath, value);
                
                //HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri(docPath);
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jpg"));

                //if (File.Exists(docPath))
                //{
                //    Log.log("File Exist");
                //    System.Drawing.Image image = System.Drawing.Image.FromFile(docPath);
                //    value.imageByte = ImageToByte(image);
                //}


                //byte[] fileContents;

                //using (MemoryStream memoryStream = new MemoryStream())
                //{
                //   // using (Bitmap image = new Bitmap(WebRequest.Create(docPath).GetResponse().GetResponseStream()))
                //    using (Bitmap image = new Bitmap(docPath))
                //        image.Save(memoryStream, ImageFormat.Jpeg);
                //    value.imageByte = memoryStream.ToArray();
                //}

                return value;

            }
            catch (Exception ex)
            {
                Log.log("Exception At Get Image , " + ex.Message + "  :  " + DateTime.Now.ToString());
                return null;

            }
        }

        // PUT: api/Notifications/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Notifications/5
        public void Delete(int id)
        {
        }

        private byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private async Task<byte[]> GetImageAsync(string path, Notice notice)
        {
           
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(path);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
            HttpResponseMessage response = await client.GetAsync(path);
            Log.log(response.StatusCode.ToString());
            Log.log(response.Headers.ToString());
            
            if (response.IsSuccessStatusCode)
            {
                notice.imageByte = await response.Content.ReadAsByteArrayAsync();
                Log.log(notice.imageByte.ToString());
            }
            return notice.imageByte;
        }
    }


    
}
