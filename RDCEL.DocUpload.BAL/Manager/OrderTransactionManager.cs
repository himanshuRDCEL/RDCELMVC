using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ExchangeOrderDetails;

namespace RDCEL.DocUpload.BAL.Manager
{
    public class OrderTransactionManager
    {
        OrderTransactionRepository _orderTransactionRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();

        /// <summary>
        /// Method to add data into OrderTran
        /// </summary>
        /// <param name="orderTransactionDC">orderTransactionDC</param>
        /// <returns>int</returns>
        public int MangeOrderTransaction(OrderTransactionDataContract orderTransactionDC)
        {
            int orderid = 0;
            _orderTransactionRepository = new OrderTransactionRepository();
            try
            {
                if(orderTransactionDC != null)
                {
                    tblOrderTran orderTran = GenericMapper<OrderTransactionDataContract, tblOrderTran>.MapObject(orderTransactionDC);
                    if(orderTran.OrderTransId > 0)
                    {
                        //code to update
                        orderTran.ModifiedBy = Convert.ToInt32(UserEnum.Admin);
                        orderTran.StatusId = Convert.ToInt32(StatusEnum.OrderCreated);
                        orderTran.ModifiedDate = _currentDatetime;
                        _orderTransactionRepository.Update(orderTran);
                    }
                    else
                    {
                        //code to create
                        orderTran.IsActive = true;
                        orderTran.CreatedDate = _currentDatetime;
                        orderTran.StatusId = Convert.ToInt32(StatusEnum.OrderCreated);
                        orderTran.CreatedBy = Convert.ToInt32(UserEnum.Admin);
                        orderTran.StatusId = Convert.ToInt32(StatusEnum.OrderCreated);
                        orderTran.AmountPaidToCustomer = false;
                        _orderTransactionRepository.Add(orderTran);
                    }
                    _orderTransactionRepository.SaveChanges();
                    orderid = orderTran.OrderTransId;
                }

            }
            catch (Exception ex )
            {
                LibLogging.WriteErrorToDB("OrderTransactionManager", "MangeOrderTransaction", ex);
            }
            return orderid;
        }

    }
}
