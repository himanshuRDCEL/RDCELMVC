using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.BlowHornApiCall;
using RDCEL.DocUpload.BAL.BlowHornSyncCall;
using RDCEL.DocUpload.DataContract.BlowHorn;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.ProcessAPI
{
   public class BlowHornSyncManager
   {
        #region Variable Declaration
        BlowHornCall _blowhornInformationCall;
        BlowHornCreateShipmentCall _blowHornCreateTicketSyncCall;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();

        #endregion

        #region sync add ticket details
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public BlowHornRreponseDataContract ProcessTicketInfo(BlowHornDataContract blowHorndataContract)
        {
            _blowhornInformationCall = new BlowHornCall();
            _blowHornCreateTicketSyncCall = new BlowHornCreateShipmentCall();
            BlowHornRreponseDataContract blowHornResponseDC = null;

            try
            {

                if (blowHorndataContract != null)
                {
                    //Create bizlog ticket with API call
                    blowHornResponseDC = _blowhornInformationCall.CreateShipMent(blowHorndataContract);
                    //get bizlogticketno as response and save it in local DB
                    //add status column in local DB
                    #region Code to add Ticket in database 
                    if (blowHornResponseDC != null)
                    {
                        if (blowHornResponseDC.message.awb_number != null && blowHornResponseDC.status!=null)
                        {
                            string BlowHornTicketno = blowHornResponseDC.message.awb_number;
                            _blowHornCreateTicketSyncCall.AddTicketBlowHornToDB(blowHorndataContract, BlowHornTicketno);
                        }
                    }

                    #endregion

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "ProcessTicketInfo", ex);
            }

            return blowHornResponseDC;
        }
        #endregion

        //#region sync cancel ticket details
        ///// <summary>
        ///// 
        ///// </summary>       
        ///// <returns></returns>   
        //public TicketCancelResponseDataContract ProcessCancelTicketInfo(string ticketNo)
        //{
        //    int result = 0;
        //    _ticketInformationCall = new TicketInformationCall();
        //    _bizlogCancelTicketSyncCall = new BizlogCancelTicketSyncCall();
        //    TicketCancelResponseDataContract ticketCancelResponceDC = null;

        //    try
        //    {
        //        if (ticketNo != null)
        //        {
        //            //Cancel bizlog ticket with API call
        //            ticketCancelResponceDC = _ticketInformationCall.CancelTicketToBizlog(ticketNo);
        //            //Change ticket status in local DB
        //            #region Code to add Ticket in database 
        //            if (ticketCancelResponceDC != null)
        //            {
        //                if (ticketCancelResponceDC.success == true)
        //                {
        //                    result = _bizlogCancelTicketSyncCall.CancelTicketToDB(ticketNo);
        //                    //string s = result.ToString();
        //                    //LibLogging.WriteErrorToDB(s, "ProcessCancelTicketInfo");
        //                }
        //            }

        //            #endregion

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("TicketSyncManager", "ProcessCancelTicketInfo", ex);
        //    }

        //    return ticketCancelResponceDC;
        //}
        //#endregion

        //#region sync ticket status details
        ///// <summary>
        ///// 
        ///// </summary>       
        ///// <returns></returns>   
        //public string ProcessTicketStatusInfo(TicketStatusDataContract ticketStatusDataContract)
        //{
        //    _ticketInformationCall = new TicketInformationCall();
        //    _bizlogTicketStatusSyncCall = new BizlogTicketStatusSyncCall();
        //    string tickeNo = null;

        //    try
        //    {
        //        if (ticketStatusDataContract != null)
        //        {

        //            //Change ticket status in local DB
        //            #region Code to add Ticket in database 

        //            tickeNo = _bizlogTicketStatusSyncCall.TicketStatusInfoToDB(ticketStatusDataContract);

        //            #endregion

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("TicketSyncManager", "ProcessCancelTicketInfo", ex);
        //    }

        //    return tickeNo;
        //}
        //#endregion

        #region set TicketDataContract object from zoho SponserSponserListDataContract 
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public BlowHornDataContract SetblowHornTicket(SponserData sponserObj, EvcApprovedData evcApprovedObj, EvcAllocationReportData EvcAllocationObj)
        {
            _blowhornInformationCall = new BlowHornCall();
            BlowHornDataContract blowhornTicketObj = null;
            ItemDetail itemDetail = null;

            try
            {
                if (sponserObj != null && evcApprovedObj != null && EvcAllocationObj != null)
                {
                    itemDetail = new ItemDetail();
                    blowhornTicketObj = new BlowHornDataContract();
                    if (sponserObj.Customer_Name != null)
                    {
                        blowhornTicketObj.pickup_customer_name = sponserObj.Customer_Name.first_name + " " + sponserObj.Customer_Name.last_name;  //Mandatory
                    }
                    blowhornTicketObj.pickup_address = sponserObj.Customer_Address_1+" "+sponserObj.Customer_Address_2;          //Mandatory
                    blowhornTicketObj.pickup_hub = sponserObj.Customer_City;                    //Mandatory
                    blowhornTicketObj.pickup_postal_code = sponserObj.Customer_Pincode;                 //Mandatory
                    blowhornTicketObj.pin_number = sponserObj.Customer_Pincode; //Mandatory
                    blowhornTicketObj.pickup_customer_mobile = sponserObj.Customer_Mobile;
                    blowhornTicketObj.customer_mobile = evcApprovedObj.EVC_Mobile_Number;
                    blowhornTicketObj.customer_name = evcApprovedObj.EVC_Name;
                    blowhornTicketObj.customer_email = evcApprovedObj.E_mail_ID;
                    blowhornTicketObj.customer_reference_number = evcApprovedObj.EVC_Regd_No;
                    blowhornTicketObj.cash_on_delivery = "0";
                    blowhornTicketObj.delivery_address = EvcAllocationObj.Address_Line_1+" "+EvcAllocationObj.Address_Line_2;
                    blowhornTicketObj.delivery_hub = EvcAllocationObj.City1;
                    blowhornTicketObj.delivery_postal_code = EvcAllocationObj.EVC_PIN_Code;
                    blowhornTicketObj.division = EvcAllocationObj.City1;
                    blowhornTicketObj.is_cod = false;
                    blowhornTicketObj.is_commercial_address = true;
                    blowhornTicketObj.is_hyperlocal = false.ToString();
                    blowhornTicketObj.what3words = "circling.novelists.wades";
                    blowhornTicketObj.alternate_customer_mobile = sponserObj.Retailer_Phone_Number;
                    blowhornTicketObj.expected_delivery_time = DateTime.Parse(sponserObj.Estimate_Delivery_Date);
                    blowhornTicketObj.pickup_datetime = DateTime.Parse(sponserObj.Expected_Pickup_Date);
                    blowhornTicketObj.is_return_order = false;
                    blowhornTicketObj.reference_number = sponserObj.Regd_No;
                    blowhornTicketObj.customer_reference_number = EvcAllocationObj.Regd_No.ToString();
                    //blowhornTicketObj.SponsrorOrderNo = sponserObj.Sp_Order_No;
                    if (sponserObj.Old_Brand != null)
                    {
                   
                        itemDetail.brand = sponserObj.Old_Brand.ToString();
                        itemDetail.item_name = sponserObj.Order;
                        itemDetail.item_category = sponserObj.Order_Type;
                        itemDetail.total_item_price = Convert.ToInt32(sponserObj.Total_Amt_Paid);
                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "SetTicketObjInfo", ex);
            }

            return blowhornTicketObj;
        }
        #endregion
    }
}
