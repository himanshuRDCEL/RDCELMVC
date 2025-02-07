using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ExchangeOrderDetails;

namespace RDCEL.DocUpload.BAL.SweetenerManager
{
    public class ManageSweetener
    {
        #region  variable declaration
        Logging logging;
        BusinessPartnerRepository _businessPartnerRepository;
        ModelNumberRepository _modalnumberRepository;
        ModalMappingRepository _modalMappingRepository;
        #endregion

        #region sweetener common method
        public SweetenerDataContract GetSweetenerAmtExchange(GetSweetenerDetailsDataContract details)
        {
            logging = new Logging();
            SweetenerDataContract sweetenerDC = new SweetenerDataContract();
            try
            {
                if (details.IsSweetenerModalBased == true)
                {
                    sweetenerDC = GetModalBasedSweetener(details);
                }
                else
                {
                    sweetenerDC = GetBasicSweetener(details);
                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("ManageSweetener", "GetSweetenerAmtExchange", ex);
            }
            return sweetenerDC;
        }
        #endregion

        #region Modal based sweetener method
        public SweetenerDataContract GetModalBasedSweetener(GetSweetenerDetailsDataContract details)
        {
            logging = new Logging();
            SweetenerDataContract sweetenerDC = new SweetenerDataContract();
            tblModelMapping modalmappingObj = new tblModelMapping();
            tblModelNumber modelnumberObj = new tblModelNumber();
            _modalMappingRepository = new ModalMappingRepository();
            _modalnumberRepository = new ModelNumberRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            try
            {
                if (details.ModalId > 0)
                {
                    //code to check if selected modal is absolute modal and not default
                    modalmappingObj = _modalMappingRepository.GetSingle(x=>x.ModelId==details.ModalId && x.IsActive==true && x.IsDefault==false && x.BusinessUnitId== details.BusinessUnitId && x.BusinessPartnerId==details.BusinessPartnerId);
                    if (modalmappingObj != null)
                    {
                        sweetenerDC.SweetenerBu = modalmappingObj.SweetenerBu!=null ? modalmappingObj.SweetenerBu:0;
                        sweetenerDC.SweetenerBP = modalmappingObj.SweetenerBP!=null? modalmappingObj.SweetenerBP:0;
                        sweetenerDC.SweetenerDigi2L = modalmappingObj.SweetenerDigi2l!=null ?modalmappingObj.SweetenerDigi2l:0;
                        sweetenerDC.SweetenerTotal = sweetenerDC.SweetenerDigi2L + sweetenerDC.SweetenerBu + sweetenerDC.SweetenerBP;
                    }
                    else
                    {
                        sweetenerDC.ErrorMessage = "No modelfound for this order in mapping table";
                    }
                }
                //check if modal is default or not
                ///summary 
                ///<>for default modal for the required bu we will change flow of getting default modal.
                ///first we will check in modal number table for default models added for that bu 
                ///and then search that modal id mapping table <>
                else
                {
                    modelnumberObj = _modalnumberRepository.GetSingle(x => x.IsActive == true && x.IsDefaultProduct == true && x.ProductCategoryId==details.NewProdCatId && x.ProductTypeId==details.NewProdTypeId && x.BusinessUnitId==details.BusinessUnitId);
                    if(modelnumberObj!=null)
                    {
                        modalmappingObj = _modalMappingRepository.GetSingle(x=>x.IsActive==true && x.IsDefault==true && x.ModelId==modelnumberObj.ModelNumberId && x.BusinessUnitId==details.BusinessUnitId && x.BusinessPartnerId==details.BusinessPartnerId);
                        if (modalmappingObj != null)
                        {
                            sweetenerDC.SweetenerBu = modalmappingObj.SweetenerBu!=null? modalmappingObj.SweetenerBu:0;
                            sweetenerDC.SweetenerBP = modalmappingObj.SweetenerBP != null ? modalmappingObj.SweetenerBP : 0;
                            sweetenerDC.SweetenerDigi2L = modalmappingObj.SweetenerDigi2l != null ? modalmappingObj.SweetenerDigi2l : 0;
                            sweetenerDC.SweetenerTotal = sweetenerDC.SweetenerDigi2L + sweetenerDC.SweetenerBu + sweetenerDC.SweetenerBP;
                        }
                        else
                        {
                            sweetenerDC.ErrorMessage = "No modelfound for this order in mapping table";
                        }
                    }
                    else
                    {
                        sweetenerDC.ErrorMessage = "No modelfound for this order in master table";
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ManageSweetener", "GetModalBasedSweetener", ex);
            }
            return sweetenerDC;
        }
        #endregion

        #region Basic sweetener calculation
        public SweetenerDataContract GetBasicSweetener(GetSweetenerDetailsDataContract details)
        {
            logging = new Logging();
            SweetenerDataContract sweetenerDC = new SweetenerDataContract();
            _businessPartnerRepository = new BusinessPartnerRepository();
            tblBusinessPartner businsessPartnerObj = new tblBusinessPartner();
            try
            {
                businsessPartnerObj = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.IsExchangeBP == true && x.BusinessPartnerId == details.BusinessPartnerId);
                if (businsessPartnerObj != null)
                {
                    sweetenerDC.SweetenerBu = businsessPartnerObj.SweetenerBU!=null ? businsessPartnerObj.SweetenerBU :0;
                    sweetenerDC.SweetenerBP = businsessPartnerObj.SweetenerBP != null ? businsessPartnerObj.SweetenerBP :0 ;
                    sweetenerDC.SweetenerDigi2L = businsessPartnerObj.SweetenerDigi2l != null ? businsessPartnerObj.SweetenerDigi2l : 0;
                    sweetenerDC.SweetenerTotal = sweetenerDC.SweetenerBu + sweetenerDC.SweetenerBP + sweetenerDC.SweetenerDigi2L;
                }
                else
                {
                    sweetenerDC.ErrorMessage = "No data found for this business partner in business partner table";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ManageSweetener", "GetModalBasedSweetener", ex);
            }
            return sweetenerDC;
        }
        #endregion
    }
}
