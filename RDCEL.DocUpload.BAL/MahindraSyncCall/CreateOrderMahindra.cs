using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.MahindraLogistics;

namespace RDCEL.DocUpload.BAL.MahindraSyncCall
{
    public class CreateOrderMahindra
    {


        #region Variable Declaration
        MahindraLogisticsRepository mahindraLogisticsRepository;

        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();

        #endregion

        #region Add SubmitRequest in database
        /// <summary>
        /// Method to add the Ticket
        /// </summary>       
        /// <returns> </returns>   
        public int AddMahindraRequestToDB(MahindraLogisticsDataContract mahindraLogisticsDataContract, string mahindraLogisticsawbNo)
        {
            mahindraLogisticsRepository = new MahindraLogisticsRepository();
            int result = 0;
            try
            {
                tblMahindraLogistic mahindraObject = SetMahindraObjectDBJson(mahindraLogisticsDataContract, mahindraLogisticsawbNo);
                {
                    mahindraLogisticsRepository.Add(mahindraObject);
                    mahindraLogisticsRepository.SaveChanges();
                    result = mahindraObject.Id;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SubmitRequestSyncCall", "AddSubmitRequestToDB", ex);
            }

            return result;
        }
        #endregion

        #region set Add SubmitRequest obj
        /// <summary>
        /// Method to set Ticket info to table
        /// </summary>
        /// <param name="aroundDataContract">aroundDataContract</param>     
        public tblMahindraLogistic SetMahindraObjectDBJson(MahindraLogisticsDataContract mahindraLogisticsDataContract, string mahindraLogisticsawbNo)
        {
            tblMahindraLogistic mahindraObject = null;
            try
            {
                if (mahindraLogisticsDataContract != null)
                {
                    mahindraObject = new tblMahindraLogistic();
                    if (mahindraLogisticsawbNo != null)
                    {
                        mahindraObject.awbNumber = mahindraLogisticsawbNo;
                    }
                    mahindraObject.first_name = mahindraLogisticsDataContract.address.first_name;
                    mahindraObject.last_name = mahindraLogisticsDataContract.address.last_name; ;
                    mahindraObject.city = mahindraLogisticsDataContract.address.city;
                    mahindraObject.state = mahindraLogisticsDataContract.address.state;
                    mahindraObject.zipcode = mahindraLogisticsDataContract.address.zipcode;
                    mahindraObject.street_address = mahindraLogisticsDataContract.address.street_address;
                    mahindraObject.telephone = mahindraLogisticsDataContract.address.telephone;
                    mahindraObject.email = mahindraLogisticsDataContract.address.email;
                    mahindraObject.first_name_pickup = mahindraLogisticsDataContract.pickUpAddress.first_name;
                    mahindraObject.last_name_pickup = mahindraLogisticsDataContract.pickUpAddress.last_name;
                    mahindraObject.zipcode_pickup = mahindraLogisticsDataContract.pickUpAddress.zipcode;
                    mahindraObject.street_address_pickup = mahindraLogisticsDataContract.pickUpAddress.street_address;
                    mahindraObject.city_pickup = mahindraLogisticsDataContract.pickUpAddress.city;
                    mahindraObject.state_pickup = mahindraLogisticsDataContract.pickUpAddress.state;
                    mahindraObject.email_pickup = mahindraLogisticsDataContract.pickUpAddress.email;
                    //mahindraObject.longitude_pickup = mahindraLogisticsDataContract.pickUpAddress.location.longitude.ToString();
                    //mahindraObject.latitude_pickup = mahindraLogisticsDataContract.pickUpAddress.location.latitude.ToString();
                    mahindraObject.telephone_pickup = mahindraLogisticsDataContract.pickUpAddress.telephone;
                    mahindraObject.client_order_id = mahindraLogisticsDataContract.client_order_id;
                    mahindraObject.total_price = Convert.ToDecimal(mahindraLogisticsDataContract.total_price);
                    mahindraObject.quantity = mahindraLogisticsDataContract.quantity;


                    //add sponsor order id
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SubmitRequestSyncCall", "Set247AroundObjectDBJson", ex);
            }
            return mahindraObject;
        }
        #endregion
    }
}
