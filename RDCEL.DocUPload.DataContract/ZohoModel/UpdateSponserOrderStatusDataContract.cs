using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class UpdateSponserOrderStatusDataContract
    {
        
        [DataMember]
        public string ID { get; set; }     
        [DataMember]
        public string Latest_Status { get; set; }
        // [DataMember]
        //public string Secondary_Order_Flag { get; set; }
        //[DataMember]
        //public string Status_Reason { get; set; }
        [DataMember]
        public string Order { get; set; }
        [DataMember]
        public string Installation { get; set; }

    }
    public class UpdateExchangeOrderPickupDateDataContract
    {

        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Expected_Pickup_Date { get; set; }
        [DataMember]
        public string Latest_Status { get; set; }
        [DataMember]
        public string Order { get; set; }
        [DataMember]
        public string Installation { get; set; }

    }


    //public class UpdateSponserOrderStatusFormResponseDataContract
    //{
    //    [DataMember]
    //    public int code { get; set; }
    //    [DataMember]
    //    public Data data { get; set; }
    //    [DataMember]
    //    public string message { get; set; }

    //}

    public class UpdateSponserOrderStatusFormRequestDataContract
    {
        public UpdateSponserOrderStatusFormRequestDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public UpdateSponserOrderStatusDataContract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }

    public class UpdateSponserOrderPickupDateFormRequestDataContract
    {
        public UpdateSponserOrderPickupDateFormRequestDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public UpdateExchangeOrderPickupDateDataContract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }
}
