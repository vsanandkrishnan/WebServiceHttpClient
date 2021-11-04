using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebServiceAutomation.Helper.Responsedata
{
    public class ResponseDataHelper
    {

        /// <summary>
        /// Deserializing JSON response data into class object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseData"></param>
        /// <returns>Json Class <typeparamref name="T"/></returns>
        public static T DeserializeJsonResponse<T>(string responseData) where T: class
        {
            return JsonConvert.DeserializeObject<T>(responseData);
        }


        /// <summary>
        /// Deserializing XML response data into class object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseData"></param>
        /// <returns>Xml Class <typeparamref name="T"/></returns>
        public static T DesrializeXmlResponse<T>(string responseData) where T:class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            TextReader textReader = new StringReader(responseData);
            return (T)xmlSerializer.Deserialize(textReader);
        }
    }
}
