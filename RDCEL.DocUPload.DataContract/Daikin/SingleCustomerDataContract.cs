using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.Daikin
{


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class IndividualCustomersi
    {
        public string POBoxIndicator { get; set; }
        public object VisitDuration { get; set; }
        public string ExternalSystem { get; set; }
        public object POBoxDeviatingStateCode { get; set; }
        public string FormattedName { get; set; }
        public object ContactPermissionCodeText { get; set; }
        public object AcademicTitleCode { get; set; }
        public string CustomerID { get; set; }
        public string EmailInvalidIndicator { get; set; }
        public string Street { get; set; }
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
        public string TitleCode { get; set; }
        public string RoleCodeText { get; set; }
        public object NamePrefixCodeText { get; set; }
        public object LatestRecommendedVisitingDate { get; set; }
        public object NextVisitingDate { get; set; }
        public object FirstName { get; set; }
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
        public object Email { get; set; }
        public string ObjectID { get; set; }
        public string TitleCodeText { get; set; }
        public DateTime CreationOn { get; set; }
        public string LanguageCode { get; set; }
        public string NormalisedPhone { get; set; }
        public string FormattedPostalAddressDescription { get; set; }
        public object BillingBlockingReasonCodeText { get; set; }
        public string LifeCycleStatusCode { get; set; }
        public string ProfessionCode { get; set; }
        public string UUID { get; set; }
        public object BirthName { get; set; }
        public string GenderCode { get; set; }
        public object BestReachedByCodeText { get; set; }
        public string LanguageCodeText { get; set; }
        public object CustomerABCClassificationCode { get; set; }
        public string City { get; set; }
        public object NickName { get; set; }
        public string NormalisedMobile { get; set; }
        public object OrderBlockingReasonCodeText { get; set; }
        public string OwnerUUID { get; set; }
        public string AdditionalHouseNumber { get; set; }
        public object BestReachedByCode { get; set; }
        public string OwnerID { get; set; }
        public object WebSite { get; set; }
        public string LastName { get; set; }
        public object MaritalStatusCodeText { get; set; }
        public DateTime ChangedOn { get; set; }
        public object District { get; set; }
        public object BirthDate { get; set; }
        public object POBox { get; set; }
        public string ExternalID { get; set; }
        public string CountryCodeText { get; set; }
        public string GenderCodeText { get; set; }
        public object NationalityCountryCode { get; set; }
        public string ProfessionCodeText { get; set; }
        public object County { get; set; }
        public string LifeCycleStatusCodeText { get; set; }
        public string AreaLocality_KUT { get; set; }
        public string CreatedByIdentityUUID { get; set; }
        public string Phone { get; set; }
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

    public class IndividualCustomerCollectionold
    {
        public IndividualCustomersi IndividualCustomer { get; set; }
    }

    public class SingleCustomerDataContract
    {
        [JsonProperty("soap:Envelope")]
        public SoapEnvelopeold soapEnvelope { get; set; }
    }

    public class SoapBodysi
    {
        public IndividualCustomerCollectionold IndividualCustomerCollection { get; set; }
    }

    public class SoapEnvelopeold
    {
        [JsonProperty("@xmlns:soap")]
        public string xmlnssoap { get; set; }

        [JsonProperty("soap:Header")]
        public object soapHeader { get; set; }

        [JsonProperty("soap:Body")]
        public SoapBodysi soapBody { get; set; }
    }


}
