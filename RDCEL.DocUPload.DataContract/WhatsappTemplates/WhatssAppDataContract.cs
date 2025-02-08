using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.WhatsappTemplates
{

    //Notification For VoucherCode
    public class Notification
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public SendVoucherOnWhatssapp @params { get; set; }
    }
    //for voucher Code
    public class WhatsappTemplate
    {
        public UserDetails userDetails { get; set; }
        public Notification notification { get; set; }
    }

    public class UserDetails
    {
        public string number { get; set; }
        public string name { get; set; }
    }

    public class WhatasappResponse
    {
        public string msgId { get; set; }
        public string message { get; set; }
    }

    //Paremeters to send voucher code on whatssapp yellow.ai
    public class SendVoucherOnWhatssapp
    {
        [JsonProperty("1")]
        public string voucherAmount { get; set; }
        [JsonProperty("2")]
        public string BrandName { get; set; }
        [JsonProperty("3")]
        public string voucherCode { get; set; }
        [JsonProperty("4")]
        public string BrandName2 { get; set; }
        [JsonProperty("5")]
        public string VoucherExpiry { get; set; }
        [JsonProperty("6")]
        public string VoucherLink { get; set; }

    }



    //Deffered Case Template Call
    public class TemplateForDeffredCase
    {
        public UserDetails userDetails { get; set; }
        public Notificationdeffered notification { get; set; }
    }


    //Defffred Notification
    public class Notificationdeffered
    { 
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public SendSmsDefferedWhatsapp @params { get; set; }
    }
    //Paremeters to send sms for deffered settlement on whatssapp yellow.ai
    public class SendSmsDefferedWhatsapp
    {
        [JsonProperty("1")]
        public string RegdNo { get; set; }
        [JsonProperty("2")]
        public string selfQCLink { get; set; }
    }



    //Deffered Case Template Call
    public class TemplateForAllianceAndD2C
    {
        public UserDetails userDetails { get; set; }
        public NotificationForAllianceAndD2C notification { get; set; }
    }


    //Defffred Notification
    public class NotificationForAllianceAndD2C
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public sendsmsAllianceD2Cwhatsapp @params { get; set; }
    }
    //Paremeters to send sms for Alliance and D2C  on whatssapp yellow.ai
    public class sendsmsAllianceD2Cwhatsapp
    {
        [JsonProperty("1")]
        public string RegdNo { get; set; }
        [JsonProperty("2")]
        public string QCDate { get; set; }
        [JsonProperty("3")]
        public string TimeSlot { get; set; }

        [JsonProperty("4")]
        public string SelfQCLink { get; set; }

    }

    //OrderConfirmation  Template Call
    public class OrderConfirmationTemplate
    {
        public UserDetails userDetails { get; set; }
        public OrderConfirmationExchange notification { get; set; }
    }


    //OrderConfirmation Notification
    public class OrderConfirmationExchange
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public OrderRecivedConfirmationWhatsapp @params { get; set; }
    }
    //Paremeters to send sms for OrderConfirmation  on whatssapp yellow.ai
    public class OrderRecivedConfirmationWhatsapp
    {
        [JsonProperty("1")]
        public string BusinessUnitName { get; set; }
        [JsonProperty("2")]
        public string RegdNo { get; set; }

        [JsonProperty("3")]
        public string SelfQcLink { get; set; }
    }


    //send link to fill personal details  Template Call
    public class PersonalDdetailsLinkWhatsappTemplate
    {
        public UserDetails userDetails { get; set; }
        public PersonalDetailsNOtification notification { get; set; }
    }


    //personal Detail Link Notification
    public class PersonalDetailsNOtification
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public PersonalDetailsLinkOnWhatsapp @params { get; set; }
    }

    //Paremeters to send sms for Link to fill personal details  on whatssapp yellow.ai
    public class PersonalDetailsLinkOnWhatsapp
    {
        //[JsonProperty("1")]
        //public string RegdNo { get; set; }
        [JsonProperty("1")]
        public string Link { get; set; }


    }


    //send link to fill personal details  Template Call
    public class FeedBackLinkTempplateWhatsapp
    {
        public UserDetails userDetails { get; set; }
        public FeedbackLinkNotification notification { get; set; }
    }


    //Feedback Link Notification
    public class FeedbackLinkNotification
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public FeedBackLinkWhatsApp @params { get; set; }
    }
    //Paremeters to send sms for FeedBack Link  on whatssapp yellow.ai
    public class FeedBackLinkWhatsApp
    {
        [JsonProperty("1")]
        public string Link { get; set; }

    }

    //send sms for voucher capture
    public class voucherCapture
    {
        public UserDetails userDetails { get; set; }
        public vaucherCaptureNotification notification { get; set; }
    }


    //Capture voucher Notification
    public class vaucherCaptureNotification
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public vouchercaptureProperties @params { get; set; }
    }
    //Paremeters to send sms for vouchercaptureProperties  on whatssapp yellow.ai
    public class vouchercaptureProperties
    {
        [JsonProperty("1")]
        public string customerName { get; set; }
        [JsonProperty("2")]
        public string vouchercode { get; set; }
        [JsonProperty("3")]
        public string companyName { get; set; }
        [JsonProperty("4")]
        public string oldProductcategory { get; set; }
        [JsonProperty("5")]
        public string olsProductCategory { get; set; }
        [JsonProperty("6")]
        public string workingQualities { get; set; }

    }


    //Deffered Case Template Call
    public class ABBOrderConfirmation
    {
        public UserDetails userDetails { get; set; }
        public NotificationForABB notification { get; set; }
    }


    //Defffred Notification
    public class NotificationForABB
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public SendSmsABBWhatsapp @params { get; set; }
    }
    //Paremeters to send sms for deffered settlement on whatssapp yellow.ai
    public class SendSmsABBWhatsapp
    {
        [JsonProperty("1")]
        public string RegdNo { get; set; }

    }


    //Notification For cash VoucherCode
    public class NotificationForCash
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public SendCashVoucherOnWhatssapp @params { get; set; }
    }
    //For cash VoucherCode
    public class WhatsappTemplatecashvoucher
    {
        public UserDetails userDetails { get; set; }
        public NotificationForCash notification { get; set; }
    }

    //Paremeters to send voucher code on whatssapp yellow.ai
    public class SendCashVoucherOnWhatssapp
    {
        [JsonProperty("1")]
        public string voucherAmount { get; set; }
        [JsonProperty("2")]
        public string BrandName { get; set; }
        [JsonProperty("3")]
        public string voucherCode { get; set; }
        [JsonProperty("4")]
        public string VoucherExpiry { get; set; }
        [JsonProperty("5")]
        public string VoucherLink { get; set; }

    }


    //Deffered Case Template Call
    public class TemplateForSmartSellSmartBuy
    {
        public UserDetails userDetails { get; set; }
        public NotificationForSmartSell notification { get; set; }
    }


    //Defffred Notification
    public class NotificationForSmartSell
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public SendWhatsapppSmartSell @params { get; set; }
    }
    //Paremeters to send sms for deffered settlement on whatssapp yellow.ai
    public class SendWhatsapppSmartSell
    {
        [JsonProperty("1")]
        public string CustName { get; set; }
        [JsonProperty("2")]

        public string Link { get; set; }
        [JsonProperty("3")]
        public string RegdNo { get; set; }
        [JsonProperty("4")]
        public string ProductCat { get; set; }
        [JsonProperty("5")]
        public string ProductType { get; set; }

    }


    //Deffered Case Template Call
    public class OrderConfirmationTemplateExchange
    {
        public UserDetails userDetails { get; set; }
        public OrderConfiirmationNotification notification { get; set; }
    }


    //Defffred Notification
    public class OrderConfiirmationNotification
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public SendWhatssappForExcahangeConfirmation @params { get; set; }
    }
    //Paremeters to send sms for deffered settlement on whatssapp yellow.ai
    public class SendWhatssappForExcahangeConfirmation
    {
        [JsonProperty("1")]
        public string CustName { get; set; }
        [JsonProperty("2")]

        public string Link { get; set; }
        [JsonProperty("3")]
        public string ProductBrand { get; set; }
        [JsonProperty("4")]
        public string RegdNO { get; set; }
        [JsonProperty("5")]
        public string ProdCategory { get; set; }
        [JsonProperty("6")]
        public string ProdType { get; set; }

    }


    //Deffered Case Template Call exchange order 
    public class OrderConfirmationTemplateExchangeUpdated
    {
        public UserDetails userDetails { get; set; }
        public OrderConfiirmationNotificationUpdated notification { get; set; }
    }


    //Defffred Notification
    public class OrderConfiirmationNotificationUpdated
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public SendWhatssappForExcahangeConfirmationUpdated @params { get; set; }
    }
    //Paremeters to send sms for deffered settlement on whatssapp yellow.ai
    public class SendWhatssappForExcahangeConfirmationUpdated
    {
        [JsonProperty("1")]
        public string CustName { get; set; }
        [JsonProperty("2")]

        public string RegdNO { get; set; }
        [JsonProperty("3")]
        public string ProdCategory { get; set; }
        [JsonProperty("4")]
        public string ProdType { get; set; }
        [JsonProperty("5")]
        public string CustomerName { get; set; }
        [JsonProperty("6")]
        public string PhoneNumber { get; set; }
        [JsonProperty("7")]
        public string Email { get; set; }

        [JsonProperty("8")]
        public string Address { get; set; }
        [JsonProperty("9")]
        public string Link { get; set; }

    }

    //send link to fill personal details  Template Call
    public class ABBRedemptionSelfQCLink
    {
        public UserDetails userDetails { get; set; }
        public ABBRedemptionSelfQCLinkNotification notification { get; set; }
    }


    //personal Detail Link Notification
    public class ABBRedemptionSelfQCLinkNotification
    {
        public string type { get; set; }
        public string sender { get; set; }
        public string templateId { get; set; }
        public ABBRedemptionParameters @params { get; set; }
    }

    //Paremeters to send sms for Link to fill personal details  on whatssapp yellow.ai
    public class ABBRedemptionParameters
    {
        
        [JsonProperty("2")]
        public string Link { get; set; }
        [JsonProperty("1")]
        public string CustomerName { get; set; }


    }
}
