using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Constant
{
    public class EmailTemplateConstants
    {
        public static string ExchangeMailBody()
        {
            string htmlString = @"  <p>Dear [FirstName],</p>



    <p>We are in receipt of your exchange order through [StoreName]  store. </p>


    <p>We (Digi2L) are the authorized exchange partners for [StoreName] and we will process the exchange of your old
        appliance.</p>


    <table cellpadding='2' cellspacing='2' border='1' style='width: 100%;'>
        <tr>
            <td>[StoreName] Order Number</td>
            <td>[SponserOrderNo]</td>
        </tr>
        <tr>
            <td>Date & Time</td>
            <td>[DateTime]</td>
        </tr>
    </table>

    <p>The details of your exchange order are given below:</p>

    <table cellpadding='2' cellspacing='2' border='1' style='width: 100%;'>
        <tr>
            <td>Cust Name </td>
            <td>[CustFirstName]</td>
        </tr>
        <tr>
            <td>Cust Mobile</td>
            <td>[CustMobileNo]</td>
        </tr>
        <tr>
            <td>Cust Add 1</td>
            <td>[CustAddress1]</td>
        </tr>
        <tr>
            <td>Cust Add 2</td>
            <td>[CustAddress2]</td>
        </tr>
        <tr>
            <td>Landmark</td>
            <td>[Landmark]</td>
        </tr>
        <tr>
            <td>Cust Pin Code</td>
            <td>[Pincode]</td>
        </tr>
        <tr>
            <td>Cust City</td>
            <td>[City]</td>
        </tr>
    </table>

    <p>Product details:</p>

    <table cellpadding='2' cellspacing='2' border='1' style='width: 100%;'>
        <tr>
            <td>Exch. Prod Group</td>
            <td>[ProductCategory]</td>
        </tr>
        <tr>
            <td>Old Prod Type</td>
            <td>[ProductType]</td>
        </tr>
        <tr>
            <td>Old Brand</td>
            <td>[Brand]</td>
        </tr>
        <tr>
            <td>Size</td>
            <td>[Size]</td>
        </tr>
    </table>

    <p>Process: </p>
    <p>a) Self-Quality Check: Please process the self quality check by clicking this link [SelfQCLink]</p>

    <p>a) Pre-Quality Check: Within the next 24 hours to pre-assess the quality of your old appliance. This will take
        about 10 minutes and will be done with your prior appointment. </p>


    <p>c) Pick-up: If the old appliance condition and details are as stated at the time of order and passes the
        pre-quality checks, it will be picked within 24-72 hours post the installation of your new product. </p>


    <p>d) Payment: Payment will be done at the time of pickup </p>

    <p>For any clarifications, please write to <a href='mailto:exchange@digimart.co.in'>exchange@digimart.co.in</a> and
        quote the reference number  [RegdNo] .</p>



    <p>Regards <br>
        Team DIGI2L
    </p>";
            return htmlString;
        }


        public static string ABBMailBody()
        {
            string htmlstring = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Document</title>
    <style>
        body {
            font-size: 15px;
            line-height: 1.4rem;
            font-family: Arial, Helvetica, sans-serif;
        }
        #wrapper {
            width: 100%;
            max-width: 620px;
            min-width: 620px;
            margin: 0 auto;
            padding: 10px 20px;
            border: 3px solid #f05a29;
            border-radius: 16px;
        }
        
        #wrapper table {
        }

        #wrapper table td {
            font-size: 13px;
            padding: 8px 20px;
            border: 1px solid #ccc;
        }

        #wrapper table td:first-child {
            background: transparent linear-gradient(109deg, #AB01FC 0%, #3325B0 100%) 0% 0% no-repeat padding-box;
            color: #fff;
            border: 1px solid #fff;
        }
        #wrapper table td:last-child {

        }
        #wrapper .text-center {
            text-align: center;
        }

        #wrapper a {
            text-decoration: none;
            color: #000;
            font-size: 13px;
            font-weight: bold;
        }

        #wrapper hr {
            border-top: 1px dashed #ccc;
        }
    </style>
</head>
<body>

    <div id='wrapper'>
        <img src='https://digi2l.in/wp-content/uploads/2023/01/top-01.png' style='width: 100%;' alt=''>
        <p>Hello Customer,</p>
        <p>Thank you for choosing Digi2L's  Asssured Buyback Plan for your new [BrandName] home appliance. Below are all the details you will need to Avail of your new appliance's Assured Buyback value or raise any queries or concerns about your plan.</p>
        <table style='border-collapse: collapse; width: 100%;'>
            <tr>
                <td>Registration No</td>
                <td>[RegdNo]</td>
            </tr>
            <tr>
                <td>Store Name</td>
                <td>[StoreName]</td>
            </tr>
            <tr>
                <td>Customer Name</td>
                <td>[firstname]</td>
            </tr>
            <tr>
                <td>
                    Customer E-mail
                </td>
                <td>[Email]</td>
            </tr>
            <tr>
                <td>
                    Address Line 1
                </td>
                <td>
                    [Address1] 
                </td>
            </tr>
            <tr>
                <td>Address Line 2</td>
                <td>[Address2]</td>
            </tr>
            <tr>
                <td>Pin Code</td>
                <td>[pincode]</td>
            </tr>
            <tr>
                <td>City</td>
                <td>[city]</td>
            </tr>
            <tr>
                <td>
                    Product Group
                </td>
                <td>
                    [productcategory] 
                </td>
            </tr>
            <tr>
                <td>Product Type</td>
                <td>
                    [ptoductType] 
                </td>
            </tr>
            <tr>
                <td>
                    Product Serial Number
                </td>
                <td>
                    [product serial no]
                </td>
            </tr>
            <tr>
                <td>Model No</td>
                <td>[Modelname]</td>
            </tr>
            <tr>
                <td>Invoice Date</td>
                <td>
                    [invoicedate]
                </td>
            </tr>
            <tr>
                <td>Invoice Number</td>
                <td>[invoiceNumber]</td>
            </tr>
            <tr>
                <td>Net Product Price (inclusive of GST)</td>
                <td>₹[Netvalue]</td>
            </tr>
            <tr>
                <td>ABB Plan Period (Months)</td>
                <td>[PlanPeriod]</td>
            </tr>
            <tr>
                <td>No Claim Period (Months)</td>
                <td>[NoClaimPEriod]</td>
            </tr>
            <tr>
                <td>ABB Plan Name</td>
                <td>[ABB PlanName]</td>
            </tr>
            <tr>
                <td>Upload Date</td>
                <td>
                    [upload date]
                </td>
            </tr>
        </table>
        <p>Please Note: In Case you find any of the above details to be incorrect, kindly request an update by getting in touch with us within 7 days from today send an email to us at: <a href='mailto:Customercare@Digi2L.in'>Customercare@Digi2L.in</a> or call us at: <a href='tel:919619697745'>+91 9619697745</a></p>
        <p>We Have attached the detailed Terms & Conditions of your plan with this email and will be happy to answer any questions you may have on the same</p>
        <hr>
        <p class='text-center'>www.digi2l.in <br>Terms & Conditions: <a href='#'>Digi2L Guaranteed Buyback Agreement : Digi2L</a></p
        
    </div>
</body>
</html>";
            return htmlstring;
        }


        public static string EmailForInstant()
        {
            string htmlString = @"  <p>Dear [FirstName],</p>



    <p>We are in receipt of your exchange order through [StoreName]  store. </p>


    <p>We (Digi2L) are the authorized exchange partners for [StoreName] and we will process the exchange of your old
        appliance.</p>


    <table cellpadding='2' cellspacing='2' border='1' style='width: 100%;'>
        <tr>
            <td>[StoreName] Order Number</td>
            <td>[SponserOrderNo]</td>
        </tr>
        <tr>
            <td>Date & Time</td>
            <td>[DateTime]</td>
        </tr>
    </table>

    <p>The details of your exchange order are given below:</p>

    <table cellpadding='2' cellspacing='2' border='1' style='width: 100%;'>
        <tr>
            <td>Cust Name </td>
            <td>[CustFirstName]</td>
        </tr>
        <tr>
            <td>Cust Mobile</td>
            <td>[CustMobileNo]</td>
        </tr>
        <tr>
            <td>Cust Add 1</td>
            <td>[CustAddress1]</td>
        </tr>
        <tr>
            <td>Cust Add 2</td>
            <td>[CustAddress2]</td>
        </tr>
        <tr>
            <td>Landmark</td>
            <td>[Landmark]</td>
        </tr>
        <tr>
            <td>Cust Pin Code</td>
            <td>[Pincode]</td>
        </tr>
        <tr>
            <td>Cust City</td>
            <td>[City]</td>
        </tr>
    </table>

    <p>Product details:</p>

    <table cellpadding='2' cellspacing='2' border='1' style='width: 100%;'>
        <tr>
            <td>Exch. Prod Group</td>
            <td>[ProductCategory]</td>
        </tr>
        <tr>
            <td>Old Prod Type</td>
            <td>[ProductType]</td>
        </tr>
        <tr>
            <td>Old Brand</td>
            <td>[Brand]</td>
        </tr>
        <tr>
            <td>Size</td>
            <td>[Size]</td>
        </tr>
    </table>

    <p>Process: </p>

    <p>a) Self-Quality Check: Please process the self quality check by clicking this link [SelfQCLink]</p>

    <p>b) Pre-Quality Check: Within the next 24 hours to pre-assess the quality of your old appliance. This will take
        about 10 minutes and will be done with your prior appointment. </p>


    <p>c) Pick-up: If the old appliance condition and details are as stated at the time of order and passes the
        pre-quality checks, it will be picked within 24-72 hours post the installation of your new product. </p>


    <p>d) Payment: Any difference amount shall be collected by UTC digital Service Engineer at the time of quality check.  </p>

    <p>For any clarifications, please write to <a href='mailto:exchange@digimart.co.in'>exchange@digimart.co.in</a> and
        quote the reference number  [RegdNo] .</p>



    <p>Regards <br>
        Team DIGI2L
    </p>";
            return htmlString;
        }



        public static string Smart_Sell_Email()
        {
            string htmlString = @"<!DOCTYPE html>
<html>
<meta http-equiv='Content-Type' content='text/html charset=UTF-8'>

<head>
    <style type='text/css'>
        table {
            border-collapse: separate;
        }

        a,
        a:link,
        a:visited {
            text-decoration: none;
            color: #00788a;
        }

        a:hover {
            text-decoration: underline;
        }

        h2,
        h2 a,
        h2 a:visited,
        h3,
        h3 a,
        h3 a:visited,
        h4,
        h5,
        h6,
        .t_cht {
            color: #000 !important;
        }

        .ExternalClass p,
        .ExternalClass span,
        .ExternalClass font,
        .ExternalClass td {
            line-height: 100%;
        }

        .ExternalClass {
            width: 100%;
        }
    </style>
</head>

<body
    style='background-color:azure; font-family: Arial, Helvetica, sans-serif; font-size: 14px; line-height: 22px; mso-line-height-rule:exactly; padding: 40px 0;'>




   <span style='padding: 40px 0'>
        <span
            style='width: 660px; margin: 0 auto;background-color: #fff;box-sizing: border-box;border-top: 7px solid #f26d42;display: block;'>
            <span>
                <span style='height:0; max-height:0;'>
    
                    <img src='http://digi2l.in/wp-content/uploads/2023/02/banner-01.png'
                        style='width: 100%; object-fit: contain;'>
    
                </span>
                <span style='display:block; padding: 0 40px 10px;'>
                    <p><b>Greetings from Digi2l!!</b></p>
                    <p>Hello [customerName],</p>
                    <p>We have received your sales order on our platform. Our team will get in touch with you at your chosen
                        date and time.</p>
                    <p>Your available time slot:</p>
                    <p><b>[QCDate]</b></p>
                    <p><b>Time: [TimeSlot]</b></p>
                    <p>Thank you for choosing our Smart Sell service for your [ProductCategory]. Please provide the prerequisite
                        for
                        the video quality check by clicking the link below.</p>
                    <span style='font-size: 12px;display: block;'>
                        <span class='app-category' style='text-align: center;margin: 20px 0;width: 100%;display: block;'>
                            <a href='[SelfQCUrl]' target='_blank'
                                style='border-radius: 3px;background-color: #0D004C;color: #fff;display: inline-block;padding: 12px 20px;margin: 5px 10px;text-decoration: none;border-bottom: 4px solid #4f0495;font-weight: normal;'>[productCatDescription]</a>
                        </span>
                    </span>
                    <p>For clarifications/query call us on <a href='tel:9619697745'
                            style='color: #45079c;text-decoration: none;font-weight: bold;'>+91 9619697745</a> or reach us at <a
                            href='mailto:customercare@digi2l.in'
                            style='color: #45079c;text-decoration: none;font-weight: bold;'>customercare@digi2l.in</a>.</p>
        
                    <p>TEAM DIGI2L</p>
                </span>
    
            </span>
    
        </span>
   </span>
    <span
        style='background-color: #0D004C;color: #fff;padding: 18px 12px;text-align: center;width: 660px;margin: 0 auto;box-sizing: border-box;border-bottom: 6px solid #7135d8;display: block;'>
        India's 1<sup style='font-size: 11px;'>st</sup> Platform To Sell Used Appliance
        <p
            style='font-size: 12px; padding: 8px 0 0 0; border-top: 1px solid #676767; font-weight:lighter;letter-spacing: 1px; opacity: 0.6;'>
            For more information visit our website: <a style='color: #fff;text-decoration: none;font-weight: bold;'
                href='https://digi2l.in/' target='_blank'>https://digi2l.in/</a></p>
        <span class='socialmediabtns' style='margin: 0;display: block;'>
            <a href='https://www.facebook.com/digi2l1/' target='_blank'
                style='width: 24px;display: inline-block;margin: 0px 10px 0 10px;padding: 4px;color: #45079c;text-decoration: none;font-weight: normal;border: 1px solid #858585;max-height: 24px;'><img
                    src='https://digi2l.in/wp-content/uploads/2023/01/social-01.png' alt='' style='width: 100%;'></a>
            <a href='https://www.instagram.com/digi2l_/' target='_blank'
                style='width: 24px;display: inline-block;margin: 0px 10px 0 10px;padding: 4px;color: #45079c;text-decoration: none;font-weight: normal;border: 1px solid #858585;max-height: 24px;'><img
                    src='https://digi2l.in/wp-content/uploads/2023/01/social-02.png' alt='' style='width: 100%;'></a>
            <a href='https://twitter.com/digi2l_' target='_blank'
                style='width: 24px;display: inline-block;margin: 0px 10px 0 10px;padding: 4px;color: #45079c;text-decoration: none;font-weight: normal;border: 1px solid #858585;max-height: 24px;'><img
                    src='https://digi2l.in/wp-content/uploads/2023/01/social-03.png' alt='' style='width: 100%;'></a>
            <a href='https://www.linkedin.com/company/digi2l1/' target='_blank'
                style='width: 24px;display: inline-block;margin: 0px 10px 0 10px;padding: 4px;color: #45079c;text-decoration: none;font-weight: normal;border: 1px solid #858585;max-height: 24px;'><img
                    src='https://digi2l.in/wp-content/uploads/2023/01/social-04.png' alt='' style='width: 100%;'></a>
        </span>
    </span>

</body>

</html>";
            return htmlString;
        }
    }
}
