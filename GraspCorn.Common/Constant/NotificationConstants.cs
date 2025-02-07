using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Constant
{
    public class NotificationConstants
    {

        public const string SMS_ABB_OTP = "Dear Customer - OTP for registration for the UTC Assured Buyback Program is [OTP] by TUTC";

        public const string SMS_Exchange_OTP = "Dear Customer - OTP for registration for the UTC Exchange Program is [OTP] by UTC Digital Technologies.";
        public const string SMS_Exchange_DeliveryConfirm = "Dear Customer, Is your new product delivered? Can we schedule the pick-up of your Old Appliance? Please click on the link below to confirm [Link]. By UTC Digital Technologies.";
        public const string SMS_Exchange_Order_Confirm = "Dear Customer, We are in receipt of your Exchange order through Reliance Digital / My Jio store with Ref No.[REGDNO] and shall contact you soon. By UTC Digital Technologies.";

        public const string SMS_Exchange_Order_Confirmation_All = "Dear Customer, We are in receipt of your Exchange order through [STORENAME] store with Ref No.[REGDNO] and shall contact you soon. by UTC Digital Technologies.";

        public const string SMS_Order_Recive_Confirmation_MyGate = "Dear Customer - Congratulations on your Smart Sell with Digi2L. You are one step closer to getting the best price for your preowned appliance. Order [RegNO] has been placed with Digi2L. Our Quality Check expert will be knocking at your door on [Date] at [Time]. By UTC Digital Technologies.";
        public const string SMS_FeedBack = "Dear Customer - Thanks for choosing Digi2L. Please click the link [FBLink] to share your experience. By UTC Digital Technologies.";





        public const string SMS_Exchange_OTP_ALL = "Dear Customer - OTP for [STORENAME] Exchange Program registration is [OTP] by UTC Digital Technologies.";
        public const string SMS_VoucherGeneration_OTP = "Dear Customer - OTP for voucher generation for the [STORENAME] is  [OTP] by Team DIGI2L";
        public const string SMS_VoucherVerification_OTP = "Dear Customer - OTP for [STORENAME] voucher verification is [OTP] by Team DIGI2L.";
        public const string SMS_VoucherRedemption_Confirmation = "Dear Customer - Congratulations!!! Your order has been validated and the Voucher code worth Rs. [ExchPrice]/- for [STORENAME] is [VCODE], Please share this with your dealer at the time of purchase of a [COMPANY] product. This code is valid for [VALIDTILLDATE].you can also download the same from [VLink]. From UTC Digital Technologies.";
        public const string SMS_Order_Recive_Confirmation = "Dear Customer - We are in receipt of your Exchange order through [STORENAME] store with Ref No.[REGNO] and shall contact you soon. By UTC Digital Technologies.";
        public const string SMS_PineLabs_CustomerDetails = "Dear Customer - We are in receipt of your Exchange order through Pine Labs with Ref No. [REGNO]. Kindly fill your personal details by clicking on this link [PLink]. By UTC Digital Technologies.";

        public const string SMS_PineLabs_CustomerDetailsNew = "Dear Customer - We are in receipt of your Exchange order through Pine Labs with Ref No. [REGNO]. Kindly fill your personal details by clicking on this link [PLink]. Thank You, Team DIGI2L.";
        public const string Deffered_Settelment = "Dear Customer - Your exchange has been registered with Ref No. [REGNO], and payment will be made to you by UTC at the time of pickup. By UTC Digital Technologies.";
        public const string SMS_PineLabs_CustomerDetailsNew1 = "Dear Customer - We are in receipt of your Exchange order through Pine Labs with Ref No. [REGNO]. Kindly fill your personal details by clicking on this link [PLink]. Thank You, Team DIGI2L.";

        public const string SMS_VOUCHERCODE = "Dear Customer - OTP for voucher generation for the UTC Exchange Program is {#var#} by UTC Digital Technologies.";
        public const string SMS_ExchangeOtp = "Dear Customer - OTP for [BrandName] Exchange Program registration is [OTP] by Team DIGI2L.";
        public const string WTSUP_ABB_OTP = "Dear Customer - Is your new product delivered? Can we schedule the pick up of your Old Appliance? Please click on the link; [Link] below to confirm.";
        public const string SMS_VOUCHER_GENERATION = "Dear Customer - Congratulations!!! Your order has been validated and the Voucher code worth Rs. [ExchPrice]/- for UTC Exchange Program is [VCode], you can also download the same from [VLink]. From UTC Digital Technologies.";
        public const string SMS_VOUCHER_GENERATIONCash = "Dear Customer - Congratulations!!! Your order has been validated and the Voucher code worth Rs. [ExchPrice]/- for [STORENAME] is [VCODE], Please share this with Digi2L Partner at the time of pick up of the old appliance. The value of this voucher is subject to QC. You can also download the same from [VLink].*T&C applied From UTC Digital Technologies.";



        //Whatsapp Template Names
        public const string Test_Template = "customer_registration";
        public const string Send_Voucher_Code_Template = "instant_voucher_";
        public const string Send_sms_for_deffred = "sms_selfqc_deffered";
        public const string send_sms_order_alliance = "order_confirmation_smart_sell_self_qc";
        public const string send_order_recived_confirmation = "sms_order_recive_confirmation_new_self_qc";
        public const string send_link_for_personal_details = "exchange_pinelabs_voucher_code";
        public const string send_sms_feedback = "sms_feedback";
        public const string voucher_capture_working = "voucher_capture";
        public const string abb_order_confirmation = "abb_registration_181023";
        public const string abb_order_confirmation_old = "abb_order_confirmation_old_copy_copy";
        public const string cash_voucher = "cash_voucher_";
        public const string smartSell = "sms_smart_sell_new_updated";
        public const string orderConfirmationForExchange = "order_confirmation_updated_for_exchange";
        public const string orderConfirmationForExchangeUpdated = "self_qc_generic_template_new";
        public const string PineLabsCustomerDetails = "pinelabs_abb_28jul";
        public const string ABBRedemptionSelfQC = "self_qc";
       
    }
}


