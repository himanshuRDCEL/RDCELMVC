using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using System;
using System.Configuration;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using Mailjet.Client.Resources;
using RDCEL.DocUpload.BAL.SponsorsApiCall;

namespace RDCEL.DocUpload.BAL
{
    public class ERPManager
    {
        #region Payment update in EVC
        public string EVCPaymentstatusUpdate(PaymentResponseModel paymentReponse, int? UserId)
        {
            EVCRegistrationRepository _EVCRegistrationRepository = new EVCRegistrationRepository();
            EVCWalletAdditionRepository _EVCWalletAdditionRepository = new EVCWalletAdditionRepository();
            EVCWalletHistoryRepository _EVCWalletHistoryRepository = new EVCWalletHistoryRepository();
            ABBPaymentRepository aBBPaymentRepository = new ABBPaymentRepository();

            tblEVCRegistration TblEvcRegistration = new tblEVCRegistration();
            tblEVCWalletAddition tblEVCWalletAddition = new tblEVCWalletAddition();
            TblEVCWalletHistory tblEVCWalletHistory = new TblEVCWalletHistory();
            tblPaymentLeaser planPaymentObj = new tblPaymentLeaser();
            string response=null;
            try
            {
                if (paymentReponse != null)
                {
                    tblEVCRegistration tblEVCRegistration = _EVCRegistrationRepository.GetSingle(x => x.EVCRegdNo == paymentReponse.RegdNo);
                    if (tblEVCRegistration != null)
                    {
                        if (paymentReponse.responseCode == Convert.ToInt32(ZaakPayPaymentStatus.successfull))
                        {

                            #region table tblEVCWalletAddition and tblEVCWalletHistory Insert and Update TblEvcRegistration 
                            tblEVCWalletAddition.EVCRegistrationId = tblEVCRegistration.EVCRegistrationId;
                            tblEVCWalletAddition.Amount = paymentReponse.amount;
                            tblEVCWalletAddition.TransactionId = paymentReponse.transactionId;
                            tblEVCWalletAddition.IsActive = true;
                            tblEVCWalletAddition.CreatedDate = DateTime.Now; ;
                            tblEVCWalletAddition.CreatedBy = UserId;
                            _EVCWalletAdditionRepository.Add(tblEVCWalletAddition);
                            _EVCWalletAdditionRepository.SaveChanges();


                            tblEVCWalletHistory.EVCRegistrationId = tblEVCRegistration.EVCRegistrationId;
                            var cAmount = _EVCRegistrationRepository.GetSingle(X => X.EVCRegistrationId == tblEVCWalletHistory.EVCRegistrationId);
                            tblEVCWalletHistory.CurrentWalletAmount = cAmount.EVCWalletAmount > 0 || cAmount.EVCWalletAmount != null ? cAmount.EVCWalletAmount : 0;
                            tblEVCWalletHistory.AddAmount = tblEVCWalletAddition.Amount;
                            tblEVCWalletHistory.BalanceWalletAmount = tblEVCWalletHistory.CurrentWalletAmount + tblEVCWalletHistory.AddAmount;
                            tblEVCWalletHistory.AmountAdditionFlag = true;
                            tblEVCWalletHistory.IsActive = true;
                            tblEVCWalletHistory.TransactionId = paymentReponse.transactionId;
                            tblEVCWalletHistory.CreatedDate = DateTime.Now; ;
                            tblEVCWalletHistory.CreatedBy = UserId;
                            _EVCWalletHistoryRepository.Add(tblEVCWalletHistory);
                            _EVCWalletHistoryRepository.SaveChanges();

                            tblEVCRegistration.EVCWalletAmount = tblEVCWalletHistory.BalanceWalletAmount;
                            tblEVCRegistration.ModifiedBy = UserId;
                            tblEVCRegistration.ModifiedDate = DateTime.Now;
                            _EVCRegistrationRepository.Update(tblEVCRegistration);
                            _EVCRegistrationRepository.SaveChanges();
                            #endregion

                        }
                        planPaymentObj.RegdNo = tblEVCRegistration.EVCRegdNo;
                        planPaymentObj.OrderId = paymentReponse.OrderId;
                        planPaymentObj.PaymentDate = DateTime.Now;
                        planPaymentObj.IsActive = true;
                        planPaymentObj.transactionId = paymentReponse.transactionId;
                        planPaymentObj.ResponseDescription = paymentReponse.responseDescription;
                        planPaymentObj.ResponseCode = paymentReponse.responseCode.ToString();
                        planPaymentObj.CardId = paymentReponse.cardId;
                        planPaymentObj.CardHashId = paymentReponse.cardhashId;
                        planPaymentObj.CardScheme = paymentReponse.cardScheme;
                        planPaymentObj.CardToken = paymentReponse.cardToken;
                        planPaymentObj.Bank = paymentReponse.bank;
                        planPaymentObj.BankId = paymentReponse.bankid;
                        planPaymentObj.amount = paymentReponse.amount;
                        planPaymentObj.CheckSum = paymentReponse.checksum;
                        planPaymentObj.PaymentMode = paymentReponse.paymentMode;
                        string transactionType = ExchangeOrderManager.GetEnumDescription(TransactionTypeEnum.Cr);
                        planPaymentObj.TransactionType = transactionType;
                        string moduleType = ExchangeOrderManager.GetEnumDescription(ModuletypeEnum.EVC);
                        planPaymentObj.ModuleType = moduleType;
                        planPaymentObj.ModuleReferenceId = tblEVCRegistration.EVCRegistrationId;
                        planPaymentObj.CreatedBy = UserId;
                        planPaymentObj.CreatedDate = DateTime.Now;


                        if (paymentReponse.responseCode == 100)
                        {
                            planPaymentObj.PaymentStatus = true;
                        }
                        else
                        {
                            planPaymentObj.PaymentStatus = false;
                        }

                        aBBPaymentRepository.Add(planPaymentObj);
                        aBBPaymentRepository.SaveChanges();

                        response = "success";
                    }
                    else
                    {
                        planPaymentObj.RegdNo = paymentReponse.RegdNo;
                        planPaymentObj.OrderId = paymentReponse.OrderId;
                        planPaymentObj.PaymentDate = DateTime.Now;
                        planPaymentObj.IsActive = true;
                        if (paymentReponse.responseCode == 100)
                        {
                            planPaymentObj.PaymentStatus = true;
                        }
                        else
                        {
                            planPaymentObj.PaymentStatus = false;
                        }
                        planPaymentObj.transactionId = paymentReponse.transactionId;
                        planPaymentObj.ResponseDescription = paymentReponse.responseDescription;
                        planPaymentObj.ResponseCode = paymentReponse.responseCode.ToString();
                        planPaymentObj.CardId = paymentReponse.cardId;
                        planPaymentObj.CardHashId = paymentReponse.cardhashId;
                        planPaymentObj.CardScheme = paymentReponse.cardScheme;
                        planPaymentObj.CardToken = paymentReponse.cardToken;
                        planPaymentObj.Bank = paymentReponse.bank;
                        planPaymentObj.BankId = paymentReponse.bankid;
                        planPaymentObj.amount = paymentReponse.amount;
                        planPaymentObj.CheckSum = paymentReponse.checksum;
                        planPaymentObj.PaymentMode = paymentReponse.paymentMode;
                        aBBPaymentRepository.Add(planPaymentObj);
                        aBBPaymentRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ERPManager", "EVCPaymentstatusUpdate", ex);
            }
            return response;
        }
    } 
}
#endregion