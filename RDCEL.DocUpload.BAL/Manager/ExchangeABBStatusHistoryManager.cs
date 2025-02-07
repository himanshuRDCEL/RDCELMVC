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
    public class ExchangeABBStatusHistoryManager
    {

        ExchangeABBStatusHistoryRepository _orderTransactionRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        /// <summary>
        /// Method to add data into OrderTran
        /// </summary>
        /// <param name="orderTransactionDC">orderTransactionDC</param>
        /// <returns>int</returns>
        public void MangeOrderHisotry(ExchangeABBStatusHistoryDataContract exchangeABBStatusHistoryDC, int userid = 3)
        {
            _orderTransactionRepository = new ExchangeABBStatusHistoryRepository();
            try
            {
                if (exchangeABBStatusHistoryDC != null)
                {
                    tblExchangeABBStatusHistory exchangeABBStatusHistory = GenericMapper<ExchangeABBStatusHistoryDataContract, tblExchangeABBStatusHistory>.MapObject(exchangeABBStatusHistoryDC);

                    //code to create
                    exchangeABBStatusHistory.IsActive = true;
                    exchangeABBStatusHistory.CreatedDate = _currentDatetime;
                    exchangeABBStatusHistory.CreatedBy = userid;
                    _orderTransactionRepository.Add(exchangeABBStatusHistory);

                    _orderTransactionRepository.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeABBStatusHistoryManager", "MangeOrderHisotry", ex);
            }
        }
    }
}
