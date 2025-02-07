using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class ABBSingleDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<ABBSingleData> data { get; set; }
    }

    public class ABBSingleData
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Regd_No { get; set; }
        //public string New_Brand { get; set; }
        //public string ABB_Plan_Period_Months { get; set; }
        //public string New_Price { get; set; }
        //public string ABB_Fees { get; set; }
        //public string Sponsor_Order_No { get; set; }
        //public string Invoice_Date { get; set; }
        //public string Order_Type { get; set; }
        //public string Invoice_No { get; set; }
        //public string ABB_Price_Id { get; set; }
        //public string ABB_Plan_Name { get; set; }
        //public SponsorName Sponsor_Name { get; set; }
        //public string New_Prod_Group { get; set; }
        //public string HSN_Code_For_ABB_Fees { get; set; }
       
        //public string Cust_City { get; set; }
        //public string Cust_Mobile { get; set; }
        
        //public string Upload_Date_Time { get; set; }
        //public string Cust_Pin_Code { get; set; }
        //public string New_Size { get; set; }
        //public string Model_No { get; set; }
        //public StoreCode Store_Code { get; set; }
        //public CustName Cust_Name { get; set; }
        //public string Landmark { get; set; }
        //public string Sponsor_Status { get; set; }
        //public string Cust_E_mail { get; set; }
        //public string Cust_Add_2 { get; set; }
        //public string New_Prod_Type { get; set; }
        //public string Cust_State { get; set; }
        ////public string Prod_Sr_No { get; set; }
        ////public string Sponsor_Prog_code { get; set; }
        //public string Cust_Add_1 { get; set; }
    }
   
}
