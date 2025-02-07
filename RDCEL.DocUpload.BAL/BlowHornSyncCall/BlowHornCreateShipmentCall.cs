using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.BlowHorn;

namespace RDCEL.DocUpload.BAL.BlowHornSyncCall
{
    class BlowHornCreateShipmentCall
    {
        #region Variable Declaration
        BlowHornRepository ticketRepository;

        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();

        #endregion

        #region Add Ticket in database
        /// <summary>
        /// Method to add the Ticket
        /// </summary>       
        /// <returns></returns>   
        public int AddTicketBlowHornToDB(BlowHornDataContract ticketDataContract, string BlowHornTicketNo)
        {
            ticketRepository = new BlowHornRepository();
            int result = 0;
            try
            {
                tblBlowHornTicket ticketInfo = SetBlowHornTicketObjectDBJson(ticketDataContract, BlowHornTicketNo);
                {
                    ticketRepository.Add(ticketInfo);

                    ticketRepository.SaveChanges();
                    result = ticketInfo.Id;
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketManager", "AddTicket", ex);
            }

            return result;
        }
        #endregion

        #region set Add Ticket obj
        /// <summary>
        /// Method to set Ticket info to table
        /// </summary>
        /// <param name="ticketDataContract">ticketDataContract</param>     
        public tblBlowHornTicket SetBlowHornTicketObjectDBJson(BlowHornDataContract ticketDataContract, string BlowHornTicketNo)
        {
            tblBlowHornTicket ticketObj = null;
            try
            {
                if (ticketDataContract != null)
                {
                    ticketObj = new tblBlowHornTicket();
                    if (BlowHornTicketNo != null)
                    {
                        ticketObj.awb_number = BlowHornTicketNo;
                    }

                    ticketObj.customer_name = ticketDataContract.customer_name;
                    ticketObj.customer_mobile = ticketDataContract.customer_mobile;
                    ticketObj.customer_email = ticketDataContract.customer_email;
                    ticketObj.customer_reference_number = ticketDataContract.customer_reference_number;
                    ticketObj.alternate_customer_mobile = ticketDataContract.alternate_customer_mobile;
                    ticketObj.cash_on_delivery = ticketDataContract.cash_on_delivery;
                    ticketObj.commercial_class = ticketDataContract.commercial_class;
                    ticketObj.delivery_address = ticketDataContract.delivery_address;
                    ticketObj.delivery_hub = ticketDataContract.delivery_hub;
                    ticketObj.delivery_lat = ticketDataContract.delivery_lat;
                    ticketObj.delivery_lon = ticketDataContract.delivery_lon;
                    ticketObj.delivery_postal_code = ticketDataContract.delivery_postal_code;
                    ticketObj.division = ticketDataContract.division;
                    ticketObj.expected_delivery_time = ticketDataContract.expected_delivery_time;
                    ticketObj.is_cod = ticketDataContract.is_cod;
                    ticketObj.is_commercial_address = ticketDataContract.is_commercial_address.ToString();
                    ticketObj.is_hyperlocal = ticketDataContract.is_hyperlocal;
                    ticketObj.is_return_order = ticketDataContract.is_return_order.ToString();
                    ticketObj.pickup_address = ticketDataContract.pickup_address;
                    ticketObj.pickup_customer_mobile = ticketDataContract.pickup_customer_mobile;
                    ticketObj.pickup_customer_name = ticketDataContract.pickup_customer_name;
                    ticketObj.pickup_datetime = ticketDataContract.pickup_datetime;
                    ticketObj.pickup_hub = ticketDataContract.pickup_hub;
                    ticketObj.pickup_lat = ticketDataContract.pickup_lat;
                    ticketObj.pickup_lon = ticketDataContract.pickup_lon;
                    ticketObj.pickup_postal_code = ticketDataContract.pickup_postal_code;
                    ticketObj.pin_number = ticketDataContract.pin_number;
                    ticketObj.reference_number = ticketDataContract.reference_number;
                    ticketObj.what3words = ticketDataContract.what3words;

                    //add sponsor order id

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketManager", "SetTicketObjectJson", ex);
            }
            return ticketObj;
        }
        #endregion
    }
}
