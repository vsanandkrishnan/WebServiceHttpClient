using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceAutomation.Helper.Authentication
{
    public class Base64StringConverter
    {
        /// <summary>
        /// COnverting username and password to Base64 format
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Encoded string for username and password</returns>
        public static string GetBase64String(string userName, string password)
        {
           string auth = userName + ":" + password;
           byte[] inArray=System.Text.UTF8Encoding.UTF8.GetBytes(auth);
           return  System.Convert.ToBase64String(inArray);
        }
    }
}
