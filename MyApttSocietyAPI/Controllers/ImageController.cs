using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MyApttSocietyAPI.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Drawing.Imaging;

namespace MyApttSocietyAPI.Controllers
{
      [EnableCors(origins: "*", headers: "*", methods: "*")]
      [RoutePrefix("api/Image")]
    public class ImageController : ApiController
    {
        // GET: api/Image
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Image/5
        [Route("Res/{ResId}")]
        [HttpGet]
        public ShopImage GetByResID(int ResId)
        {
            var context = new SocietyDBEntities();
            var Image = (from res in context.ViewUserImages
                         where (res.ResID == ResId)
                             select new ShopImage() { ID = res.ResID, ImageString = res.Profile_image }).First();
            return Image;
        }


        // GET: api/Image/5
        [Route("User/{UserId}")]
        [HttpGet]
        public ShopImage GetByUserID(int UserId)
        {
            try
            {
                var context = new SocietyDBEntities();
                var Image = (from res in context.ViewUserImages
                             where (res.UserID == UserId)
                             select new ShopImage() { ID = res.ResID, ImageString = res.Profile_image }).First();
                return Image;
            }
            catch (Exception ex)
            {

                return new ShopImage { ID = -99, ImageString = { } };
            }
        }

        [Route("Mob/{Mobile}")]
        [HttpGet]
        public ShopImage GetByMobile(String Mobile)
        {
            var context = new SocietyDBEntities();

            var userID = (from res in context.ViewSocietyUsers
                         where (res.MobileNo == Mobile)
                         select res.UserID).First();

            var Image = (from res in context.UserImages
                         where (res.UserID == userID)
                             select new ShopImage() { ID = res.UserID, ImageString = res.Profile_image }).First();
            return Image;
        }
        // POST: api/Image
        public HttpResponseMessage Post([FromBody]Profile value)
        {
            String resp;
            string imagestring = value.ImageString;
            if(imagestring.Length%4 != 0)
            {
                while(imagestring.Length%4>0)
                imagestring = imagestring + "=";
            }
            try
            {
                        if (value.UserID == 0 || value.ImageString == null)
                        {
                            resp = "{\"Response\":\"Fail\",\"Message\":\"UserID or Image String is null\" }";
                            var response = Request.CreateResponse(HttpStatusCode.ExpectationFailed);
                            response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                            return response;
                        }
                        else
                        {
                                    using (var context = new SocietyDBEntities())
                                    {
                                           List<UserImage> users = (from u in context.UserImages
                                                                         where u.UserID == value.UserID
                                                                         select u).ToList();
                                            if (users.Count == 0)
                                            {

                                                Log.log("Saving Image for new user : " + value.ResID + " " + value.UserID);
                                                context.UserImages.Add(new UserImage
                                                {
                                                  
                                                    UserID = value.UserID,
                                                    Profile_image = Convert.FromBase64String(imagestring),

                                                });

                                            }
                                            else
                                            {
                                                Log.log("Image updated for user : " + value.ResID + " " + value.UserID);
                                                foreach (UserImage user in users)
                                                {
                                                    user.Profile_image = Convert.FromBase64String(imagestring);
                                                 SavetoFileServer( value,  user);

                                                }
                                            }

                                            context.SaveChanges();
                                            resp = "{\"Response\":\"OK\"}";
                                            var response = Request.CreateResponse(HttpStatusCode.OK);
                                            response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                                            return response;
                                        }
                    
                        }

              
            }

            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {

                        Log.log("api/Image Failed to Update : Property-" + validationError.PropertyName + "  Error- " + validationError.ErrorMessage + "  At " + DateTime.Now.ToString());


                    }
                }
                resp = "{\"Response\":\"Fail\",\"Message\":\"" + dbEx.Message + "\" }";
                var response = Request.CreateResponse(HttpStatusCode.ExpectationFailed);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
        }

        protected bool SavetoFileServer(Profile value , UserImage user)
        {
            bool success = false;
            try
            {
                ImageFormat format = ImageFormat.Png;
                String imagename = value.UserID.ToString();
                string imagepath = @"http://www.nestin.online/ImageServer/User/" + imagename+"."+format;
                using (FileStream stream =new FileStream(imagepath, FileMode.Create))
                {
                    stream.Write(user.Profile_image, 0, user.Profile_image.Length);
                    stream.Flush();
                }
                success = true;
            }

            catch ( Exception ex)
            {

            }
            return success;
        }
        
        // PUT: api/Image/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Image/5
        public void Delete(int id)
        {
        }
    }
}
