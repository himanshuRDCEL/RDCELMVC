using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.DaikinModel
{


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Item
    {
        public string TypeID { get; set; }
        public string SeverityCode { get; set; }
        public string Note { get; set; }
    }

    public class Log
    {
        public string MaximumLogItemSeverityCode { get; set; }
        public List<Item> Item { get; set; }
    }

    public class Ns2ServiceRequestBundleMaintainConfirmation2Sync
    {
        [JsonProperty("@xmlns:ns2")]
        public string xmlnsns2 { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public Log Log { get; set; }
    }

    public class RequestSerciceresponse
    {
        [JsonProperty("soap:Envelope")]
        public SoapEnvelope soapEnvelope { get; set; }
    }

    public class ServiceRequest
    {
        public string ReferenceObjectNodeSenderTechnicalID { get; set; }
        public string ChangeStateID { get; set; }
        public string UUID { get; set; }
        public string ID { get; set; }
    }

    public class SoapBody
    {
        [JsonProperty("ns2:ServiceRequestBundleMaintainConfirmation2_sync")]
        public Ns2ServiceRequestBundleMaintainConfirmation2Sync ns2ServiceRequestBundleMaintainConfirmation2_sync { get; set; }
    }

    public class SoapEnvelope
    {
        [JsonProperty("@xmlns:soap")]
        public string xmlnssoap { get; set; }

        [JsonProperty("soap:Header")]
        public object soapHeader { get; set; }

        [JsonProperty("soap:Body")]
        public SoapBody soapBody { get; set; }
    }



}
