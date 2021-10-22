using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceAutomation.Model
{
    class RestResponse
    {
        private int statuscode;
        private string responsedata;

        public RestResponse(int statuscode,string responsedata)
        {
            this.statuscode = statuscode;
            this.responsedata = responsedata;
        }

        public int Statuscode
        {
            get
            {
                return statuscode;
            }
        }


        public string ResponseData {

            get 
            { 
                return responsedata; 
            }
        }

        public override string ToString()
        {
            return String.Format("StatusCode : {0} ResponseData : {1}", statuscode, responsedata);
        }
    }
}
