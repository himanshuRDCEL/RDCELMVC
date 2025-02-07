using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.Bizlog;

namespace RDCEL.DocUpload.BAL.SyncCall
{
    public class BizlogTicketStatusSyncCall
    {
        #region Variable Declaration      
        BizlogTicketStatusRepository ticketStatusRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();

        #endregion

        #region add or update Ticket status in database
        /// <summary>
        /// Method to change Ticket status
        /// </summary>       
        /// <returns>string</returns>   
        public string TicketStatusInfoToDB(TicketStatusDataContract ticketStatusDataContract)
        {
            ticketStatusRepository = new BizlogTicketStatusRepository();
            string result = null;
            try
            {
                tblBizlogTicketStatu ticketStatusInfo = SetTicketStatusObjectDBJson(ticketStatusDataContract);
                if (ticketStatusInfo != null)
                {
                    if (ticketStatusInfo.BizlogTicketNo != null)
                    {
                        tblBizlogTicketStatu tempTicketStatusInfo = ticketStatusRepository.GetSingle(x => x.BizlogTicketNo.ToLower().Equals(ticketStatusInfo.BizlogTicketNo.ToLower()));

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
                        result = ticketStatusInfo.BizlogTicketNo;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BizlogTicketStatusSyncCall", "TicketStatusInfoToDB", ex);
            }

            return result;
        }
        #endregion

        #region set Ticket status obj
        /// <summary>
        /// Method to set Ticket status info to table
        /// </summary>
        /// <param name=""></param>     
        public tblBizlogTicketStatu SetTicketStatusObjectDBJson(TicketStatusDataContract ticketStatusDataContract)
        {
            tblBizlogTicketStatu ticketStatusObj = null;
            try
            {
                if (ticketStatusDataContract != null)
                {
                    ticketStatusObj = new tblBizlogTicketStatu();

                    ticketStatusObj.BizlogTicketNo = ticketStatusDataContract.ticketNo;
                    ticketStatusObj.Status = ticketStatusDataContract.status;
                    ticketStatusObj.Remarks = ticketStatusDataContract.remarks;
                    ticketStatusObj.IsActive = true;
                    ticketStatusObj.CreatedDate = currentDatetime;

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BizlogTicketStatusSyncCall", "SetTicketStatusObjectDBJson", ex);
            }
            return ticketStatusObj;
        }
        #endregion

        #region add or update Ticket status in database
        /// <summary>
        /// Method to change Ticket status
        /// </summary>       
        /// <returns>string</returns>   
        public string TicketStatusMahindralogisticsToDB(TicketStatusDataContract ticketStatusDataContract)
        {
            ticketStatusRepository = new BizlogTicketStatusRepository();
            string result = null;
            try
            {
                tblBizlogTicketStatu ticketStatusInfo = SetTicketStatusObjectDBForMahindraLogisticsJson(ticketStatusDataContract);
                if (ticketStatusInfo != null)
                {
                    if (ticketStatusInfo.BizlogTicketNo != null)
                    {
                        tblBizlogTicketStatu tempTicketStatusInfo = ticketStatusRepository.GetSingle(x => x.BizlogTicketNo.ToLower().Equals(ticketStatusInfo.BizlogTicketNo.ToLower()));

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
                        result = ticketStatusInfo.BizlogTicketNo;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BizlogTicketStatusSyncCall", "TicketStatusMahindralogisticsToDB", ex);
            }

            return result;
        }
        #endregion

        #region set Ticket status obj
        /// <summary>
        /// Method to set Ticket status info to table
        /// </summary>
        /// <param name=""></param>     
        public tblBizlogTicketStatu SetTicketStatusObjectDBForMahindraLogisticsJson(TicketStatusDataContract ticketStatusDataContract)
        {
            tblBizlogTicketStatu ticketStatusObj = null;
            try
            {
                if (ticketStatusDataContract != null)
                {
                    ticketStatusObj = new tblBizlogTicketStatu();

                    ticketStatusObj.BizlogTicketNo = ticketStatusDataContract.ticketNo;
                    ticketStatusObj.Status = ticketStatusDataContract.status;
                    ticketStatusObj.Remarks = ticketStatusDataContract.remarks;
                    ticketStatusObj.IsActive = true;
                    ticketStatusObj.CreatedDate = currentDatetime;
                    ticketStatusObj.awbNo = ticketStatusDataContract.ticketNo;

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BizlogTicketStatusSyncCall", "SetTicketStatusObjectDBForMahindraLogisticsJson", ex);
            }
            return ticketStatusObj;
        }
        #endregion
    }
}
