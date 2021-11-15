using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XMLModel;

namespace RestSharpAutomation.ObjectRequest
{
    public class RequestCreation
    {
        
        public static ResponseV2Json GetLaptopObject(int id)
        {
            ResponseV2Json laptop = new ResponseV2Json();
            laptop.BrandName = "Dell Inspirion";
            laptop.LaptopName = "Dell Latitude Inspirion";

            WebServiceAutomation.Model.JsonModel.Features features = new WebServiceAutomation.Model.JsonModel.Features();

            List<string> featureList = new List<string>()
            {
                ("Simple Feature one")
            };

            features.Feature = featureList;

            laptop.Features = features;

            laptop.Id = id;

            return laptop;
        }

        public static Laptop GetLaptopObjectXml(int id)
        {
            Laptop laptop = new Laptop();
            laptop.BrandName = "Dell Inspirion";
            laptop.LaptopName = "Dell Latitude Inspirion";

            WebServiceAutomation.Model.XMLModel.Features features = new WebServiceAutomation.Model.XMLModel.Features();

            List<string> featureList = new List<string>()
            {
                ("Simple Feature one")
            };

            features.Feature = featureList;

            laptop.Features = features;

            laptop.Id = id.ToString();

            return laptop;
        }
    }
}
