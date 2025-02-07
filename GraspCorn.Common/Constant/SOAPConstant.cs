using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Constant
{
    public class SOAPConstant
    {
        public const string Diakin_SOAPURL = "https://e1131-iflmap.hcisbt.ap1.hana.ondemand.com/cxf/createCustomer_UTI_Buyback";
        public const string Diakin_SOAPServiceURL = "https://e1131-iflmap.hcisbt.ap1.hana.ondemand.com/cxf/serviceRequest_UTI_Buyback";
        public const string Diakin_SOAP_GetCustomer = "https://e1131-iflmap.hcisbt.ap1.hana.ondemand.com/cxf/IndividualCustomer_UTI_Buyback";
        public const string Diakin_SOAP_GetOrder = "https://e1131-iflmap.hcisbt.ap1.hana.ondemand.com/cxf/ServiceRequest";
        public const string Diakin_Envelop = @"<soap:Envelope xmlns:soap='http://www.w3.org/2003/05/soap-envelope' xmlns:glob='http://sap.com/xi/SAPGlobal20/Global' xmlns:a1t='http://sap.com/xi/AP/CustomerExtension/BYD/A1TNV' xmlns:ynk='http://0001228287-one-off.sap.com/YNKM31JVY_' xmlns:y1eb='http://0001228287-one-off.sap.com/Y1EBEGRFY_' xmlns:glob1='http://sap.com/xi/AP/Globalization'>"
            + "<soap:Header/>"
            + "<soap:Body>"
            + "[MessageBody]"
            + "</soap:Body>"
            + "</soap:Envelope>";

        public const string Diakin_GetCustomer = "<Request>"
                + "<CustomerMobile>%2B91 [CustMobile]</CustomerMobile>"
                + "</Request>";

        public const string Diakin_Customer = "<glob:CustomerBundleMaintainRequest_sync_V1>"
                + "<BasicMessageHeader/> "
                + "<Customer actionCode='04' addressInformationListCompleteTransmissionIndicator='true' salesArrangementListCompleteTransmissionIndicator='false'>"
                + "<CategoryCode>1</CategoryCode>"
                + "<LifeCycleStatusCode>2</LifeCycleStatusCode>"
                + "<Person>"
                   + "<GivenName>[FirstName]</GivenName>"
                  + "<FamilyName>[LastName]</FamilyName>"
                + "</Person>"
                + "<AddressInformation actionCode = '04' addressUsageListCompleteTransmissionIndicator='true'>"
                   + "<AddressUsage actionCode = '04' >"
                       + "<AddressUsageCode> XXDEFAULT </AddressUsageCode >"
                        + "<DefaultIndicator> true </DefaultIndicator >"
                    + "</AddressUsage >"
                    + "<Address actionCode='04' telephoneListCompleteTransmissionIndicator='true'>"
                        + "<Email>"
                          + "<URI>[Email]</URI>"
                       + "</Email>"
                        + "<PostalAddress>"
                          + "<CountryCode>IN</CountryCode>"
                           + "<RegionCode>[RegionCode]</RegionCode>"
                            + "<CityName>[City]</CityName>"
                            + "<StreetPostalCode>[Pincode]</StreetPostalCode>"
                            + "<HouseID>[City]</HouseID>"
                        + "</PostalAddress>"
                        + "<Telephone>"
                            + "<FormattedNumberDescription>[Mobile]</FormattedNumberDescription>"
                            + "<MobilePhoneNumberIndicator>true</MobilePhoneNumberIndicator>"
                        + "</Telephone>"
                    + "</Address>"
                + "</AddressInformation>"
                + "<Role actionCode = '04'>"
                   + "<RoleCode>ZC4C</RoleCode>"
                + "</Role >"
                + "<ns13:AreaLocality xmlns:ns13= 'http://sap.com/xi/AP/CustomerExtension/BYD/A1TNV'>[AreaLocality]</ns13:AreaLocality>"
            + "</Customer>"
            + "</glob:CustomerBundleMaintainRequest_sync_V1>";

        // New Xml Body for service request
        public const string Diakin_ServiceRequest = "<glob:ServiceRequestBundleMaintainRequest2_sync>"
            + "<BasicMessageHeader />"
            + "<ServiceRequest actionCode='04' businessTransactionDocumentReferenceListCompleteTransmissionIndicator='true' partnerContactPartyListCompleteTransmissionIndicator='true' otherPartyListCompleteTransmissionIndicator='true' solutionProposalListCompleteTransmissionIndicator='true' itemListCompleteTransmissionIndicator='true' textListCompleteTransmissionIndicator='true'>"
            + "<Name>Demo and Installation-Buyback Scheme</Name>"
            + "<BuyerParty contactPartyListCompleteTransmissionIndicator='false'>"
            + "<!--Optional: -->"
                + "<BusinessPartnerInternalID>[CustomerId]</BusinessPartnerInternalID>"
            + "</BuyerParty>"
            + "<ServiceSupportTeamParty>"
                + "<OrganisationalCentreID>[Branch]</OrganisationalCentreID>"
            + "</ServiceSupportTeamParty>"
            + "<MainIncidentServiceIssueCategory>"
            + "<!-- Optional: -->"
                + "<ServiceIssueCategoryID>[SubType]</ServiceIssueCategoryID>"
            + "</MainIncidentServiceIssueCategory>"
            + "<MainServiceReferenceObject>"
                + "<InstalledBaseID></InstalledBaseID>"
            + "</MainServiceReferenceObject>"
            + "<Text actionCode='04'>"
                + "<TypeCode>[TypeCode]</TypeCode>"
                + "<!--Optional:-->"
                + "<Content>Demo and Installation-Buyback Scheme</Content>"
            + "</Text>"
            + "<a1t:WarrantyStatus>[WarrantyStatus]</a1t:WarrantyStatus>"
            + "<y1eb:Prod_Type>[Product_Type]</y1eb:Prod_Type>"
            + " <ProcessorParty><EmployeeID>[EmployeeId]</EmployeeID></ProcessorParty>"
            + "</ServiceRequest>"
            + "</glob:ServiceRequestBundleMaintainRequest2_sync>";


        public const string Diakin_ServiceRequestOld = "<ServiceRequest actionCode='04' businessTransactionDocumentReferenceListCompleteTransmissionIndicator='true' partnerContactPartyListCompleteTransmissionIndicator='true' otherPartyListCompleteTransmissionIndicator='true' solutionProposalListCompleteTransmissionIndicator='true' itemListCompleteTransmissionIndicator='true' textListCompleteTransmissionIndicator='true'>"
           + "<ProcessingTypeCode>[TicketCategory]</ProcessingTypeCode>"
           + "<Name>UTC Digital [SubType] </Name>"
           + "<LifeCycleStatusCode>[Status]</LifeCycleStatusCode>"
           + "<BuyerParty contactPartyListCompleteTransmissionIndicator='false'>"
           + "< !--Optional: -->"
               + "<BusinessPartnerInternalID>[CustomerId]</BusinessPartnerInternalID>"
           + "</BuyerParty>"
           + "<ServiceSupportTeamParty>"
               + "<OrganisationalCentreID>[Branch]</OrganisationalCentreID>"
           + "</ServiceSupportTeamParty>"
           + "<MainIncidentServiceIssueCategory>"
           + "<!-- Optional: -->"
               + "<ServiceIssueCategoryID>[SubType]</ServiceIssueCategoryID>"
           + "</MainIncidentServiceIssueCategory>"
           + "<MainServiceReferenceObject>"
               + "<!-- Optional: -->"
               + "<MaterialID>[MaterialID]</MaterialID>"
               + "<!-- Optional: -->"
               + "<IndividualProductSerialID>[ProductSerialID]</IndividualProductSerialID>"
           + "</MainServiceReferenceObject>"
           + "<ynk:Prod_Type>[Product_Type]</ynk:Prod_Type>"
           + "<a1t:SourceOfCall>[SourceOfCall]</a1t:SourceOfCall>"
           + "</ServiceRequest>";

        //Daikin Get Order Details 
        public const string Diakin_GetOrder = "<Request>"
        + "<ID>[Order]</ID>"
        + "</Request>";
    }
}
