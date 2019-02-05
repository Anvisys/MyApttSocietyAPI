using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyApttSocietyAPI.Models;
using System.Web.Http.Cors;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace MyApttSocietyAPI.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]

    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        public string Get(int id)
        {
            return "value";
        }

        [Route("Validate")]
        [HttpPost]
        public ValidUser Post([FromBody]ValidateUser ValUser)
        {
            var ValidUser = new ValidUser();
            try
            {

                using (var context = new SocietyDBEntities())
                {

                    if (ValUser.Email == null && ValUser.Mobile == null)
                    {
                       
                        ValidUser.result = "Fail";
                        ValidUser.message = "Email and Maobile are null";
                        return ValidUser;

                    }

                    else if (ValUser.Email == null || ValUser.Email == "")
                    {
                       
                        var users = (from USER in context.ViewUsers
                                     where USER.MobileNo == ValUser.Mobile
                                     select USER);
                        if (users.Count() >0)
                        {
                            ValUser.Email = users.First().EmailId;
                        }
                        else {
                            ValidUser.result = "Fail";
                            ValidUser.message = "Mobile Number is incorrect";
                            return ValidUser;
                        }

                       
                    }
                    String encPwd = ValidateUser.EncryptPassword(ValUser.Email, ValUser.Password);

                    Log.log("Encrypted Password is :" + encPwd + " At " + DateTime.Now.ToString());

                    var L2EQuery = context.ViewUsers.Where(u => (u.UserLogin == ValUser.Email || u.MobileNo == ValUser.Mobile) && u.Password == encPwd);
                    var user = L2EQuery.FirstOrDefault();

                    
                    if (user != null)
                    {
                        if (ValUser.RegistrationID != null || ValUser.RegistrationID != "")
                        {
                            var GCM = context.GCMLists;
                            var reg = GCM.Where(g => g.MobileNo == ValUser.Mobile);
                            if (reg.Count() == 0)
                            {

                                GCM.Add(new GCMList
                                {
                                    MobileNo = ValUser.Mobile,
                                    RegID = ValUser.RegistrationID,
                                    Topic = "",
                                });
                            }
                            else
                            {
                                reg.First().RegID = ValUser.RegistrationID;
                            }
                            context.SaveChanges();
                        }
                        ValidUser.result = "Ok";
                        ValidUser.UserData = user;

                        ValidUser.SocietyUser = (from res in context.ViewSocietyUsers
                                                 where ((res.DeActiveDate == null) || (DbFunctions.TruncateTime(res.DeActiveDate) > DbFunctions.TruncateTime(DateTime.UtcNow))) && res.UserID == user.UserID
                                                 select res).ToList();

                    }
                    else
                    {
                        ValidUser.result = "Fail";
                        ValidUser.message = "No Valid User";
                        ValidUser.UserData.FirstName = "";
                        ValidUser.UserData.LastName = "";
                    }
                   
                }
            }
            catch (Exception ex)
            {
               ValidUser.UserData.FirstName = "";
               ValidUser.UserData.LastName = "";
            }
            return ValidUser;
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
