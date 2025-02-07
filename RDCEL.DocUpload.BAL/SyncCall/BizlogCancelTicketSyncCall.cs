using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;

namespace RDCEL.DocUpload.BAL.SyncCall
{
    public class BizlogCancelTicketSyncCall
    {
        #region Variable Declaration
        BizlogTicketStatusRepository ticketStatusRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();

        #endregion

        #region Cancel Ticket status in database
        /// <summary>
        /// Method to Cancel the Ticket
        /// </summary>       
        /// <returns></returns>   
        public int CancelTicketToDB(string BizlogTicketNo)
        {
            ticketStatusRepository = new BizlogTicketStatusRepository();
            int result = 0;
            try
            {
                tblBizlogTicketStatu ticketStatusInfo = SetCancelTicketObjectDBJson(BizlogTicketNo);
                if (ticketStatusInfo != null)
                {
                    tblBizlogTicketStatu tempTicketStatusInfo = ticketStatusRepository.GetSingle(x => x.BizlogTicketNo.ToLower().Equals(BizlogTicketNo.ToLower()));
                    if (tempTicketStatusInfo != null)
                    {

                        ticketStatusInfo.Id = tempTicketStatusInfo.Id;
                        ticketStatusInfo.BizlogTicketNo = tempTicketStatusInfo.BizlogTicketNo;

                        ticketStatusRepository.Update(ticketStatusInfo);

                    }
                    else
                    {
                        ticketStatusRepository.Add(ticketStatusInfo);
                    }

                    ticketStatusRepository.SaveChanges();
                    result = ticketStatusInfo.Id;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketManager", "CancelTicket", ex);
            }

            return result;
        }
        #endregion

        #region set Cancel Ticket obj
        /// <summary>
        /// Method to set Cancel Ticket status info to table
        /// </summary>
        /// <param name=""></param>     
        public tblBizlogTicketStatu SetCancelTicketObjectDBJson(string BizlogTicketNo)
        {
            tblBizlogTicketStatu ticketStatusObj = null;
            try
            {
                if (BizlogTicketNo != null)
                {
                    ticketStatusObj = new tblBizlogTicketStatu();

                    ticketStatusObj.BizlogTicketNo = BizlogTicketNo;
                    ticketStatusObj.Status = "Cancelled";
                    ticketStatusObj.Remarks = "Ticket Cancelled";
                    ticketStatusObj.IsActive = true;
                    ticketStatusObj.CreatedDate = currentDatetime;

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketManager", "SetCancelTicketObjectJson", ex);
            }
            return ticketStatusObj;
        }
        #endregion

    }
}
