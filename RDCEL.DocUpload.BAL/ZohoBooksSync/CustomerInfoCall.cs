using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.DataContract.ZohoBooksModel;

namespace RDCEL.DocUpload.BAL.ZohoBooksSync
{
    public class CustomerInfoCall
    {
        #region Get all Customer detail
        /// <summary>
        /// Method to get all Customer detail from ZOho Books by organization id
        /// </summary>
        /// <returns></returns>
        public List<ContactData> GetAllContact(string orgId)
        {
            ContactListDataContract ContactDC = null;
            List<ContactData> finalContactList = new List<ContactData>();
            IRestResponse response = null;
            //string baseURL = "https://books.zoho.com/api/v3/contacts?organization_id=650516721&page=[pageNo]&per_page=200";
              string baseURL = "https://books.zoho.com/api/v3/contacts?organization_id="+ orgId + "&page=[pageNo]&per_page=200";
            int limit = 200;
            int frm = 1;
            
            try
            {
                for (int i = 1; i < limit; i++)
                {
                   string finalURL = baseURL.Replace("[pageNo]", i.ToString());

                    response = ZohoBooksServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(finalURL, Method.GET, null);

                if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            ContactDC = JsonConvert.DeserializeObject<ContactListDataContract>(response.Content);
                        }
                    }
                    if (ContactDC != null && ContactDC.contacts.Count > 0 && response != null && response.StatusCode == HttpStatusCode.OK)

                        finalContactList.AddRange(ContactDC.contacts);
                    else
                        break;

                    frm = i;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("CustomerInfoCall", "GetAllContact", ex);
            }
            return finalContactList;
        }

        #endregion

    }
}
