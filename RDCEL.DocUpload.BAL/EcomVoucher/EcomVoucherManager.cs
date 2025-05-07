using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using Mailjet.Client.Resources;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUPload.DataContract.EcomVoucher;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.BAL.EcomVoucher
{
    public class EcomVoucherManager

    {
        EcomVoucherRepository _ecomVoucherRepository;
        DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
        //private CustomDataProtection _protector;
        MailManager _mailManager;
        ConfigurationRepository _configurationRepository;
        EcomPhoneSpecificsRepository _ecomPhoneSpecificsRepository;

        // Mapper _mapper;
        //ILogging _logging;
        /// <summary>
        /// Method to manage (Add/Edit) voucher 
        /// </summary>
        /// <param name="EcomVM">EcomVM</param>
        /// <param name="EcomVMId">EcomVMId</param>
        /// <returns>int</returns>
        public EcomVoucherDataContract ManageEcomVoucher(EcomVoucherDataContract EcomVM)
        {
            tblEcomVoucher TblEcomVoucher = new tblEcomVoucher();
            _ecomVoucherRepository = new EcomVoucherRepository();
            _configurationRepository = new ConfigurationRepository();
            bool flag = false;
            EcomVM.success = false;
            try
            {
                if (EcomVM != null)
                {
                    string key = ConfigurationManager.AppSettings["ERPEncryptionKey"].ToString();

                    EcomVM.Phoneno = EcomVM.Phoneno?.Trim();
                    if (EcomVM.Phoneno != null)
                    {
                        EcomVM.Phoneno = StringSecurity.EncryptString(EcomVM.Phoneno, key);
                        EcomVM.Phoneno = EcomVM.Phoneno?.Trim();

                    }

                    EcomVM.StartDate=_currentDatetime;

                    if (EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.GenericVoucher))
                    {
                        var tblConfiguration = _configurationRepository.GetSingle(x => x.Name == "EcomGeneric" && x.IsActive == true);
                        EcomVM.EndDate = Convert.ToDateTime(EcomVM.StartDate).AddDays(Convert.ToInt32(tblConfiguration.Value));

                    }
                    else if (EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.BrandSpecificVoucher))
                    {
                        var tblConfiguration = _configurationRepository.GetSingle(x => x.Name == "EcomBrandSpecific" && x.IsActive == true);
                        EcomVM.EndDate = Convert.ToDateTime(EcomVM.StartDate).AddDays(Convert.ToInt32(tblConfiguration.Value));
                    }
                    else   if (EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.PhoneSpecificVoucher))
                    {
                        var tblConfiguration = _configurationRepository.GetSingle(x => x.Name == "EcomBrandPhoneSpecific" && x.IsActive == true);
                        EcomVM.EndDate = Convert.ToDateTime(EcomVM.StartDate).AddDays(Convert.ToInt32(tblConfiguration.Value));
                    }

                    

                    if (TblEcomVoucher.EcomVoucherId > 0 && EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.BrandSpecificVoucher))
                    {
                        
                    TblEcomVoucher = GenericMapper<EcomVoucherDataContract,tblEcomVoucher>.MapObject(EcomVM);
                        TblEcomVoucher.ModifiedBy = Convert.ToInt32(UserEnum.Admin);
                        TblEcomVoucher.ModifiedDate = _currentDatetime;
                        _ecomVoucherRepository.Update(TblEcomVoucher);
                    }
                    else
                    {
                        if (EcomVM.EcomPhoneSpecificsListVM != null && EcomVM.EcomPhoneSpecificsListVM.Any() && EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.PhoneSpecificVoucher))
                        {
                            flag = ManagePhoneSpecificVoucher(EcomVM);

                        }
                        else if (EcomVM.VoucherCount != null && EcomVM.VoucherCount > 0 && EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.GenericVoucher))
                        {
                            flag = ManageGenericVoucher(EcomVM);
                        }
                        else if (EcomVM.Phoneno != null && EcomVM.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.BrandSpecificVoucher))
                        {
                            TblEcomVoucher = GenericMapper<EcomVoucherDataContract, tblEcomVoucher>.MapObject(EcomVM);
                            //Code to Insert the object
                            
                            TblEcomVoucher.IsActive = true;
                            TblEcomVoucher.voucherstatus = EcomVoucherStatusEnum.Generated.ToString(); ;
                            TblEcomVoucher.VoucherCode = GenerateVoucher();
                            TblEcomVoucher.CreatedDate = _currentDatetime;
                            TblEcomVoucher.CreatedBy = Convert.ToInt32(UserEnum.Admin);
                            _ecomVoucherRepository.Add(TblEcomVoucher);
                            flag = true;
                        }
                    }
                    _ecomVoucherRepository.SaveChanges();
                    if (flag == true)
                    {
                        EcomVM.success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EcomVoucherManager", "ManageEcomVoucher", ex);
            }

            return EcomVM;
        }

        /// <summary>
        /// Method to get the User by id 
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns>EcomVoucherViewModel</returns>
        public EcomVoucherDataContract GetEcomVoucherById(int id)
        {
            EcomVoucherDataContract EcomVM = null;
            tblEcomVoucher TblEcomVoucher = null;

            try
            {

                TblEcomVoucher = _ecomVoucherRepository.GetSingle(x => x.IsActive == true && x.EcomVoucherId == id);

                if (TblEcomVoucher != null)
                {
                    EcomVM = GenericMapper<tblEcomVoucher, EcomVoucherDataContract>.MapObject(TblEcomVoucher);
                  
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EcomVoucherManager", "GetEcomVoucherById", ex);
            }
            return EcomVM;
        }
        /// <summary>
        /// methd to manage Genenric type voucher
        /// </summary>
        /// <param name="EcomVM"></param>
        /// <returns></returns>
        public bool ManageGenericVoucher(EcomVoucherDataContract EcomVM)
        {
            bool flag = false;
            try
            {
                if (EcomVM != null && EcomVM.VoucherCount != null)
                {
                    for (int i = 0; i < EcomVM.VoucherCount; i++)
                    {
                        tblEcomVoucher voucher = new tblEcomVoucher
                        {

                            IsActive = true,
                            voucherstatus = EcomVoucherStatusEnum.Generated.ToString(),
                            VoucherCode = GenerateVoucher(),
                            CreatedDate = _currentDatetime,
                            StartDate = EcomVM.StartDate,
                            EndDate = EcomVM.EndDate,
                            CompanyId = EcomVM.CompanyId,
                            ValueType = EcomVM.ValueType,
                            FixedValue = EcomVM.FixedValue,
                            Percentage = EcomVM.Percentage,
                            PercLimit = EcomVM.PercLimit,
                           EcomVoucherType = Convert.ToInt32(EcomVoucherTypeEnum.GenericVoucher)
                       
                    };

                        _ecomVoucherRepository.Add(voucher);
                        _ecomVoucherRepository.SaveChanges();
                        flag = true;

                    }

                }
            }
            catch (Exception ex)
            {

            }
            return flag;
        }

        public bool ManagePhoneSpecificVoucher(EcomVoucherDataContract EcomVM)
        {
            string key = ConfigurationManager.AppSettings["ERPEncryptionKey"].ToString();

            bool flag = false;
            int ecomVoucherId = 0;
            _ecomPhoneSpecificsRepository = new EcomPhoneSpecificsRepository(); 
            _ecomVoucherRepository  = new EcomVoucherRepository();
            try
            {
                if (EcomVM != null && EcomVM.EcomPhoneSpecificsListVM != null)
                {
                    tblEcomVoucher voucher = new tblEcomVoucher
                    {

                        IsActive = true,
                        voucherstatus = EcomVoucherStatusEnum.Generated.ToString(),
                        BrandId = EcomVM.BrandId,
                        CategoryIds = EcomVM.CategoryIds,
                        StartDate = EcomVM.StartDate,
                        EndDate = EcomVM.EndDate,
                        ValueType = EcomVM.ValueType,
                        FixedValue = EcomVM.FixedValue,
                        Percentage = EcomVM.Percentage,
                        PercLimit = EcomVM.PercLimit,
                        CreatedDate = _currentDatetime,
                        CompanyId = EcomVM.CompanyId,

                        EcomVoucherType = Convert.ToInt32(EcomVoucherTypeEnum.PhoneSpecificVoucher)
                    };
                    _ecomVoucherRepository.Add(voucher);
                    _ecomVoucherRepository.SaveChanges();
                    ecomVoucherId = voucher.EcomVoucherId;

                }


                if (ecomVoucherId > 0)
                {

                    foreach (var item in EcomVM.EcomPhoneSpecificsListVM)
                    {
                        tblEcomPhoneSpecific pHvoucher = new tblEcomPhoneSpecific
                        {
                            Phoneno = StringSecurity.EncryptString(item.Phoneno, key),
                            Voucherstatus = EcomVoucherStatusEnum.Generated.ToString(),
                            VoucherCode = GenerateVoucher(),
                            EcomVoucherId = ecomVoucherId,
                            IsActive = true,
                            CreatedDate = _currentDatetime,
                            
                        };
                        _ecomPhoneSpecificsRepository.Add(pHvoucher);
                    }
                    _ecomPhoneSpecificsRepository.SaveChanges();
                    flag = true;
                }

            }
            catch (Exception ex)
            {

            }
            return flag;
        }

        /// <summary>
        /// method to get voucher price details
        /// </summary>
        /// <param name="priceDC"></param>
        /// <returns></returns>
        public ResponseEcomVoucherPriceDC GetEcomVoucherPriceDetail(ResquestEcomVoucherPriceDC priceDC , int ? companyid )
        {
            ResponseEcomVoucherPriceDC respVM = new ResponseEcomVoucherPriceDC();
            tblEcomVoucher TblEcomVoucher = null;
            _ecomVoucherRepository = new EcomVoucherRepository();
            _ecomPhoneSpecificsRepository = new EcomPhoneSpecificsRepository();
            bool flag = false;

            try
            {
                if (companyid != 0)
                {
                    // Get master voucher (only for Generic/BrandSpecific etc.)
                    var masterVoucher = _ecomVoucherRepository.GetSingle(x =>
                        x.IsActive == true &&
                        x.VoucherCode == priceDC.VoucherCode &&
                        x.IsUsed != true &&
                        x.CompanyId == companyid);

                    // Try getting child voucher (PhoneSpecific)
                    var childVoucher = _ecomPhoneSpecificsRepository.GetSingle(x =>
                        x.VoucherCode == priceDC.VoucherCode &&
                        x.IsActive == true);

                    // Use appropriate voucher
                    if (childVoucher != null && childVoucher.EcomVoucherId.HasValue)
                    {
                        TblEcomVoucher = _ecomVoucherRepository.GetSingle(x =>
                            x.EcomVoucherId == childVoucher.EcomVoucherId &&
                            x.IsActive == true &&
                            x.IsUsed != true &&
                            x.CompanyId == companyid);
                    }
                    else
                    {
                        TblEcomVoucher = masterVoucher;
                    }

                    if (TblEcomVoucher != null)
                    {
                        if (TblEcomVoucher.EndDate.HasValue && TblEcomVoucher.EndDate.Value < DateTime.Now)
                        {
                            respVM.Message = "Voucher has expired.";
                        }
                        else if (TblEcomVoucher.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.GenericVoucher))
                        {
                            respVM.valueType = EnumHelper.DescriptionAttr((EcomVoucherValueTypeEnum)TblEcomVoucher.ValueType.Value);
                            respVM.FixedValue = TblEcomVoucher.FixedValue;
                            respVM.Percent = TblEcomVoucher.Percentage;
                            respVM.MaxDiscount = TblEcomVoucher.PercLimit;
                            respVM.Message = "Success";
                            flag = true;
                        }
                        else if (TblEcomVoucher.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.BrandSpecificVoucher))
                        {
                            bool isBrandMatched = true;
                            bool isCategoryMatched = true;

                            if (priceDC.BrandId.HasValue && TblEcomVoucher.BrandId != priceDC.BrandId.Value)
                                isBrandMatched = false;

                            if (priceDC.CategoryId.HasValue && !string.IsNullOrEmpty(TblEcomVoucher.CategoryIds))
                            {
                                var categoryList = TblEcomVoucher.CategoryIds.Split(',').Select(int.Parse).ToList();
                                if (!categoryList.Contains(priceDC.CategoryId.Value))
                                    isCategoryMatched = false;
                            }

                            if (!isBrandMatched)
                                respVM.Message = "Voucher is not valid for the selected brand.";
                            else if (!isCategoryMatched)
                                respVM.Message = "Voucher is not valid for the selected category.";
                            else
                            {
                                respVM.valueType = EnumHelper.DescriptionAttr((EcomVoucherValueTypeEnum)TblEcomVoucher.ValueType.Value);
                                respVM.FixedValue = TblEcomVoucher.FixedValue;
                                respVM.Percent = TblEcomVoucher.Percentage;
                                respVM.MaxDiscount = TblEcomVoucher.PercLimit;
                                respVM.Message = "Success";
                                flag = true;
                            }
                        }
                        else if (TblEcomVoucher.EcomVoucherType == Convert.ToInt32(EcomVoucherTypeEnum.PhoneSpecificVoucher))
                        {
                            if (string.IsNullOrEmpty(priceDC.PhoneNo))
                            {
                                respVM.Message = "Phone number is required for this voucher.";
                            }
                            else if ((!string.IsNullOrEmpty(TblEcomVoucher.CategoryIds) && priceDC.CategoryId == null) ||
                                     (TblEcomVoucher.BrandId.HasValue && priceDC.BrandId == null))
                            {
                                if (!string.IsNullOrEmpty(TblEcomVoucher.CategoryIds) && priceDC.CategoryId == null)
                                {
                                    respVM.Message = "Category is required for this voucher.";
                                }
                                else if (TblEcomVoucher.BrandId.HasValue && priceDC.BrandId == null)
                                {
                                    respVM.Message = "Brand is required for this voucher.";
                                }
                            }
                            else if (!string.IsNullOrEmpty(TblEcomVoucher.CategoryIds) &&
                                     !TblEcomVoucher.CategoryIds.Split(',').Select(int.Parse).Contains(priceDC.CategoryId.Value))
                            {
                                respVM.Message = "This voucher is not valid for the provided category.";
                            }
                            else if (TblEcomVoucher.BrandId.HasValue &&
                                     TblEcomVoucher.BrandId.Value != priceDC.BrandId)
                            {
                                respVM.Message = "This voucher is not valid for the provided brand.";
                            }
                            else if (childVoucher == null)
                            {
                                respVM.Message = "Invalid voucher for phone-specific type.";
                            }
                            else
                            {
                                string key = ConfigurationManager.AppSettings["ERPEncryptionKey"];
                                string encryptedPhone = StringSecurity.EncryptString(priceDC.PhoneNo, key);
                               

                                if (childVoucher.Phoneno == encryptedPhone)
                                {
                                    respVM.valueType = EnumHelper.DescriptionAttr((EcomVoucherValueTypeEnum)TblEcomVoucher.ValueType.Value);
                                    respVM.FixedValue = TblEcomVoucher.FixedValue;
                                    respVM.Percent = TblEcomVoucher.Percentage;
                                    respVM.MaxDiscount = TblEcomVoucher.PercLimit;
                                    respVM.Message = "Success";
                                    flag = true;

                                    tblEcomPhoneSpecific tblEcomPhoneSpecific = _ecomPhoneSpecificsRepository.GetSingle(x => x.IsActive == true && x.VoucherCode == priceDC.VoucherCode);
                                    if (tblEcomPhoneSpecific != null)
                                    {
                                        tblEcomPhoneSpecific.ModifiedDate = _currentDatetime;
                                        tblEcomPhoneSpecific.Voucherstatus = EcomVoucherStatusEnum.Captured.ToString();
                                        _ecomPhoneSpecificsRepository.Update(tblEcomPhoneSpecific);
                                        _ecomPhoneSpecificsRepository.SaveChanges();
                                    }
                                }
                                else
                                {
                                    respVM.Message = "This voucher is not valid for the provided phone number.";
                                }
                            }
                        }
                        else
                        {
                            respVM.Message = "Invalid voucher.";
                        }

                        if (flag)
                        {
                            TblEcomVoucher.voucherstatus = EcomVoucherStatusEnum.Captured.ToString();
                            TblEcomVoucher.ModifiedDate = _currentDatetime;
                            _ecomVoucherRepository.Update(TblEcomVoucher);
                            _ecomVoucherRepository.SaveChanges();
                        }
                    }
                    else
                    {
                        respVM.Message = "Invalid or inactive voucher.";
                    }
                }
                
else
                {
                    respVM.Message = "Invalid company.";
                }

                // 🔄 FINAL MASTER TABLE UPDATE
                if (flag)
                {
                    TblEcomVoucher.voucherstatus = EcomVoucherStatusEnum.Captured.ToString();
                    TblEcomVoucher.ModifiedDate = _currentDatetime;
                    TblEcomVoucher.IsUsed = true;

                    _ecomVoucherRepository.Update(TblEcomVoucher);
                    _ecomVoucherRepository.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EcomVoucherManager", "GetEcomVoucherPriceDetail", ex);
            }
            return respVM;
        }


        /// <summary>
        /// method to voucher redemption
        /// </summary>
        /// <param name="priceDC"></param>
        /// <returns></returns>
        public EcomVoucherRedemptionDataContract ManageEcomVoucherRedemption(EcomVoucherRedemptionDataContract redemDC, int? companyid)
        {
            EcomVoucherRedemptionDataContract redempDC = new EcomVoucherRedemptionDataContract();
            tblEcomVoucher TblEcomVoucher = null;
            _ecomVoucherRepository = new EcomVoucherRepository();

          
            try
            {
                //if (redemDC.VoucherCode != null && redemDC.IsPayment == true)
                //{
                //    TblEcomVoucher = _ecomVoucherRepository.GetSingle(x => x.IsActive == true && x.VoucherCode == redemDC.VoucherCode && x.IsUsed != true && companyid == companyid);

                //    if (TblEcomVoucher != null)
                //    {
                //        TblEcomVoucher.voucherstatus = EcomVoucherStatusEnum.Redeemed.ToString();
                //        TblEcomVoucher.ModifiedDate = _currentDatetime;
                //        _ecomVoucherRepository.Update(TblEcomVoucher);
                //        _ecomVoucherRepository.SaveChanges();

                //        redempDC.Message = "voucher redemption successful.";
                //        redempDC.Sucess = true;
                //    }
                //}
                //else
                //{
                //    redempDC.Message = "Something went wrong.";
                //    redempDC.Sucess = false;

                //}

                // First: check in PhoneSpecific table
                var phoneSpecific = _ecomVoucherRepository
                    .GetSingle(x => x.VoucherCode == redemDC.VoucherCode && x.IsActive == true);

                if (phoneSpecific != null && phoneSpecific.EcomVoucherId !=null)
                {
                    TblEcomVoucher = _ecomVoucherRepository.GetSingle(x =>
                        x.EcomVoucherId == phoneSpecific.EcomVoucherId &&
                        x.IsActive == true &&
                        x.IsUsed != true &&
                        x.CompanyId == companyid);
                }
                else
                {
                    // Else: check in master table directly for other types
                    TblEcomVoucher = _ecomVoucherRepository.GetSingle(x =>
                        x.VoucherCode == redemDC.VoucherCode &&
                        x.IsActive == true &&
                        x.IsUsed != true &&
                        x.CompanyId == companyid);
                }

                if (TblEcomVoucher != null)
                {
                    TblEcomVoucher.voucherstatus = EcomVoucherStatusEnum.Redeemed.ToString();
                    TblEcomVoucher.IsUsed = true;
                    TblEcomVoucher.ModifiedDate = _currentDatetime;

                    _ecomVoucherRepository.Update(TblEcomVoucher);
                    _ecomVoucherRepository.SaveChanges();


                    // If voucher is phone-specific, update child table as well
                    if (TblEcomVoucher.ValueType == (int)EcomVoucherTypeEnum.PhoneSpecificVoucher)
                    {
                        var phoneVoucher = _ecomPhoneSpecificsRepository.GetSingle(x =>
                            x.EcomVoucherId == TblEcomVoucher.EcomVoucherId && x.VoucherCode== redemDC.VoucherCode &&
                            x.IsUsed != true);

                        if (phoneVoucher != null)
                        {
                            phoneVoucher.Voucherstatus = EcomVoucherStatusEnum.Redeemed.ToString();
                            phoneVoucher.IsUsed = true;
                            phoneVoucher.ModifiedDate = _currentDatetime;
                            _ecomPhoneSpecificsRepository.Update(phoneVoucher);
                            _ecomPhoneSpecificsRepository.SaveChanges();

                        }
                    }

                    redemDC.Message = "Voucher redemption successful.";
                    redemDC.Sucess = true;
                }
                else
                {
                    redemDC.Message = "Invalid or already used voucher.";
                    redemDC.Sucess = false;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EcomVoucherManager", "ManageEcomVoucherRedemption", ex);
            }
            return redempDC;
        }


        public string GenerateVoucher()
        {
            string code = null;

            try
            {
                code = "V" + UniqueString.RandomNumberByLength(8);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EcomVoucherManager", "GenerateVoucher", ex);
            }

            return code;
        }

    }
}
