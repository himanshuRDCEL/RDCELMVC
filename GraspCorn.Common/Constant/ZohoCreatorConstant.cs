using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Constant
{
    public class ZohoCreatorConstant
    {

    }
    public class ZohoCreatorAPICallURL
    {
        public ZohoCreatorAPICallURL()
        {

        }

        /// <summary>
        /// Method to setup the URL 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetURLFor(string url, string reportLinkName, string filterCriterial = "")
        {
            url = url.Replace("[CreatorBaseURL]", ConfigurationManager.AppSettings["CreatorBaseURL"].ToString());
            url = url.Replace("[AccountOwnerName]", ConfigurationManager.AppSettings["AccountOwnerName"].ToString());
            url = url.Replace("[ApplicationLinkName]", ConfigurationManager.AppSettings["ApplicationLinkName"].ToString());
            url = url.Replace("[ReportLinkName]", reportLinkName);
            if (!string.IsNullOrEmpty(filterCriterial))
                url = url.Replace("[Filter]", filterCriterial);
            return url;
        }

        // get records from zoho with filters
        public const string GetUrlWithFilter = "https://[CreatorBaseURL]/api/v2/[AccountOwnerName]/[ApplicationLinkName]/report/[ReportLinkName][Filter]";

        //get records from zoho without filters
        public const string GetReportWithoutFilter = "https://[CreatorBaseURL]/api/v2/[AccountOwnerName]/[ApplicationLinkName]/report/[ReportLinkName]";

        // post details into zoho
        public const string AddDetails = "https://[CreatorBaseURL]/api/v2/[AccountOwnerName]/[ApplicationLinkName]/form/[ReportLinkName]";

        //"https://creatorapp.zoho.com/api/v2/infobyd_digimart/digi2l/form/Sponsor"//

    }
    public class FilterConstant
    {
        public const string Sponser_filter = "?from=[FromCount]&limit=200";
        public const string SponserbyIdForUpdate_filter = "/[sponsorId]";
        public const string SponserbyId_filter = "?ID=[sponsorId]";
        public const string SponserbyOrderNo_filter = "?Sp_Order_No=[sponsorOrderNo]";
        public const string SponserbyOrderby_TicketNo_filter = "?LGC_Tkt_No=[bizlogTicketNo]";
        public const string Evc_Allocation_RNo_filter = "?criteria=(Regd_No=[sponserID])";
        public const string Evc_Masters_Id_filter = "?criteria=(ID=[evcID])";
        public const string Evc_Id_filter = "/[EVCID]";
        public const string ABB_Id_filter = "?criteria=(ID=[ABBID])";
        public const string Store_Filter_By_Code = @"?criteria=(Store_Code=""[StoreCode]"")";
    }
    public class ReportLinkNameConstant
    {
        public const string Sponser_report = "All_Sponsors";
        public const string Customer_details_report = "customer_details";
        //public const string Evc_Allocation_report = "Copy_of_All_Call_Allocations";
        public const string Evc_Allocation_report = "All_Call_Allocations";
        public const string All_Evc_Masters_report = "All_Evc_Masters";
        public const string All_Program_Master_report = "All_Program_Master";
        public const string All_Abb_Price_Masters_report = "All_Abb_Price_Masters";
        public const string All_Sub_Category_sponsor_report = "Product_Group_With_Long_Name";
        public const string All_Category_sponsor_report = "Product_Group_Category_sponsor_Report";
        public const string All_Price_Master_Report = "Price_Master_Report";
        public const string All_Brand_Master_Report = "Brand_Master_Report";
        public const string Product_Size_Report = "Old_Product_Size_Report";       
        public const string Store_Code_Master_Report = "Store_Code_Master_Report";
        public const string BSH_Stores_Abb_Table = "BSH_Stores_Abb_Table";
        public const string Pin_Code_Master_Report = "Pin_Code_Master_Report";

    }
    public class FormLinkNameConstant
    {
        public const string Sponser_form = "Sponsor";
        public const string SponserStatus_form = "Import_Status";
        public const string ABBReg_form = "ABB_Registration";
        public const string EVC_Master_form = "EVC_Master";

    }

    public class SponsorDeliveryStatusConstant
    {
        public const string Delivered = "Delivered";
        public const string Cancelled = "Cancelled";


    }

}
