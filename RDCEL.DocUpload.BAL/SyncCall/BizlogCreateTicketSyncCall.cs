using GraspCorn.Common.Enums;
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
    class BizlogCreateTicketSyncCall
    {
        #region Variable Declaration
        BizlogTicketRepository ticketRepository;
        LogisticsRepository logisticsRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();

        #endregion

        #region Add Ticket in database
        /// <summary>
        /// Method to add the Ticket
        /// </summary>       
        /// <returns></returns>   
        public int AddTicketToDB(TicketDataContract ticketDataContract, string BizlogTicketNo)
        {
            ticketRepository = new BizlogTicketRepository();
            int result = 0;
            try
            {
                tblBizlogTicket ticketInfo = SetTicketObjectDBJson(ticketDataContract, BizlogTicketNo);
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
        public tblBizlogTicket SetTicketObjectDBJson(TicketDataContract ticketDataContract, string BizlogTicketNo)
        {
            tblBizlogTicket ticketObj = null;
            try
            {
                if (ticketDataContract != null)
                {
                    ticketObj = new tblBizlogTicket();
                    if (BizlogTicketNo != null)
                    {
                        ticketObj.BizlogTicketNo = BizlogTicketNo;
                    }
                    //add sponsor order id
                    ticketObj.SponsrorOrderNo = ticketDataContract.orderNumber;

                    ticketObj.ConsumerName = ticketDataContract.consumerName;
                    ticketObj.ConsumerComplaintNumber = ticketDataContract.consumerComplaintNumber;
                    ticketObj.AddressLine1 = ticketDataContract.addressLine1;
                    ticketObj.AddressLine2 = ticketDataContract.addressLine2;
                    ticketObj.City = ticketDataContract.city;
                    ticketObj.Pincode = ticketDataContract.pincode;
                    ticketObj.TelephoneNumber = ticketDataContract.telephoneNumber;
                    ticketObj.RetailerPhoneNo = ticketDataContract.retailerPhoneNo;
                    ticketObj.AlternateTelephoneNumber = ticketDataContract.alternateTelephoneNumber;
                    ticketObj.EmailId = ticketDataContract.emailId;                  
                    ticketObj.DateOfPurchase = ticketDataContract.dateOfPurchase;
                    ticketObj.DateOfComplaint = ticketDataContract.dateOfComplaint;
                    ticketObj.NatureOfComplaint = ticketDataContract.natureOfComplaint;
                    ticketObj.IsUnderWarranty = ticketDataContract.isUnderWarranty;
                    ticketObj.Brand = ticketDataContract.brand;
                    ticketObj.ProductCategory = ticketDataContract.productCategory;
                    ticketObj.ProductName = ticketDataContract.productName;
                    ticketObj.ProductCode = ticketDataContract.productCode;
                    ticketObj.Model = ticketDataContract.model;
                    ticketObj.IdentificationNo = ticketDataContract.identificationNo;
                    ticketObj.DropLocation = ticketDataContract.dropLocation;
                    ticketObj.DropLocAddress1 = ticketDataContract.dropLocAddress1;
                    ticketObj.DropLocAddress2 = ticketDataContract.dropLocAddress2;
                    ticketObj.DropLocCity = ticketDataContract.dropLocCity;
                    ticketObj.DropLocState = ticketDataContract.dropLocState;
                    ticketObj.DropLocPincode = ticketDataContract.dropLocPincode;
                    ticketObj.DropLocContactPerson = ticketDataContract.dropLocContactPerson;
                    ticketObj.DropLocContactNo = ticketDataContract.dropLocContactNo;
                    ticketObj.DropLocAlternateNo = ticketDataContract.dropLocAlternateNo;
                    ticketObj.PhysicalEvaluation = ticketDataContract.physicalEvaluation;
                    ticketObj.TechEvalRequired = ticketDataContract.TechEvalRequired;
                    ticketObj.Value = ticketDataContract.value;
                    ticketObj.CreatedDate = currentDatetime;
                    ticketObj.IsActive = true;

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketManager", "SetTicketObjectJson", ex);
            }
            return ticketObj;
        }
        #endregion

        #region Add Updated Ticket in database
        /// <summary>
        /// Method to add the Ticket
        /// </summary>       
        /// <returns></returns>   
        public int AddUpdatedTicketToDB(UpdatedTicketDataContract updatedticketDataContract, string BizlogTicketNo)
        {
            logisticsRepository = new LogisticsRepository();
            ticketRepository = new BizlogTicketRepository();
            int result = 0;
            try
            {
                tblBizlogTicket ticketInfo = SetUpdatedTicketObjectDBJson(updatedticketDataContract, BizlogTicketNo);
                {
                    ticketRepository.Add(ticketInfo);
                    ticketRepository.SaveChanges();
                    result = ticketInfo.Id;
                   
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketManager", "AddUpdatedTicketToDB", ex);
            }

            return result;
        }
        #endregion

        #region set Add Ticket obj
        /// <summary>
        /// Method to set Ticket info to table
        /// </summary>
        /// <param name="ticketDataContract">ticketDataContract</param>     
        public tblBizlogTicket SetUpdatedTicketObjectDBJson(UpdatedTicketDataContract ticketDataContract, string BizlogTicketNo)
        {
            tblBizlogTicket ticketObj = null;
            try
            {
                if (ticketDataContract != null)
                {
                    ticketObj = new tblBizlogTicket();
                    if (BizlogTicketNo != null)
                    {
                        ticketObj.BizlogTicketNo = BizlogTicketNo;
                    }
                    //add sponsor order id
                    ticketObj.SponsrorOrderNo = ticketDataContract.primary.orderNo;
                    if (ticketDataContract.products.Count > 0)
                    {
                        foreach (var item in ticketDataContract.products)
                        {
                            ticketObj.ConsumerName = item.src_add.srcContactPerson;
                            ticketObj.AddressLine1 = item.src_add.srcAdd1;
                            ticketObj.AddressLine2 = item.src_add.srcAdd2;
                            ticketObj.City = item.src_add.srcCity;
                            ticketObj.Pincode = item.src_add.srcPincode;
                            ticketObj.TelephoneNumber = item.src_add.srcContact1;
                            ticketObj.RetailerPhoneNo = item.dst_add.dstContact1;
                            ticketObj.AlternateTelephoneNumber = item.src_add.srcContact2;
                            ticketObj.EmailId = item.src_add.srcEmailId;
                            ticketObj.DateOfComplaint = currentDatetime.ToString();
                            ticketObj.NatureOfComplaint = string.Empty;
                            ticketObj.IsUnderWarranty = item.primary.isUnderWarranty;
                            ticketObj.Brand = item.primary.brandName;
                            ticketObj.ProductCategory = item.primary.productCode;
                            ticketObj.ProductName = item.primary.productName;
                            ticketObj.ProductCode = item.primary.productCode;
                            ticketObj.Model = item.primary.modelName;
                            ticketObj.IdentificationNo = item.primary.identificationNo;
                            ticketObj.DropLocation = item.dst_add.dstLocation;
                            ticketObj.DropLocAddress1 = item.dst_add.dstAdd1;
                            ticketObj.DropLocAddress2 = item.dst_add.dstAdd2;
                            ticketObj.DropLocCity = item.dst_add.dstCity;
                            ticketObj.DropLocState = item.dst_add.dstState;
                            ticketObj.DropLocPincode = item.dst_add.dstPincode;
                            ticketObj.DropLocContactPerson = item.dst_add.dstContactPerson;
                            ticketObj.DropLocContactNo = item.dst_add.dstContact1;
                            ticketObj.DropLocAlternateNo = item.dst_add.dstContact2;
                            ticketObj.PhysicalEvaluation = item.primary.isPhysicalEval;
                            ticketObj.TechEvalRequired = item.primary.isTechEval;
                            ticketObj.Value = item.primary.cost;
                            ticketObj.CreatedDate = currentDatetime;
                            ticketObj.IsActive = true;
                            ticketObj.DateOfPurchase = item.primary.dateOfPurchase;
                            ticketObj.TicketPriority = item.primary.ticketPriority;
                        }
                    }
                    ticketObj.ConsumerComplaintNumber = ticketDataContract.primary.conComplaintNo;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketManager", "SetUpdatedTicketObjectDBJson", ex);
            }
            return ticketObj;
        }
        #endregion
    }
}
