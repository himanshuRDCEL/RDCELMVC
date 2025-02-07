using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class ExchageOrderUpdateDataContract
    {
    }

    public class ExchageOrderVoucherUpdateDataContract
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Voucher_Code { get; set; }
        [DataMember]
        public string Is_Voucher_Redeemed { get; set; }
        [DataMember]
        public string Voucher_Redeemed_By { get; set; }
        [DataMember]
        public string Store_Code { get; set; }
        [DataMember]
        public string Voucher_Amount { get; set; }
        [DataMember]
        public string New_Product_Name { get; set; }
        [DataMember]
        public string New_Product_Code { get; set; }
        [DataMember]
        public string Associate_Code { get; set; }
        [DataMember]
        public string Amount_Payable_Through_LGC { get; set; }
        [DataMember]
        public string Voucher_Redeem_Date { get; set; }
        public string Associate_Name { get; set; }
        public string Associate_Email { get; set; }
        
        [DataMember]
        public string Retailer_Phone_Number { get; set; }
        public string Purchased_Product_Category { get; set; }
    }

   

    public class ExchageOrderVoucherUpdateFormDataContract
    {
        public ExchageOrderVoucherUpdateFormDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public ExchageOrderVoucherUpdateDataContract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }

    #region For Samsung

    public class ExchageOrderVoucherUpdateSamsungDataContract
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Amount_Payable_Through_LGC { get; set; }

    }

    public class ExchageOrderVoucherUpdateFormSamsungDataContract
    {
        public ExchageOrderVoucherUpdateFormSamsungDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public ExchageOrderVoucherUpdateSamsungDataContract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }

    #endregion

    #region Update Customer Details for Pine Labs
    public class CustomerDetailsUpdateFormDataContract
    {
        public CustomerDetailsUpdateFormDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public CustomerDetailsUpdateDatacontract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }
    #endregion
}
