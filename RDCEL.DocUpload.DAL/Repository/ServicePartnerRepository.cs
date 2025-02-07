using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL.AbstractRepository;
using RDCEL.DocUpload.DAL.Helper;

namespace RDCEL.DocUpload.DAL.Repository
{
   public class ServicePartnerRepository:AbstractRepository<tblServicePartner>
   {
        /// <summary>
        /// Method to get the list of service Partners
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetServicePartnerList()
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                dt = obj.ExecuteDataTableWithoutParam("sp_GetListOfAllServicePartners");
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

    }
}
