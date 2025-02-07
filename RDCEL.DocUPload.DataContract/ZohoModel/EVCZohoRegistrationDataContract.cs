using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class EVCZohoRegistrationDataContract
    {
        public EVCZoho data { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class EVCZoho
    {
        public string EVC_Wallet_Amount_Copy { get; set; }
        public string Employee_Email_id { get; set; }
        public string Copy_of_Cancelled_Cheque { get; set; }
        public string Evc_Name { get; set; }
        public string Select_Employee_Id { get; set; }
        public string Insert_OTP { get; set; }
        public string OTP { get; set; }
        public string Name { get; set; }
        public string Regd_Address_Line_2 { get; set; }
        public string Regd_Address_Line_1 { get; set; }
        public string EVC_Name { get; set; }
        public string Employee_Name { get; set; }
        public string Bussiness_Name { get; set; }
        public string E_mail_ID { get; set; }
        public string Upload_GST_Registration { get; set; }
        public string Running_Balance { get; set; }
        public string EVC_Status { get; set; }
        public string I_Confirm_Terms_Condition { get; set; }
        public string State1 { get; set; }
        public string City1 { get; set; }
        public string Total_Of_Actual_Base_Amount { get; set; }
        public string City { get; set; }
        public string Amount_Add_In_Wallet { get; set; }
        public string Alternate_Mobile_Number { get; set; }
        public string Contact_Person_Address { get; set; }
        public string City_Code { get; set; }
        public string PIN_Code { get; set; }
        public string Total_Of_In_Progress { get; set; }
        public string Date_field { get; set; }
        public string Total_Of_Delivered { get; set; }
        public string Bank_Name { get; set; }
        public string IFSC_Code { get; set; }
        public string State { get; set; }
        public string EVC_Wallet_Amount { get; set; }
        public string EVC_Regd_No { get; set; }
        public string Type_of_Entity { get; set; }
        public string E_Waste_Registration_Number { get; set; }
        public string EVC_Mobile_Number { get; set; }
        public string Account_No { get; set; }
        public string E_Waste_Certificate { get; set; }
        public string Place { get; set; }
        public string GST_Number { get; set; }
    }
    #region EVC Registration Reponse

    public class EVCRegistrationResponse
    {
        public int code { get; set; }
        public EVCRegistrationInfo data { get; set; }
        public string message { get; set; }
    }

    public class EVCRegistrationInfo
    {
        public string ID { get; set; }
    }

    #endregion


}
