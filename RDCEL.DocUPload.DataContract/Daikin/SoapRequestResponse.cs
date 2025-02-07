using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.Daikin
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Customer
    {
        public string ReferenceObjectNodeSenderTechnicalID { get; set; }
        public string ChangeStateID { get; set; }
        public string InternalID { get; set; }
        public string UUID { get; set; }
    }

    public class Item
    {
        public string TypeID { get; set; }
        public string CategoryCode { get; set; }
        public string SeverityCode { get; set; }
        public string Note { get; set; }

    }

    public class Log
    {
        public string MaximumLogItemSeverityCode { get; set; }
        public List<Item> Item { get; set; }
    }

    public class Ns2CustomerBundleMaintainConfirmationSyncV1
    {
        [JsonProperty("@xmlns:ns2")]
        public string xmlnsns2 { get; set; }
        public Customer Customer { get; set; }
        public Log Log { get; set; }
    }

    public class DaikinCustomerDataContract 
    {
        [JsonProperty("soap:Envelope")]
        public SoapEnvelope soapEnvelope { get; set; }
    }

    public class SoapBody
    {
        [JsonProperty("ns2:CustomerBundleMaintainConfirmation_sync_V1")]
        public Ns2CustomerBundleMaintainConfirmationSyncV1 ns2CustomerBundleMaintainConfirmation_sync_V1 { get; set; }

        [JsonProperty("ns2:ServiceRequestBundleMaintainConfirmation2_sync")]
        public Ns2ServiceRequestBundleMaintainConfirmation2Sync ns2ServiceRequestBundleMaintainConfirmation2_sync { get; set; }
        public IndividualCustomerCollection IndividualCustomerCollection { get; set; }
        public IndividualOrderCollection IndividualOrderCollection { get; set; }
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

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class Ns2ServiceRequestBundleMaintainConfirmation2Sync
    {
        [JsonProperty("@xmlns:ns2")]
        public string xmlnsns2 { get; set; }
        public Log Log { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class IndividualCustomer
    {
        public string POBoxIndicator { get; set; }
        public object VisitDuration { get; set; }
        public object ExternalSystem { get; set; }
        public object POBoxDeviatingStateCode { get; set; }
        public string FormattedName { get; set; }
        public object ContactPermissionCodeText { get; set; }
        public object AcademicTitleCode { get; set; }
        public string CustomerID { get; set; }
        public string EmailInvalidIndicator { get; set; }
        public object Street { get; set; }
        public string CreatedBy { get; set; }
        public string ChangedBy { get; set; }
        public object LastVisitingDate { get; set; }
        public object Initials { get; set; }
        public object AddressLine2 { get; set; }
        public object AddressLine1 { get; set; }
        public object TaxJurisdictionCode { get; set; }
        public object MaritalStatusCode { get; set; }
        public DateTime ETag { get; set; }
        public object AddressLine5 { get; set; }
        public object AddressLine4 { get; set; }
        public object RecommendedVisitingFrequency { get; set; }
        public object AcademicTitleCodeText { get; set; }
        public object POBoxDeviatingCountryCodeText { get; set; }
        public object CareOfName { get; set; }
        public object Building { get; set; }
        public object ContactPermissionCode { get; set; }
        public string HouseNumber { get; set; }
        public string RoleCode { get; set; }
        public object CustomerABCClassificationCodeText { get; set; }
        public string StateCodeText { get; set; }
        public string StreetPostalCode { get; set; }
        public object POBoxDeviatingCountryCode { get; set; }
        public string TimeZoneCodeText { get; set; }
        public object NamePrefixCode { get; set; }
        public object TitleCode { get; set; }
        public string RoleCodeText { get; set; }
        public object NamePrefixCodeText { get; set; }
        public object LatestRecommendedVisitingDate { get; set; }
        public object NextVisitingDate { get; set; }
        public string FirstName { get; set; }
        public object BillingBlockingReasonCode { get; set; }
        public string ZDnD_SDK { get; set; }
        public DateTime EntityLastChangedOn { get; set; }
        public string Mobile { get; set; }
        public object OrderBlockingReasonCode { get; set; }
        public object POBoxDeviatingCity { get; set; }
        public string ChangedByIdentityUUID { get; set; }
        public object POBoxPostalCode { get; set; }
        public string TimeZoneCode { get; set; }
        public string SalesSupportBlockingIndicator { get; set; }
        public object DifferentCity { get; set; }
        public object POBoxDeviatingStateCodeText { get; set; }
        public string Email { get; set; }
        public string ObjectID { get; set; }
        public object TitleCodeText { get; set; }
        public DateTime CreationOn { get; set; }
        public object LanguageCode { get; set; }
        public object NormalisedPhone { get; set; }
        public string FormattedPostalAddressDescription { get; set; }
        public object BillingBlockingReasonCodeText { get; set; }
        public string LifeCycleStatusCode { get; set; }
        public object ProfessionCode { get; set; }
        public string UUID { get; set; }
        public object BirthName { get; set; }
        public string GenderCode { get; set; }
        public object BestReachedByCodeText { get; set; }
        public object LanguageCodeText { get; set; }
        public object CustomerABCClassificationCode { get; set; }
        public string City { get; set; }
        public object NickName { get; set; }
        public string NormalisedMobile { get; set; }
        public object OrderBlockingReasonCodeText { get; set; }
        public object OwnerUUID { get; set; }
        public object AdditionalHouseNumber { get; set; }
        public object BestReachedByCode { get; set; }
        public object OwnerID { get; set; }
        public object WebSite { get; set; }
        public string LastName { get; set; }
        public object MaritalStatusCodeText { get; set; }
        public DateTime ChangedOn { get; set; }
        public object District { get; set; }
        public object BirthDate { get; set; }
        public object POBox { get; set; }
        public object ExternalID { get; set; }
        public string CountryCodeText { get; set; }
        public string GenderCodeText { get; set; }
        public object NationalityCountryCode { get; set; }
        public object ProfessionCodeText { get; set; }
        public object County { get; set; }
        public string LifeCycleStatusCodeText { get; set; }
        public string AreaLocality_KUT { get; set; }
        public string CreatedByIdentityUUID { get; set; }
        public object Phone { get; set; }
        public object AdditionalLastName { get; set; }
        public string CountryCode { get; set; }
        public object Room { get; set; }
        public object NationalityCountryCodeText { get; set; }
        public object Floor { get; set; }
        public object DeliveryBlockingReasonCodeText { get; set; }
        public string StateCode { get; set; }
        public object MiddleName { get; set; }
        public object TaxJurisdictionCodeText { get; set; }
        public object Fax { get; set; }
        public object DeliveryBlockingReasonCode { get; set; }
    }

    public class IndividualCustomerCollection
    {
        public List<IndividualCustomer> IndividualCustomer { get; set; }
    }

    public class IndividualOrderCollection
    {
        public List<IndividualOrder> IndividualOrder { get; set; }
    }


    public class IndividualOrder
    {
        public string ServiceRequestLifeCycleStatusCode { get; set; }
       
    }
}
