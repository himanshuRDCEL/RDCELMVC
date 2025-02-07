using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class ABBRegistrationDataContract
    {
        [DataMember]
        public string Sponsor_Name { get; set; }
        [DataMember]
        public string Store_Code { get; set; }
        [DataMember]
        public string Regd_No { get; set; }
        [DataMember]
        public string Sponsor_Order_No { get; set; }
        [DataMember]
        public CustName Cust_Name { get; set; }
        [DataMember]
        public string Cust_Mobile { get; set; }
        [DataMember]
        public string Cust_E_mail { get; set; }
        [DataMember]
        public string Cust_Add_1 { get; set; }
        [DataMember]
        public string Cust_Add_2 { get; set; }
        [DataMember]
        public string Customer_Location { get; set; }
        [DataMember]
        public string Cust_Pin_Code { get; set; }
        [DataMember]
        public string Cust_City { get; set; }
        [DataMember]
        public string Cust_State { get; set; }
        [DataMember]
        public string New_Prod_Group { get; set; }
        [DataMember]
        public string New_Prod_Type { get; set; }
        [DataMember]
        public string New_Brand { get; set; }
        [DataMember]
        public string New_Size { get; set; }
        [DataMember]
        public string Prod_Sr_No { get; set; }
        [DataMember]
        public string Model_No { get; set; }
        [DataMember]
        public string ABB_Plan_Name { get; set; }
        [DataMember]
        public string HSN_Code_For_ABB_Fees { get; set; }
        [DataMember]
        public string Invoice_Date { get; set; }
        [DataMember]
        public string Invoice_No { get; set; }
        [DataMember]
        public string New_Price { get; set; }
        [DataMember]
        public string ABB_Fees { get; set; }
        [DataMember]
        public string Product_Net_Price_Incl_Of_GST { get; set; }
        [DataMember]
        public string ABB_Plan_Period_Months { get; set; }
        [DataMember]
        public string Order_Type { get; set; }
        [DataMember]
        public string Invoice_Image { get; set; }
        [DataMember]
        public string Sponsor_Prog_code { get; set; }
        [DataMember]
        public string No_Of_Claim_Period_Months { get; set; }
        [DataMember]
        public string ABB_Price_Id { get; set; }
        [DataMember]
        public string Upload_Date_Time { get; set; }
        [DataMember]
        public string Upload_Date { get; set; }
        [DataMember]
        public string Sponsor_Status { get; set; }
        [DataMember]
        public string Mar_Com { get; set; }
        [DataMember]
        public string Approve_this { get; set; }
        [DataMember]
        public string Is_ABB_Payment_Done { get; set; }

        [DataMember]
        public string Payment_Remark { get; set; }

        [DataMember]
        public string Transaction_Id { get; set; }
        [DataMember]
        public string Order_Id { get; set; }

    }
    public class CustName
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class ABBRegistrationFormResponseDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public Data data { get; set; }
        [DataMember]
        public string message { get; set; }

    }

    public class ABBRegistrationFormRequestDataContract
    {
        public ABBRegistrationFormRequestDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public ABBRegistrationDataContract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }

}
