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

        [Route("Society/{SocName}")]
        [HttpGet]
        public IEnumerable<Society> GetSocieties(String SocName)
        {
            var context = new SocietyDBEntities();
             var soc = (from s in context.Societies
                     where s.SocietyName.Contains(SocName)
                     select s).ToList();

            return soc;
        }

        [Route("Flat/{SocID}/{FlatNumber}")]
        [HttpGet]
        public IEnumerable<ViewFlat> GetFlats(int SocID, String FlatNumber)
        {
            var context = new SocietyDBEntities();
            var soc = (from s in context.ViewFlats
                       where s.FlatNumber == FlatNumber && s.SocietyId == SocID
                       select s).ToList();

            return soc;
        }

        [Route("Setting/{UserId}")]
        [HttpGet]
        public IHttpActionResult GetSetting(int UserId)
        {
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var L2EQuery = context.ViewUserSettings.Where(f => f.UserID == UserId);
                    if (L2EQuery.Count() > 0)
                    {
                        var userSetting = L2EQuery.FirstOrDefault();
                        return Ok(userSetting);
                    }
                    else {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.log("Error in Get User Setting at: " + DateTime.Now.ToString());
                return NotFound();
            }
        }

        [Route("IsValid")]
        [HttpPost]
        public ValidUser IsValid([FromBody]ValidateUser ValUser)
        {
            Log.log("Reached Validate At " + DateTime.Now.ToString());
            var ValidUser = new ValidUser();
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    if (ValUser.Email == null && ValUser.Mobile == null)
                    {
                        Log.log("Both are null " + DateTime.Now.ToString());
                        ValidUser.result = "Fail";
                        ValidUser.message = "Email and Maobile are null";
                        return ValidUser;

                    }

                    else if (ValUser.Email == null || ValUser.Email == "")
                    {
                        Log.log("one is valid " + DateTime.Now.ToString());
                        var users = (from USER in context.ViewSocietyUsers
                                     where USER.MobileNo == ValUser.Mobile
                                     select USER);
                        if (users.Count() > 0)
                        {
                            ValUser.Email = users.First().EmailId;
                        }
                        else
                        {
                            ValidUser.result = "Fail";
                            ValidUser.message = "Mobile Number is incorrect";
                            return ValidUser;
                        }


                    }
                    String encPwd = ValidateUser.EncryptPassword(ValUser.Email.ToLower(), ValUser.Password);

                    Log.log("Encrypted Password is :" + encPwd + " At " + DateTime.Now.ToString());

                    var L2EQuery = context.TotalUsers.Where(u => (u.UserLogin == ValUser.Email || u.MobileNo == ValUser.Mobile) && u.Password == encPwd);
                    var user = L2EQuery.FirstOrDefault();


                    if (user != null)
                    {
                        Log.log(user.FirstName);
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
                Log.log(ex.Message);
                ValidUser.UserData.FirstName = "";
                ValidUser.UserData.LastName = "";
            }
            return ValidUser;
        }



        [Route("Setting")]
        [HttpPost]
        public HttpResponseMessage UpdateSetting([FromBody]UserSetting UserSetting)
        {
            String resp;
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var L2EQuery = context.UserSettings.Where(f => f.UserId == UserSetting.UserId);

                    if (L2EQuery.Count() > 0)
                    {
                      
                          context.UserSettings.Remove(L2EQuery.FirstOrDefault());
                    }
                           
                   context.UserSettings.Add(UserSetting);
                   context.SaveChanges();
                    var newSetting = context.ViewUserSettings.Where(f => f.UserID == UserSetting.UserId).ToList() ;

                    resp = "{\"Response\":\"OK\"}";
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;

                }
            }
            catch (Exception ex)
            {
                Log.log("Error in Update User Setting at: " + DateTime.Now.ToString());
                resp = "{\"Response\":\"Fail\"}";
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
        }

      
        [Route("Add/Demo")]
        [HttpPost]
        public ValidUser AddUser([FromBody]TotalUser User)
        {
            String resp;
            ValidUser DemoUser = new ValidUser();
            try
            {
                var context = new SocietyDBEntities();
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    var users = (from USER in context.ViewSocietyUsers
                                 where USER.MobileNo == User.MobileNo || USER.EmailId == User.EmailId
                                 select USER);
                    if (users.Count() > 0)
                    {
                        DemoUser.result = "Fail";
                        DemoUser.message = "No Valid User";

                        //return BadRequest();

                        //resp = "{\"Response\":\"Fail\"}";
                        //var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                        //response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                        //return response;

                        return DemoUser;

                    }
                    else
                    {
                        String encryptPwd = ValidateUser.EncryptPassword(User.EmailId, User.Password);
                        User.Password = encryptPwd;

                        // Add User
                        context.TotalUsers.Add(User
                            );
                        context.SaveChanges();

                        Flat newFlat = new Flat
                        {
                            FlatNumber = User.FirstName.Substring(0,1) + User.LastName.Substring(0,1) + User.MobileNo.Substring(7,3),
                            BHK = 3,
                            Block = User.FirstName.Substring(0, 1),
                            FlatArea = "1200",
                            Floor =  Convert.ToInt32(User.MobileNo.Substring(9, 1)),
                            IntercomNumber = Convert.ToInt32(User.MobileNo.Substring(5, 5)),
                            SocietyID = 1,
                            UserID = User.UserID
                        };
                        // Add Flat
                        context.Flats.Add(newFlat);
                        context.SaveChanges();

                        SocietyUser demoSocietyUser = new SocietyUser
                        {
                            UserID = User.UserID,
                            SocietyID = 1,
                            ActiveDate = DateTime.UtcNow,
                            CompanyName = "",
                            DeActiveDate = DateTime.UtcNow.AddDays(15),
                            FlatID = newFlat.ID,
                            ModifiedDate = DateTime.UtcNow,
                            ServiceType = 0,
                            Type = "Owner"
                        };

                     

                        context.SocietyUsers.Add(demoSocietyUser);

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        var socUser = context.ViewSocietyUsers.Where(x => x.ResID == demoSocietyUser.ResID).First();
                        DemoUser.UserData = User;
                        DemoUser.result = "Ok";
                        DemoUser.SocietyUser.Add(socUser);

                        var sub = "Your Demo ID is created";
                        var EmailBody = "Dear User \n You have successfully Registered with Nestin.Online For Demo. You demo will run for 15 days. Please" +
                                        "Explore the application and contact us for any further query";
                        var smsBody = "Welcome to Nestin.online. your demo login is valid for 15 days.";

                        Utility.SendMail(User.EmailId, sub, EmailBody);
                        Utility.sendSMS2Resident(smsBody, User.MobileNo);
                        //return Ok();
                        //resp = "{\"Response\":\"Ok\"}";
                        //var response = Request.CreateResponse(HttpStatusCode.OK);
                        //response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                        return DemoUser;

                    }

                }
            }
            catch (Exception ex)
            {
                //return InternalServerError(ex.InnerException);
                //resp = "{\"Response\":\"Fail\"}";
                //var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                //response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                //return response;

                DemoUser.result = "Fail";
                DemoUser.message = "Server Error";
                return DemoUser;
            }
          
        }



        [Route("Add/Register")]
        [HttpPost]
        public ValidUser AddNewUser([FromBody]TotalUser User)
        {
            String resp;
            ValidUser DemoUser = new ValidUser();
            try
            {
                var context = new SocietyDBEntities();
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    var users = (from USER in context.ViewSocietyUsers
                                 where USER.MobileNo == User.MobileNo || USER.EmailId == User.EmailId
                                 select USER);
                    if (users.Count() > 0)
                    {
                        DemoUser.result = "Fail";
                        DemoUser.message = "No Valid User";

                        //return BadRequest();

                        //resp = "{\"Response\":\"Fail\"}";
                        //var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                        //response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                        //return response;

                        return DemoUser;

                    }
                    else
                    {
                        String encryptPwd = ValidateUser.EncryptPassword(User.EmailId, User.Password);
                        User.Password = encryptPwd;

                        // Add User
                        context.TotalUsers.Add(User
                            );
                        context.SaveChanges();

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        DemoUser.UserData = User;
                        DemoUser.result = "Ok";

                        var sub = "Your User Login is created";
                        var EmailBody = "Dear User \n You have successfully Registered with Nestin.Online. Please select your Role from Role Page";
                        var smsBody = "Welcome to Nestin.online. your Registration is succesfull.";

                        Utility.SendMail(User.EmailId, sub, EmailBody);
                        Utility.sendSMS2Resident(smsBody, User.MobileNo);

                        //return Ok();
                        //resp = "{\"Response\":\"Ok\"}";
                        //var response = Request.CreateResponse(HttpStatusCode.OK);
                        //response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                        return DemoUser;

                    }

                }
            }
            catch (Exception ex)
            {
                //return InternalServerError(ex.InnerException);
                //resp = "{\"Response\":\"Fail\"}";
                //var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                //response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                //return response;

                DemoUser.result = "Fail";
                DemoUser.message = "Server Error";
                return DemoUser;
            }

        }


        [Route("Add/SocietyUser")]
        [HttpPost]
        public ValidUser AddUserFlat([FromBody]SocietyUser socUser)
        {
         
            ValidUser DemoUser = new ValidUser();
            try
            {
                var context = new SocietyDBEntities();
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    var users = (from USER in context.TotalUsers
                                 where USER.UserID == socUser.UserID
                                 select USER).First();
                    if (users == null)
                    {
                        DemoUser.result = "Fail";
                        DemoUser.message = "No Valid User";
                        return DemoUser;

                    }
                    else
                    {
                       
                        context.SocietyUsers.Add(socUser);

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        var viewSocUser = context.ViewSocietyUsers.Where(x => x.ResID == socUser.ResID).First();
                        DemoUser.UserData = users;
                        DemoUser.result = "Ok";
                        DemoUser.SocietyUser.Add(viewSocUser);

                        var sub = "Your Role is created";
                        var EmailBody = "Dear User \n You have successfully Registered with Nestin.Online For Demo." +
                                        "Explore the application and contact us for any further query";
                        var smsBody = "Welcome to Nestin.online. your demo login is valid for 15 days.";

                        Utility.SendMail(users.EmailId, sub, EmailBody);
                        Utility.sendSMS2Resident(smsBody, users.MobileNo);
                        //return Ok();
                        //resp = "{\"Response\":\"Ok\"}";
                        //var response = Request.CreateResponse(HttpStatusCode.OK);
                        //response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                        return DemoUser;

                    }

                }
            }
            catch (Exception ex)
            {
                //return InternalServerError(ex.InnerException);
                //resp = "{\"Response\":\"Fail\"}";
                //var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                //response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                //return response;

                DemoUser.result = "Fail";
                DemoUser.message = "Server Error";
                return DemoUser;
            }

        }



        [Route("Add/House/{UserId}")]
        [HttpPost]
        public IHttpActionResult AddHouse([FromBody]House newHouse, int UserId)
        {
            try
            {
                var context = new SocietyDBEntities();
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {

                    context.Houses.Add(newHouse);
                    context.SaveChanges();

                    context.SocietyUsers.Add(new SocietyUser {
                        UserID = UserId,
                        FlatID = 0,
                        HouseID = newHouse.ID,
                        CompanyName = "NA",
                        ServiceType = 0,
                        SocietyID = 0,
                        Status =0,
                        Type= "Individual",                       
                        ActiveDate = DateTime.UtcNow,
                        DeActiveDate = DateTime.UtcNow.AddYears(1),
                        ModifiedDate = DateTime.UtcNow
                    });

                    context.SaveChanges();
                    dbContextTransaction.Commit();

                    var User = context.ViewSocietyUsers.Where(x => x.UserID == UserId).First();

                    var sub = "Your Demo ID is created";
                    var EmailBody = "Dear User \n You have successfully Registered with Nestin.Online For Demo. You demo will run for 15 days. Please" +
                                    "Explore the application and contact us for any further query";
                    var smsBody = "Welcome to Nestin.online. your demo login is valid for 15 days.";

                    Utility.SendMail(User.EmailId, sub, EmailBody);
                    Utility.sendSMS2Resident(smsBody, User.MobileNo);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.InnerException);
            }

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
