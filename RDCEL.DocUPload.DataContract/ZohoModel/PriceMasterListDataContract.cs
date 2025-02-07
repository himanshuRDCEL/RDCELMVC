using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class PriceMasterListDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<PriceMasterData> data { get; set; }
    }

    public class PriceMasterData
    {
            
        [DataMember]
        public string Exch_price_ID { get; set; }    
        [DataMember]
        public ProdGroup Prod_Group { get; set; }
        [DataMember]
        public ProdType Prod_Type { get; set; }
        [DataMember]
        public Size Size { get; set; }         
        [DataMember]
        public Brand1 Brand_1 { get; set; }
        [DataMember]
        public Brand2 Brand_2 { get; set; }
        [DataMember]
        public Brand3 Brand_3 { get; set; }
        [DataMember]
        public Brand4 Brand_4 { get; set; }
        [DataMember]
        public string Quote_P_H { get; set; }
        [DataMember]
        public string Quote_Q_H { get; set; }
        [DataMember]
        public string Quote_R_H { get; set; }
        [DataMember]
        public string Quote_S_H { get; set; }
        [DataMember]
        public string Quote_P { get; set; }
        [DataMember]
        public string Quote_Q { get; set; }
        [DataMember]
        public string Quote_R { get; set; }
        [DataMember]
        public string Quote_S { get; set; }
        [DataMember]
        public string Price_Start_Date { get; set; }
        [DataMember]
        public string Price_End_Date { get; set; }
        [DataMember]
        public string ID { get; set; }
       
    }
   
    public class ProdType
    {
        [DataMember]
        public string display_value { get; set; }
        [DataMember]
        public string ID { get; set; }
    }

    public class ProdGroup
    {
        [DataMember]
        public string display_value { get; set; }
        [DataMember]
        public string ID { get; set; }
    }

    

    public class Brand1
    {
        [DataMember]
        public string display_value { get; set; }
        [DataMember]
        public string ID { get; set; }
    }
    public class Brand2
    {
        [DataMember]
        public string display_value { get; set; }
        [DataMember]
        public string ID { get; set; }
    }
    public class Brand3
    {
        [DataMember]
        public string display_value { get; set; }
        [DataMember]
        public string ID { get; set; }
    }

    public class Brand4
    {
        [DataMember]
        public string display_value { get; set; }
        [DataMember]
        public string ID { get; set; }
    }
    public class Size
    {
        [DataMember]
        public string display_value { get; set; }
        [DataMember]
        public string ID { get; set; }
    }

}
