
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using RDCEL.DocUpload.DataContract.Daikin;

namespace ConsoleApp.Model
{
    public class ExchangeOrderViewModel
    {
        public int ExchangeOrderID { get; set; }
        public string RegdNo { get; set; }
        public string SponsorServiceRefId { get; set; }

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Root
    {
        [JsonProperty("soap:Envelope")]
        public SoapEnvelope soapEnvelope { get; set; }
    }

    public class ServiceRequestParty
    {
        public string CustomerMobile { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string PartyID { get; set; }
        public string ServiceRequestUserLifeCycleStatusCode { get; set; }
        public string ObjectID { get; set; }
        public DateTime ETag { get; set; }
        public string ServiceRequestLifeCycleStatusCode { get; set; }
        public DateTime LastChangeDateTime { get; set; }
        public string ID { get; set; }
        public string ProcessingTypeCode { get; set; }
        public string RoleCode { get; set; }
        public string Name { get; set; }
    }

    public class ServiceRequestPartyCollection
    {
        public ServiceRequestParty ServiceRequestParty { get; set; }
    }

    public class SoapBody
    {
        public ServiceRequestPartyCollection ServiceRequestPartyCollection { get; set; }
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
