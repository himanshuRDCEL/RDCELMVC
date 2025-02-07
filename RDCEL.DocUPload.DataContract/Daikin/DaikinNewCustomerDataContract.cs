using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.Daikin
{
   
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CustomerNew
    {
        public string ReferenceObjectNodeSenderTechnicalID { get; set; }
        public string ChangeStateID { get; set; }
        public string InternalID { get; set; }
        public string UUID { get; set; }
    }

    public class ItemNew
    {
        public string TypeID { get; set; }
        public string CategoryCode { get; set; }
        public string SeverityCode { get; set; }
        public string Note { get; set; }
    }

    public class LogNew
    {
        public string MaximumLogItemSeverityCode { get; set; }
        public ItemNew Item { get; set; }
    }

    public class Ns2CustomerBundleMaintainConfirmationSyncV1New
    {
        [JsonProperty("@xmlns:ns2")]
        public string xmlnsns2 { get; set; }
        public CustomerNew Customer { get; set; }
        public LogNew Log { get; set; }
    }

    public class DaikinNewCustomerDataContractNew
    {
        [JsonProperty("soap:Envelope")]
        public SoapEnvelopeNew soapEnvelope { get; set; }
    }

    public class SoapBodyNew
    {
        [JsonProperty("ns2:CustomerBundleMaintainConfirmation_sync_V1")]
        public Ns2CustomerBundleMaintainConfirmationSyncV1New ns2CustomerBundleMaintainConfirmation_sync_V1 { get; set; }
    }

    public class SoapEnvelopeNew
    {
        [JsonProperty("@xmlns:soap")]
        public string xmlnssoap { get; set; }

        [JsonProperty("soap:Header")]
        public object soapHeader { get; set; }

        [JsonProperty("soap:Body")]
        public SoapBodyNew soapBody { get; set; }
    }


}
