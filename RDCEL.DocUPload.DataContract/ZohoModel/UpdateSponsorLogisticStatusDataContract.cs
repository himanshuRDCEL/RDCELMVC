using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class UpdateSponsorLogisticStatusDataContract
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string LGC_Tkt_No { get; set; }
        [DataMember]
        public string EVC_Drop { get; set; }
        [DataMember]
        public string Pickup { get; set; }
        [DataMember]
        public string Latest_Status { get; set; }
        [DataMember]
        public string Secondary_Order_Flag { get; set; }
        [DataMember]
        public string Status_Reason { get; set; }
        //[DataMember]
        //public string Order { get; set; }
        [DataMember]
        public string Logistic_By { get; set; }
        [DataMember]
        public string Pickup_Priority { get; set; }


    }

    public class UpdateSponsorLogisticStatusFormRequestDataContract
    {
        public UpdateSponsorLogisticStatusFormRequestDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public UpdateSponsorLogisticStatusDataContract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }

    #region To update the status
    public class LogisticStatusDataContract
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string LGC_Tkt_No { get; set; }
        [DataMember]
        public string EVC_Drop { get; set; }
        [DataMember]
        public string Pickup { get; set; }
        [DataMember]
        public string Latest_Status { get; set; }
        [DataMember]
        public string Secondary_Order_Flag { get; set; }
        [DataMember]
        public string Status_Reason { get; set; }


    }

    public class LogisticStatusFormRequestDataContract
    {
        public LogisticStatusFormRequestDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public LogisticStatusDataContract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }
    #endregion

    #region To  update customer details
   
    public class CustomerDetailsUpdateDatacontract
    {
       [DataMember]
        public CustomerName Customer_Name { get; set; }
        
        [DataMember]
        public string Id { get; set; }
        
        [DataMember]
        public string Customer_Email_Address { get; set; }
        [DataMember]
        public string Customer_Address_1 { get; set; }
        [DataMember]
        public string Customer_City { get; set; }
        [DataMember]
        public string Customer_Mobile { get; set; }
        [DataMember]
        public string Customer_Pincode { get; set; }
        [DataMember]
        public string Customer_Address_2 { get; set; }
        [DataMember]
        public string Customer_Address_Landmark { get; set; }
        [DataMember]
        public string Customer_State_Name { get; set; }

    }
    #endregion
}
