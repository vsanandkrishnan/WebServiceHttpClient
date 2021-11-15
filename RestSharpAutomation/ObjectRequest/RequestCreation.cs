using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.JsonModel;

namespace RestSharpAutomation.ObjectRequest
{
    public class RequestCreation
    {
        
        public static ResponseV2Json GetLaptopObject(int id)
        {
            ResponseV2Json laptop = new ResponseV2Json();
            laptop.BrandName = "Dell Inspirion";
            laptop.LaptopName = "Dell Latitude Inspirion";

            Features features = new Features();

            List<string> featureList = new List<string>()
            {
                ("Simple Feature one")
            };

            features.Feature = featureList;

            laptop.Features = features;

            laptop.Id = id;

            return laptop;
        }
    }
}
