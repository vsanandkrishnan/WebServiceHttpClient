using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceAutomation.Model.JsonModel
{
    public class ResponseV2Json
    {
        public string BrandName { get; set; }
        public Features Features { get; set; }
        public int Id { get; set; }
        public string LaptopName { get; set; }
    }
}
