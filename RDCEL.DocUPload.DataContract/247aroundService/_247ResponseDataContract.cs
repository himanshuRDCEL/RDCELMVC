using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace RDCEL.DocUpload.DataContract._247aroundService
{
    public class _247ResponseDataContract
    {
        public Data data { get; set; }
    }
    public class Data
    {
        public Response response { get; set; }
        public int code { get; set; }
        public string result { get; set; }


    }
    public class Response
    {
        [JsonProperty(PropertyName = "247aroundBookingID")]
        public string _247aroundBookingID { get; set; }
        [JsonProperty(PropertyName = "247aroundBookingStatus")]
        public string _247aroundBookingStatus { get; set; }
        [JsonProperty(PropertyName = "247aroundBookingRemarks")]
        public string _247aroundBookingRemarks { get; set; }

    }




}
