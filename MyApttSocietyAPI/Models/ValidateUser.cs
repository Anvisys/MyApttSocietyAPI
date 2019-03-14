using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Cryptography;
using System.Text;

namespace MyApttSocietyAPI.Models
{
    public class ValidateUser
    {
        public String Mobile { get; set; }
        public String Email { get; set; }

        public String Password { get; set; }

        public String RegistrationID { get; set; }


        public static string EncryptPassword(string userID, string userPWD)
        {

            MD5CryptoServiceProvider encoder = new MD5CryptoServiceProvider();
            byte[] bytDataToHash = Encoding.UTF8.GetBytes(userID.ToLower() + userPWD);
            byte[] bytHashValue = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(bytDataToHash);
            return BitConverter.ToString(bytHashValue).Replace("-", "");
        }
    }
}