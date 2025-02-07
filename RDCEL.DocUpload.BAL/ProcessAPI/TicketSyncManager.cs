using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.BizlogApiCall;
using RDCEL.DocUpload.BAL.SyncCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.Bizlog;
using RDCEL.DocUpload.DataContract.LogisticsDetails;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.ProcessAPI
{
    public class TicketSyncManager
    {
        #region Variable Declaration
        TicketInformationCall _ticketInformationCall;
        BizlogCancelTicketSyncCall _bizlogCancelTicketSyncCall;
        BizlogCreateTicketSyncCall _bizlogCreateTicketSyncCall;
        BizlogTicketStatusSyncCall _bizlogTicketStatusSyncCall;
        ProductTypeRepository productTypeRepository;
        ProductCategoryRepository productCategoryRepository;
        ExchangeOrderRepository exchangeOrderRepository;
        BusinessPartnerRepository businessPartnerRepository;
        CustomerDetailsRepository _customerDetailsRepository;
        CityMasterRepository _cityMasterRepository;
        StateMasterRepository _stateMasterRepository;
        ABBRegistrationRepository _abbregistrationRepository;
        LogisticsRepository logisticsRepository;
        WalletTransactioRepository _walletTransactionRepository;
        ExchangeABBStatusHistoryRepository _statusHistoryRepository;
        AbbRedemptionRepository _abbredemptionRepository;
        OrderTransactionRepository orderTransactionRepository;
        OrderLGC_Repository _orderLGCRepository;

        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();

        #endregion

        #region sync add ticket details
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public TicketResponceDataContract ProcessTicketInfo(TicketDataContract ticketDataContract)
        {
            _ticketInformationCall = new TicketInformationCall();
            _bizlogCreateTicketSyncCall = new BizlogCreateTicketSyncCall();
            TicketResponceDataContract TicketResponceDC = null;

            try
            {

                if (ticketDataContract != null)
                {
                    //Create bizlog ticket with API call
                    TicketResponceDC = _ticketInformationCall.AddTicketToBizlog(ticketDataContract);
                    //get bizlogticketno as response and save it in local DB
                    //add status column in local DB
                    #region Code to add Ticket in database 
                    if (TicketResponceDC != null)
                    {
                        if (TicketResponceDC.ticketNo != null && TicketResponceDC.success == true && TicketResponceDC.ticketNo.Count > 0)
                        {
                            string BizlogTicketNo = TicketResponceDC.ticketNo[0];
                            _bizlogCreateTicketSyncCall.AddTicketToDB(ticketDataContract, BizlogTicketNo);
                        }
                    }

                    #endregion

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "ProcessTicketInfo", ex);
            }

            return TicketResponceDC;
        }
        #endregion

        #region sync cancel ticket details
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public TicketCancelResponseDataContract ProcessCancelTicketInfo(string ticketNo)
        {
            int result = 0;
            _ticketInformationCall = new TicketInformationCall();
            _bizlogCancelTicketSyncCall = new BizlogCancelTicketSyncCall();
            TicketCancelResponseDataContract ticketCancelResponceDC = null;

            try
            {
                if (ticketNo != null)
                {
                    //Cancel bizlog ticket with API call
                    ticketCancelResponceDC = _ticketInformationCall.CancelTicketToBizlog(ticketNo);
                    //Change ticket status in local DB
                    #region Code to add Ticket in database 
                    if (ticketCancelResponceDC != null)
                    {
                        if (ticketCancelResponceDC.success == true)
                        {
                            result = _bizlogCancelTicketSyncCall.CancelTicketToDB(ticketNo);
                            //string s = result.ToString();
                            //LibLogging.WriteErrorToDB(s, "ProcessCancelTicketInfo");
                        }
                    }

                    #endregion

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "ProcessCancelTicketInfo", ex);
            }

            return ticketCancelResponceDC;
        }
        #endregion

        #region sync ticket status details
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public string ProcessTicketStatusInfo(TicketStatusDataContract ticketStatusDataContract)
        {
            _ticketInformationCall = new TicketInformationCall();
            _bizlogTicketStatusSyncCall = new BizlogTicketStatusSyncCall();
            string tickeNo = null;

            try
            {
                if (ticketStatusDataContract != null)
                {

                    //Change ticket status in local DB
                    #region Code to add Ticket in database 

                    tickeNo = _bizlogTicketStatusSyncCall.TicketStatusInfoToDB(ticketStatusDataContract);

                    #endregion

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "ProcessCancelTicketInfo", ex);
            }

            return tickeNo;
        }
        #endregion

        #region set TicketDataContract object from zoho SponserSponserListDataContract 
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public TicketDataContract SetTicketObjInfo(SponserData sponserObj, EvcApprovedData evcApprovedObj, EvcAllocationReportData EvcAllocationObj)
        {
            _ticketInformationCall = new TicketInformationCall();
            TicketDataContract bizlogTicketObj = null;

            try
            {
                if (sponserObj != null && evcApprovedObj != null && EvcAllocationObj != null)
                {

                    bizlogTicketObj = new TicketDataContract();
                    if (sponserObj.Customer_Name != null)
                    {
                        bizlogTicketObj.consumerName = sponserObj.Customer_Name.first_name + " " + sponserObj.Customer_Name.last_name;  //Mandatory
                    }
                    bizlogTicketObj.consumerComplaintNumber = "123456";
                    bizlogTicketObj.addressLine1 = sponserObj.Customer_Address_1;          //Mandatory
                    bizlogTicketObj.addressLine2 = sponserObj.Customer_Address_2;           //Mandatory
                    bizlogTicketObj.city = sponserObj.Customer_City;                    //Mandatory
                    bizlogTicketObj.pincode = sponserObj.Customer_Pincode;                 //Mandatory
                    bizlogTicketObj.telephoneNumber = sponserObj.Customer_Mobile;         //Mandatory
                    bizlogTicketObj.retailerPhoneNo = !string.IsNullOrEmpty(sponserObj.Retailer_Phone_Number) ? sponserObj.Retailer_Phone_Number : "8123456789";//  sponserObj.;
                                                                                                                                                                // bizlogTicketObj.alternateTelephoneNumber = 
                    bizlogTicketObj.emailId = sponserObj.Customer_Email_Address;

                    bizlogTicketObj.orderNumber = sponserObj.Sp_Order_No;               //Mandatory
                    if (sponserObj.Old_Brand != null)
                    {
                        bizlogTicketObj.brand = sponserObj.Old_Brand.display_value;                     // Mandatory
                    }

                    //bizlogTicketObj.productCategory = "mobile";          // Mandatory
                    //bizlogTicketObj.productCategory = sponserObj.Product_Category;          // Mandatory
                    if (sponserObj.New_Prod_Group != null)
                    {
                        bizlogTicketObj.productCategory = sponserObj.New_Prod_Group.display_value;          // Mandatory
                    }
                    if (sponserObj.New_Product_Technology != null)
                    {
                        bizlogTicketObj.productName = sponserObj.New_Product_Technology.display_value;     // Mandatory
                        if (sponserObj.Size != null)
                        {
                            bizlogTicketObj.model = sponserObj.Size.display_value;      //doubt        // Mandatory
                        }
                        else
                        {
                            bizlogTicketObj.model = sponserObj.New_Product_Technology.display_value;      //doubt        // Mandatory
                        }

                    }


                    //bizlogTicketObj.identificationNo = sponserObj.Sr_No;
                    bizlogTicketObj.identificationNo = sponserObj.Regd_No;         // Mandatory


                    //bizlogTicketObj.dropLocAddress1 = evcApprovedObj.Regd_Address_Line_1;     // Mandatory
                    //bizlogTicketObj.dropLocAddress2 = evcApprovedObj.Regd_Address_Line_2;          // Mandatory
                    //bizlogTicketObj.dropLocCity = evcApprovedObj.City;               // Mandatory
                    //bizlogTicketObj.dropLocState = evcApprovedObj.State;              // Mandatory
                    //bizlogTicketObj.dropLocPincode = evcApprovedObj.PIN_Code;            // Mandatory
                    bizlogTicketObj.dropLocAddress1 = EvcAllocationObj.Address_Line_1;     // Mandatory
                    bizlogTicketObj.dropLocAddress2 = EvcAllocationObj.Address_Line_2;          // Mandatory
                    bizlogTicketObj.dropLocCity = EvcAllocationObj.City1;               // Mandatory
                    bizlogTicketObj.dropLocState = EvcAllocationObj.State;              // Mandatory
                    bizlogTicketObj.dropLocPincode = EvcAllocationObj.EVC_PIN_Code;            // Mandatory

                    bizlogTicketObj.dropLocation = evcApprovedObj.Bussiness_Name;             // Mandatory
                    bizlogTicketObj.dropLocContactPerson = evcApprovedObj.EVC_Name;       // Mandatory
                    bizlogTicketObj.dropLocContactNo = evcApprovedObj.EVC_Mobile_Number;          // Mandatory


                    bizlogTicketObj.retailerPhoneNo = !string.IsNullOrEmpty(sponserObj.Retailer_Phone_Number) ? sponserObj.Retailer_Phone_Number : "8123456789";//  sponserObj.;
                    if (sponserObj.Physical_Evolution != null)
                    {
                        bizlogTicketObj.physicalEvaluation = sponserObj.Physical_Evolution;            // Mandatory
                    }
                    else
                    {
                        bizlogTicketObj.physicalEvaluation = "No";              // Mandatory
                    }
                    // bizlogTicketObj.physicalEvaluation = sponserObj.Physical_Evolution;         // Mandatory
                    bizlogTicketObj.TechEvalRequired = !string.IsNullOrEmpty(sponserObj.Tech_Evl_Required) ? sponserObj.Tech_Evl_Required : "No";          // Mandatory;
                    // bizlogTicketObj.dateOfPurchase = sponserObj.
                    if (!string.IsNullOrEmpty(sponserObj.Date_Of_Complaint))
                    {
                        DateTime complaintDate = Convert.ToDateTime(sponserObj.Date_Of_Complaint);
                        string cDate = complaintDate.ToString("dd-MM-yyyy");
                        bizlogTicketObj.dateOfComplaint = cDate;        // Mandatory
                        bizlogTicketObj.dateOfPurchase = cDate;        // Mandatory
                    }
                    else
                    {
                        string cDate = DateTime.Now.ToString("dd-MM-yyyy");
                        bizlogTicketObj.dateOfComplaint = cDate;        // Mandatory
                        bizlogTicketObj.dateOfPurchase = cDate;        // Mandatory
                    }

                    bizlogTicketObj.levelOfIrritation = "1";        // Mandatory
                    bizlogTicketObj.natureOfComplaint = !string.IsNullOrEmpty(sponserObj.Nature_Of_Complaint) ? sponserObj.Nature_Of_Complaint : "Pick and Drop (One Way)";        // Mandatory
                    //bizlogTicketObj.natureOfComplaint = "Pick And Drop (One Way)";        // Mandatory
                    bizlogTicketObj.isUnderWarranty = !string.IsNullOrEmpty(sponserObj.Is_Under_Warranty) ? sponserObj.Is_Under_Warranty : "No";          // Mandatory
                    //bizlogTicketObj.value = sponserObj.Actual_Amt_payable_incl_Bonus;
                    bizlogTicketObj.value = sponserObj.Amount_Payable_Through_LGC;

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "SetTicketObjInfo", ex);
            }

            return bizlogTicketObj;
        }
        #endregion

        #region Updated Create ticket for bizlog
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public UpdatedTicketDataContract UpdatedSetTicketObjInfo(CustomerandOrderDetailsDataContract customerObj, EVCdetailsDataContract evcDataObj)
        {
            _ticketInformationCall = new TicketInformationCall();
            UpdatedTicketDataContract bizlogTicketObj = null;
            Product product = new Product();
            Primary primary = new Primary();
            List<Product> product1 = new List<Product>();
            exchangeOrderRepository = new ExchangeOrderRepository();
            productTypeRepository = new ProductTypeRepository();
            productCategoryRepository = new ProductCategoryRepository();
            businessPartnerRepository = new BusinessPartnerRepository();
            tblExchangeOrder Exchangeobj = new tblExchangeOrder();

            try
            {

                //bizlogTicketObj.products = new List<Product>();
                if (customerObj != null && evcDataObj != null)
                {
                    bizlogTicketObj = new UpdatedTicketDataContract();
                    bizlogTicketObj.primary = new Primary();
                    bizlogTicketObj.primary.ticketPriority = !string.IsNullOrEmpty(customerObj.pickupPriority) ? customerObj.pickupPriority : "high";
                    bizlogTicketObj.primary.flowId = "PickAndDropOneWay";
                    bizlogTicketObj.primary.retailerId = "d0eb6dd5-2f8f-4e39-ae3e-50631201e122";
                    bizlogTicketObj.primary.retailerNo = evcDataObj.Evc_PhoneNumber;
                    bizlogTicketObj.primary.conComplaintNo = customerObj.RegdNo;
                    bizlogTicketObj.primary.ticketDetails = "Fragile Products, handle with care";
                    bizlogTicketObj.primary.isPhysicalEval = "no";
                    bizlogTicketObj.primary.orderNo = customerObj.RegdNo;
                    bizlogTicketObj.primary.isTechEval = "no";


                    //Data in list format to pass to bizlog 
                    //product details
                    product.primary = new Primary();
                    DateTime complaintDate = Convert.ToDateTime(customerObj.OrderDate);
                    string cDate = complaintDate.ToString("yyyy-MM-dd");
                    product.primary.productCode = customerObj.ProductCategory;
                    product.primary.productName = customerObj.ProductCategory + " " + customerObj.ProductType;
                    product.primary.dateOfPurchase = cDate;
                    product.primary.identificationNo = customerObj.RegdNo;
                    if (customerObj.IsDeffered == true)
                    {
                        product.primary.productValue = customerObj.productCost;
                        product.primary.cost = customerObj.productCost;
                        bizlogTicketObj.primary.cost = customerObj.productCost;
                        bizlogTicketObj.primary.productValue = customerObj.productCost;
                    }
                    else
                    {
                        product.primary.productValue = "0";
                        product.primary.cost = "0";
                        bizlogTicketObj.primary.cost = "0";
                        bizlogTicketObj.primary.productValue = "0";
                    }
                    //product.src_add to Add in bizlog
                    product.src_add = new SrcAdd();
                    product.src_add.srcAdd1 = customerObj.Address1;
                    product.src_add.srcAdd2 = customerObj.Address2;
                    product.src_add.srcCity = customerObj.city;
                    product.src_add.srcContact1 = customerObj.PhoneNumber;
                    product.src_add.srcContactPerson = customerObj.FirstName + " " + customerObj.LastName;
                    product.src_add.srcEmailId = customerObj.Email;
                    product.src_add.srcLandmark = customerObj.Address1;
                    product.src_add.srcLocation = customerObj.city;
                    product.src_add.srcPincode = customerObj.Pincode;
                    product.src_add.srcState = customerObj.state;

                    // Destination Address for Bizlog
                    product.dst_add = new DstAdd();
                    product.dst_add.dstAdd1 = evcDataObj.Evc_Address1;
                    product.dst_add.dstAdd2 = evcDataObj.Evc_Address2;
                    product.dst_add.dstContactPerson = evcDataObj.ContactPersonName;
                    product.dst_add.dstContact1 = evcDataObj.Evc_PhoneNumber;
                    product.dst_add.dstContact2 = evcDataObj.Evc_AlternatePhoneNumber;
                    product.dst_add.dstEmailId = evcDataObj.Evc_Email;
                    product.dst_add.dstLandmark = evcDataObj.BusinessName;
                    product.dst_add.dstPincode = evcDataObj.Evc_pincode;
                    product.dst_add.dstState =  evcDataObj.Evc_state;
                    product.dst_add.dstCity = evcDataObj.Evc_city;

                    bizlogTicketObj.products = new List<Product>();
                    bizlogTicketObj.products.Add(product);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "SetTicketObjInfo", ex);
            }
            return bizlogTicketObj;
        }
        #endregion

        #region sync add ticket details New updated Integration
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public UpdatedTicketResponceDataContract UpdatedProcessTicketInfo(UpdatedTicketDataContract updatedticketDataContract)
        {
            _ticketInformationCall = new TicketInformationCall();
            _bizlogCreateTicketSyncCall = new BizlogCreateTicketSyncCall();
            UpdatedTicketResponceDataContract TicketResponceDC = null;

            try
            {
                var code = updatedticketDataContract.primary.productCode;
                if (updatedticketDataContract != null)
                {
                    //Create bizlog ticket with API call
                    TicketResponceDC = _ticketInformationCall.AddUpdatedTicketToBizlog(updatedticketDataContract);
                    //get bizlogticketno as response and save it in local DB
                    //add status column in local DB
                    #region Code to add Ticket in database 
                    if (TicketResponceDC != null)
                    {
                        if (TicketResponceDC.data.ticketNo != null && TicketResponceDC.success == true)
                        {
                            string BizlogTicketNo = TicketResponceDC.data.ticketNo;
                            _bizlogCreateTicketSyncCall.AddUpdatedTicketToDB(updatedticketDataContract, BizlogTicketNo);
                        }
                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "ProcessTicketInfo", ex);
            }

            return TicketResponceDC;
        }
        #endregion

        #region sync ticket status details
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public string ProcessawbNoMahindraLogistics(TicketStatusDataContract ticketStatusDataContract)
        {
            _ticketInformationCall = new TicketInformationCall();
            _bizlogTicketStatusSyncCall = new BizlogTicketStatusSyncCall();
            string tickeNo = null;

            try
            {
                if (ticketStatusDataContract != null)
                {

                    //Change ticket status in local DB
                    #region Code to add Ticket in database 

                    tickeNo = _bizlogTicketStatusSyncCall.TicketStatusMahindralogisticsToDB(ticketStatusDataContract);

                    #endregion

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "ProcessawbNoMahindraLogistics", ex);
            }

            return tickeNo;
        }
        #endregion


        #region Set SponserObject for customer detils
        public CustomerandOrderDetailsDataContract SetOrderDetailsObject(tblExchangeOrder exchangeOrder)
        {
            _customerDetailsRepository = new CustomerDetailsRepository();
            productCategoryRepository = new ProductCategoryRepository();
            productTypeRepository = new ProductTypeRepository();
            CustomerandOrderDetailsDataContract customerDC = new CustomerandOrderDetailsDataContract();
            try
            {
                if (exchangeOrder != null)
                {
                    tblCustomerDetail customerObj = _customerDetailsRepository.GetSingle(x => x.Id == exchangeOrder.CustomerDetailsId && x.IsActive==true);
                    if (customerObj != null)
                    {
                        customerDC.FirstName = customerObj.FirstName;
                        customerDC.LastName = customerObj.LastName;
                        customerDC.Email = customerObj.Email;
                        customerDC.Address1 = customerObj.Address1;
                        if(customerDC.Address1==null || customerDC.Address1 == "")
                        {
                            customerDC.Message = "customer address is not available for this order please check address details";
                        }
                        else if(string.IsNullOrEmpty(customerObj.PhoneNumber))
                        {
                            customerDC.Message = "customer phone number is not provided";
                        }
                        else if (string.IsNullOrEmpty(customerObj.City))
                        {
                            customerDC.Message = "customer city is not provided";
                        }
                        else if (string.IsNullOrEmpty(customerObj.ZipCode))
                        {
                            customerDC.Message = "customer pincode is not provided";
                        }
                        
                        customerDC.Address2 = customerObj.Address2;
                        customerDC.PhoneNumber = customerObj.PhoneNumber;
                        customerDC.state = customerObj.State;
                        customerDC.city = customerObj.City;
                        customerDC.Pincode = customerObj.ZipCode;
                        customerDC.RegdNo = exchangeOrder.RegdNo;
                        customerDC.OrderDate = exchangeOrder.CreatedDate.ToString();
                        customerDC.SponserOrdrNumber = exchangeOrder.SponsorOrderNumber;
                        customerDC.IsDeffered = Convert.ToBoolean(exchangeOrder.IsDefferedSettlement);
                        if (exchangeOrder.IsDefferedSettlement == true)
                        {
                            customerDC.productCost = exchangeOrder.FinalExchangePrice.ToString();
                            if (exchangeOrder.FinalExchangePrice == null)
                            {
                                customerDC.Message = "Final Price is 0 for this order and case is deffered";
                            }
                        }
                        else
                        {
                            customerDC.productCost = 0.ToString();
                        }
                        customerDC.IsDtoC = Convert.ToBoolean(exchangeOrder.IsDtoC);
                        tblProductType producttype = productTypeRepository.GetSingle(x => x.Id == exchangeOrder.ProductTypeId);
                        if (producttype != null)
                        {
                            tblProductCategory productCategory = productCategoryRepository.GetSingle(x => x.Id == producttype.ProductCatId);
                            if (productCategory != null)
                            {
                                customerDC.ProductCategory = productCategory.Description;
                                customerDC.ProductType = producttype.Description;
                            }
                            else
                            {
                                customerDC.Message = "Product category not found";
                            }
                        }
                        else
                        {
                            customerDC.Message = "product type not found";
                        }
                        
                    }
                    else
                    {
                        customerDC.Message = "Customer details not found";

                    }
                  

                }
                else
                {
                    customerDC.Message = "Order details not found";

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "SetOrderDetailsObject", ex);
            }
            return customerDC;
        }
        #endregion

        #region Set SponserObject for customer detils
        public CustomerandOrderDetailsDataContract SetOrderDetailsObjectForABB(tblABBRedemption aBBRedemption)
        {
            _customerDetailsRepository = new CustomerDetailsRepository();
            productCategoryRepository = new ProductCategoryRepository();
            productTypeRepository = new ProductTypeRepository();
            _abbregistrationRepository = new ABBRegistrationRepository();
            CustomerandOrderDetailsDataContract customerDC = new CustomerandOrderDetailsDataContract();
            try
            {
                tblABBRegistration abbRegistration = _abbregistrationRepository.GetSingle(x => x.ABBRegistrationId == aBBRedemption.ABBRegistrationId);
                if (abbRegistration != null)
                {
                    customerDC.FirstName = abbRegistration.CustFirstName;
                    customerDC.LastName = abbRegistration.CustLastName;
                    customerDC.Email = abbRegistration.CustEmail;
                    customerDC.Address1 = abbRegistration.CustAddress1;
                    customerDC.Address2 = abbRegistration.CustAddress2;
                    customerDC.PhoneNumber = abbRegistration.CustMobile;
                    customerDC.state = abbRegistration.CustState;
                    customerDC.city = abbRegistration.CustCity;
                    customerDC.Pincode = abbRegistration.CustPinCode;
                    customerDC.RegdNo = abbRegistration.RegdNo;
                    customerDC.OrderDate = abbRegistration.UploadDateTime.ToString();
                    customerDC.SponserOrdrNumber = abbRegistration.SponsorOrderNo;
                    customerDC.productCost = aBBRedemption.RedemptionValue.ToString();

                    if (string.IsNullOrEmpty(customerDC.Address1))
                    {
                        customerDC.Message = "customer address is not available for this order please check address details";
                    }
                    else if (string.IsNullOrEmpty(abbRegistration.CustMobile))
                    {
                        customerDC.PhoneNumber = "customer phone number is not provided";
                    }
                    else if (string.IsNullOrEmpty(abbRegistration.CustCity))
                    {
                        customerDC.Message = "customer city is not provided";
                    }
                    else if (string.IsNullOrEmpty(abbRegistration.CustPinCode))
                    {
                        customerDC.Message = "customer pincode is not provided";
                    }
                    customerDC.IsDeffered = true;
                    tblProductType producttype = productTypeRepository.GetSingle(x => x.Id == abbRegistration.NewProductCategoryTypeId);
                    if (producttype != null)
                    {
                        tblProductCategory productCategory = productCategoryRepository.GetSingle(x => x.Id == producttype.ProductCatId);
                        if (productCategory != null)
                        {
                            customerDC.ProductCategory = productCategory.Description;
                            customerDC.ProductType = producttype.Description;
                        }
                        else
                        {
                            customerDC.Message = "Product category not found";
                        }
                    }
                    else
                    {
                        customerDC.Message = "product type not found";
                    }
                }
                else
                {
                    customerDC.Message = "Order details not found";

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "SetOrderDetailsObjectForABB", ex);
            }
            return customerDC;
        }
        #endregion


        #region Set SponserObject for customer detils
        public EVCdetailsDataContract SetEvcDetailsDataContract(tblEVCRegistration evcRegistration)
        {
            _cityMasterRepository = new CityMasterRepository();
            _stateMasterRepository = new StateMasterRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            productCategoryRepository = new ProductCategoryRepository();
            productTypeRepository = new ProductTypeRepository();
            EVCdetailsDataContract EvcDataContract = new EVCdetailsDataContract();
            try
            {
                if (evcRegistration != null)
                {
                    EvcDataContract.EVCName = evcRegistration.BussinessName;
                    EvcDataContract.ContactPersonName = evcRegistration.ContactPerson;
                    EvcDataContract.Evc_Email = evcRegistration.EmailID;
                    EvcDataContract.Evc_PhoneNumber = evcRegistration.EVCMobileNumber;
                    EvcDataContract.Evc_AlternatePhoneNumber = evcRegistration.AlternateMobileNumber;
                    EvcDataContract.Evc_RegdNo = evcRegistration.EVCRegdNo;
                    EvcDataContract.Evc_pincode = evcRegistration.PinCode;
                    if (evcRegistration.StateId > 0)
                    {
                        tblState stateObj = _stateMasterRepository.GetSingle(x => x.StateId == evcRegistration.StateId);
                        if (stateObj != null)
                        {
                            EvcDataContract.Evc_state = stateObj.Name;
                        }
                    }
                    if (evcRegistration.CityId > 0)
                    {
                        tblCity cityObj = _cityMasterRepository.GetSingle(x => x.CityId == evcRegistration.CityId);
                        if (cityObj != null)
                        {
                            EvcDataContract.Evc_city = cityObj.Name;
                        }
                    }
                    EvcDataContract.Evc_Address1 = evcRegistration.RegdAddressLine1;
                    EvcDataContract.Evc_Address2 = evcRegistration.RegdAddressLine2;

                    if (string.IsNullOrEmpty(EvcDataContract.Evc_Address1))
                    {
                        EvcDataContract.Message = "evc address 1 is null please provide evc address details";
                    }
                    else if (string.IsNullOrEmpty(EvcDataContract.ContactPersonName))
                    {
                        EvcDataContract.Message = "contact person name is not provided from evc please provide contact person name";
                    }
                    else if (string.IsNullOrEmpty(EvcDataContract.Evc_PhoneNumber))
                    {
                        EvcDataContract.Message = "EVC phone number is null please provide evc phone number";
                    }
                    else if (string.IsNullOrEmpty(EvcDataContract.Evc_city))
                    {
                        EvcDataContract.Message = "EVC city is null please provide city";
                    }
                    else if (string.IsNullOrEmpty(EvcDataContract.Evc_Email))
                    {
                        EvcDataContract.Message = "Evc email is null please provide email";
                    }
                }
                else
                {
                    EvcDataContract.Message = "Evc data not found";

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "SetEvcDetailsDataContract", ex);
            }
            return EvcDataContract;
        }
        #endregion

        #region Add Data To Logistics Table
        public int AddTicketToTblLogistics(string ticketNumber, int transId, int servicepartnerId, string regdno,CustomerandOrderDetailsDataContract customerDetails)
        {
            exchangeOrderRepository = new ExchangeOrderRepository();
            orderTransactionRepository = new OrderTransactionRepository();
            _statusHistoryRepository = new ExchangeABBStatusHistoryRepository();
            _walletTransactionRepository = new WalletTransactioRepository();
            logisticsRepository = new LogisticsRepository();
            _abbredemptionRepository = new AbbRedemptionRepository();
            _orderLGCRepository = new OrderLGC_Repository();
            int LogisticsId = 0;
            int typeOfOrder = 0;
            
            try
            {
                if (ticketNumber != null&&regdno!=null && transId > 0 && servicepartnerId > 0)
                {
                    tblLogistic logistic = new tblLogistic();
                    logistic.TicketNumber = ticketNumber;
                    logistic.RegdNo = regdno;
                    logistic.CreatedDate = DateTime.Now;
                    logistic.ServicePartnerId = servicepartnerId;
                    logistic.IsActive = true;
                    logistic.OrderTransId = transId;
                    logistic.AmtPaybleThroughLGC =Convert.ToDecimal(customerDetails.productCost);
                    logistic.StatusId= Convert.ToInt32(StatusEnum.LogisticsTicketUpdated);
                    logisticsRepository.Add(logistic);
                    logisticsRepository.SaveChanges();
                    LogisticsId = logistic.LogisticId;
                    if (LogisticsId > 0)
                    {
                        tblWalletTransaction wallettramsactionObj = _walletTransactionRepository.GetSingle(x => x.OrderTransId == transId);
                        {
                            typeOfOrder =Convert.ToInt32(wallettramsactionObj.OrderType);
                            wallettramsactionObj.OrderOfInprogressDate = DateTime.Now;
                            wallettramsactionObj.ModifiedDate = DateTime.Now;
                            wallettramsactionObj.StatusId= Convert.ToInt32(StatusEnum.LogisticsTicketUpdated).ToString();
                            _walletTransactionRepository.Update(wallettramsactionObj);
                            _walletTransactionRepository.SaveChanges();
                        }
                        if (typeOfOrder == Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange))
                        {
                            tblExchangeOrder exchangeObj = exchangeOrderRepository.GetSingle(x => x.RegdNo == regdno);
                            if(exchangeObj!=null)
                            {
                              
                                exchangeObj.StatusId = Convert.ToInt32(StatusEnum.LogisticsTicketUpdated);
                                exchangeOrderRepository.Update(exchangeObj);
                                exchangeOrderRepository.SaveChanges();
                            }
                            tblOrderTran transobj = orderTransactionRepository.GetSingle(x => x.OrderTransId == transId);
                            if (transobj != null)
                            {
                                transobj.StatusId= Convert.ToInt32(StatusEnum.LogisticsTicketUpdated);
                                orderTransactionRepository.Update(transobj);
                                orderTransactionRepository.SaveChanges();
                            }
                            tblExchangeABBStatusHistory ordersatatusObj = new tblExchangeABBStatusHistory();
                            ordersatatusObj.CreatedDate = DateTime.Now;
                            ordersatatusObj.OrderType = typeOfOrder;
                            ordersatatusObj.RegdNo = exchangeObj.RegdNo;
                            ordersatatusObj.SponsorOrderNumber = exchangeObj.SponsorOrderNumber;
                            ordersatatusObj.StatusId = Convert.ToInt32(StatusEnum.LogisticsTicketUpdated);
                            ordersatatusObj.OrderTransId = transId;
                            ordersatatusObj.CustId =Convert.ToInt32(exchangeObj.CustomerDetailsId);
                            ordersatatusObj.CreatedBy = Convert.ToInt32(UserEnum.Admin);
                            ordersatatusObj.IsActive = true;
                            ordersatatusObj.CreatedDate = DateTime.Now;
                            _statusHistoryRepository.Add(ordersatatusObj);
                            _statusHistoryRepository.SaveChanges();
                        }
                        else
                        {
                            tblABBRedemption abbRedemption = _abbredemptionRepository.GetSingle(x => x.RegdNo == regdno);
                            if(abbRedemption!=null)
                            {

                                abbRedemption.ABBRedemptionStatus = Convert.ToInt32(StatusEnum.LogisticsTicketUpdated).ToString();
                                _abbredemptionRepository.Update(abbRedemption);
                                _abbredemptionRepository.SaveChanges();
                                
                            }
                            tblOrderTran transobj = orderTransactionRepository.GetSingle(x => x.OrderTransId == transId);
                            if (transobj != null)
                            {
                                transobj.StatusId = Convert.ToInt32(StatusEnum.LogisticsTicketUpdated);
                                orderTransactionRepository.Update(transobj);
                                orderTransactionRepository.SaveChanges();
                            }

                            tblExchangeABBStatusHistory ordersatatusObj = new tblExchangeABBStatusHistory();
                            ordersatatusObj.CreatedDate = DateTime.Now;
                            ordersatatusObj.OrderType = typeOfOrder;
                            ordersatatusObj.RegdNo = abbRedemption.RegdNo;
                            ordersatatusObj.StatusId = Convert.ToInt32(StatusEnum.LogisticsTicketUpdated);
                            ordersatatusObj.OrderTransId = transId;
                            ordersatatusObj.CustId = abbRedemption.RedemptionId;
                            ordersatatusObj.CreatedBy = Convert.ToInt32(UserEnum.Admin);
                            ordersatatusObj.IsActive = true;
                            ordersatatusObj.CreatedDate = DateTime.Now;
                            _statusHistoryRepository.Add(ordersatatusObj);
                            _statusHistoryRepository.SaveChanges();
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketSyncManager", "AddTicketToTblLogistics", ex);
            }
            return LogisticsId;
        }

        #endregion

    }
}
