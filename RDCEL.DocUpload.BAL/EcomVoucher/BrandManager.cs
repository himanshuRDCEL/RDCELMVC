using GraspCorn.Common.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DataContract.ProductTaxonomy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RDCEL.DocUpload.BAL.EcomVoucher
{
    public class BrandManager
    {
        #region Variable declaration
       
        BrandRepository _brandRepository;
        CategoryRepository _categoryRepository;
        #endregion


        #region Ecom sa
        /// <summary>
        /// method to get brand list
        /// </summary>
        /// <param name="businessunitId"></param>
        /// <returns></returns>
        public List<BrandName> GetBrandListByBUId(int? businessunitId)
        {
            List<tblBrand> TblBrandList = null;
            List<BrandName> BrandDDL = new List<BrandName>();
            _brandRepository = new BrandRepository();
            try
            {
                TblBrandList = _brandRepository.GetList(x => x.BusinessUnitId == businessunitId && x.IsActive == true).ToList();
                if (TblBrandList != null && TblBrandList.Count > 0)
                {
                    TblBrandList = TblBrandList.Distinct().ToList();
                    BrandDDL = GenericMapper<tblBrand, BrandName>.MapList(TblBrandList);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BrandDDL;

        }

        /// <summary>
        /// METHOD TO GET Category list by brand id
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetCategoryListByBrandId(int? brandId)
        {
            List<tblCategory> TblCategoryList = null;
            List<SelectListItem> CategoryDDL = new List<SelectListItem>();
            _categoryRepository = new CategoryRepository();
            try
            {
                TblCategoryList = _categoryRepository.GetList(x => x.BrandId == brandId && x.IsActive == true).ToList();
                if (TblCategoryList != null && TblCategoryList.Count > 0)
                {
                    TblCategoryList = TblCategoryList.Distinct().ToList();
                    CategoryDDL = TblCategoryList.ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.Name,
                            Value = a.CategoryId.ToString(),
                            Selected = false
                        };
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return CategoryDDL;
        }


        #endregion
    }
}
