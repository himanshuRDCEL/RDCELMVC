using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.MahindraApicall;
using RDCEL.DocUpload.BAL.MahindraSyncCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.LogisticsDetails;
using RDCEL.DocUpload.DataContract.MahindraLogistics;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.ProcessAPI
{
    public class MahindraLogisticsSyncManager
    {
        #region Variable Declaration
        MahindraInformationCall _mahindraApiCall;
        CreateOrderMahindra _CreateOrderForMahindra;
        ProductTypeRepository productTypeRepository;
        ProductCategoryRepository productCategoryRepository;
        ExchangeOrderRepository exchangeOrderRepository;
        BusinessPartnerRepository businessPartnerRepository;
        #endregion
        #region Submit request for Service
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public MahindraLogisticsResponseDataContract ProcessLogisticsRequest(MahindraLogisticsDataContract mahindraDC)
        {
            _mahindraApiCall = new MahindraInformationCall();
            _CreateOrderForMahindra = new CreateOrderMahindra();
            MahindraLogisticsResponseDataContract _mahindraResponseDC = null;
            try
            {
                if (mahindraDC != null)
                {
                    _mahindraResponseDC = _mahindraApiCall.PlaceSingleOrder(mahindraDC);
                    if (_mahindraResponseDC != null)
                    {
                        #region Code to Add Submit request to Database
                        if (_mahindraResponseDC.awbNumber >0 /*&& _mahindraResponseDC.status != null*/)
                        {
                            string BookingId = _mahindraResponseDC.awbNumber.ToString();
                            _CreateOrderForMahindra.AddMahindraRequestToDB(mahindraDC, BookingId);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MahindraLogisticsSyncManager", "ProcessServiceRequest", ex);
            }
            return _mahindraResponseDC;
        }
        #endregion

        #region Set SubmitRequest Object
        public MahindraLogisticsDataContract SetMahindraObj(CustomerandOrderDetailsDataContract customerObj, EVCdetailsDataContract evcObj)
        {
            _mahindraApiCall = new MahindraInformationCall();
            MahindraLogisticsDataContract mahindraObj = null;
            LineItem lineItems = new LineItem();
            exchangeOrderRepository = new ExchangeOrderRepository();
            productTypeRepository = new ProductTypeRepository();
            productCategoryRepository = new ProductCategoryRepository();
            businessPartnerRepository = new BusinessPartnerRepository();
            try
            {

                mahindraObj = new MahindraLogisticsDataContract();
                mahindraObj.line_items = new List<LineItem>();
                mahindraObj.pickUpAddress = new PickUpAddress();
                mahindraObj.address = new Address();
                if (customerObj.FirstName != null&& customerObj.LastName!=null)
                {
                    mahindraObj.pickUpAddress.first_name = customerObj.FirstName;  //Mandatory
                    mahindraObj.pickUpAddress.last_name = customerObj.LastName;
                }
                mahindraObj.pickUpAddress.city = customerObj.city;                //Mandatory
                mahindraObj.pickUpAddress.state = customerObj.state;   //Manadatory
                mahindraObj.pickUpAddress.street_address = customerObj.Address1 + " " + customerObj.Address2;                  //Mandatory
                mahindraObj.pickUpAddress.telephone = customerObj.PhoneNumber;  //Mandatory
                mahindraObj.pickUpAddress.zipcode = customerObj.Pincode;
                mahindraObj.pickUpAddress.email = customerObj.Email;
                mahindraObj.client_order_id = customerObj.RegdNo;
                mahindraObj.address.first_name = evcObj.ContactPersonName;
                mahindraObj.address.last_name = evcObj.ContactPersonName;
                mahindraObj.address.city = string.IsNullOrEmpty(evcObj.Evc_city) ? evcObj.Evc_city : evcObj.Evc_city;
                mahindraObj.address.state = string.IsNullOrEmpty(evcObj.Evc_state) ? evcObj.Evc_state : evcObj.Evc_state;
                mahindraObj.address.street_address = string.IsNullOrEmpty(evcObj.Evc_Address1) ? evcObj.Evc_Address1 + " , " + evcObj.Evc_Address2 : evcObj.Evc_Address1 + " , " + evcObj.Evc_Address2;
                mahindraObj.address.telephone = evcObj.Evc_PhoneNumber;
                mahindraObj.address.zipcode = string.IsNullOrEmpty(evcObj.Evc_pincode) ? evcObj.Evc_pincode : evcObj.Evc_pincode;
                mahindraObj.address.email = evcObj.Evc_Email;


                DateTime myDate = DateTime.Now.AddHours(24);
                mahindraObj.order_delivery_date = myDate.ToString("yyyy-MM-dd");
                mahindraObj.quantity = 1;
                lineItems.sku = customerObj.RegdNo;
                lineItems.name = customerObj.ProductCategory + " , " + customerObj.ProductType;
                if (customerObj.IsDeffered == true)
                {
                    lineItems.price_per_each_item = customerObj.productCost;
                    lineItems.total_price = customerObj.productCost != null  ? Convert.ToDouble(customerObj.productCost) : Convert.ToDouble(customerObj.productCost);
                    mahindraObj.total_price = customerObj.productCost != null ? Convert.ToDouble(customerObj.productCost) : Convert.ToDouble(customerObj.productCost);
                }
                else
                {
                    lineItems.price_per_each_item = "0";
                    lineItems.total_price = 0;
                    mahindraObj.total_price = 0;
                }
                lineItems.quantity = 1;
                mahindraObj.line_items.Add(lineItems);


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MahindraLogisticsSyncManager", "SetMahindraObj", ex);
            }

            return mahindraObj;
        }
        #endregion
    }
}
